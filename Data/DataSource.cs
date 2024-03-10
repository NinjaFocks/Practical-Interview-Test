using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

public class DataSource : IDataSource
{
    private static SqliteConnection _connection;
    private int _pageSize = 10;

    public DataSource()
    {
        _connection = new SqliteConnection("");
    }

    public async Task<KeyValuePair<int,IList<HashModel>>> GetHashModelsAsync(string? search = null, int? pageSize = null, int pageNumber = 1)
    {
        pageSize = pageSize ?? _pageSize;

        //calculate offset
        var offset = (pageNumber * pageSize) - pageSize;

        //build hash count
        var countQuery = "SELECT count(*) FROM safe_hashes";
        countQuery += BuildQuery(search, pageSize, offset);

        //get hash count
        var pageCount = await GetPageCountAsync(countQuery);

        //build hash query
        var hashQuery = $"SELECT * FROM safe_hashes";
        hashQuery += BuildQuery(search, pageSize, offset);

        await _connection.OpenAsync();

        //get hashes
        SqliteCommand cmd = new SqliteCommand(hashQuery, _connection);
        var reader = await cmd.ExecuteReaderAsync();

        var hashes = new List<HashModel>();

        while (await reader.ReadAsync())
        {
            hashes.Add(new HashModel
            {
                Id = reader.GetInt32(0),
                Hash = reader.GetString(1)
            });
        }

        //close connections
        await reader.CloseAsync();
        await _connection.CloseAsync();

        //return data
        return new KeyValuePair<int, IList<HashModel>>(pageCount, hashes);
    }

    private string BuildQuery(string? search, int? pageSize, int? offset)
    {
        if (string.IsNullOrEmpty(search))
        {
            return $" ORDER BY hash_id ASC LIMIT {pageSize} OFFSET {offset}";
        }

        search = search.Trim().ToUpper();

        var query = "";
        if ( search.Contains(","))
        {            
            var searches = search.Split(',').Where(s => s != "").ToArray();

            query += " WHERE sha1 ";
            for (int i=0; i<searches.Count(); i++)
            {
                if (i == 0)
                {
                    query += $"LIKE '%{searches[i]}%'";
                }
                else
                {
                    query += $" OR sha1 LIKE '%{searches[i]}%'";
                }
                
            }
        } 
        else
        {
            query += $" WHERE sha1 LIKE '%{search}%'";
        }

        query += $" ORDER BY hash_id ASC LIMIT {pageSize} OFFSET {offset}";

        return query;
    }

    public async Task<int> GetPageCountAsync(string? query = null)
    {
        if (string.IsNullOrEmpty(query))
        {
            query = "SELECT count(*) FROM safe_hashes";
        }

        await _connection.OpenAsync();

        //query DB
        SqliteCommand cmd = new SqliteCommand(query, _connection);
        var reader = await cmd.ExecuteReaderAsync();

        int hashCount = 1;
        
        while (await reader.ReadAsync())
        {
            hashCount = reader.GetInt32(0);
        }        

        //close connections
        await reader.CloseAsync();
        await _connection.CloseAsync();

        //calculate page numbers
        return (int)Math.Ceiling((double)hashCount / _pageSize);
    }
}