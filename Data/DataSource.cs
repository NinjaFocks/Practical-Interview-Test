using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

public class DataSource : IDataSource
{
    private static SqliteConnection _connection;
    private int _pageSize = 10;

    public DataSource()
    {
        _connection = new SqliteConnection();
    }

    public async Task<IList<HashModel>> GetHashModelsAsync(int? pageSize = null, int pageNumber = 1)
    {
        pageSize = pageSize ?? _pageSize;

        //calculate offset
        var offset = (pageNumber * pageSize) - pageSize;

        await _connection.OpenAsync();

        //get hashes
        var query = $"SELECT * FROM safe_hashes ORDER BY hash_id ASC LIMIT {pageSize} OFFSET {offset}";

        SqliteCommand cmd = new SqliteCommand(query, _connection);
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
        return hashes;
    }

    public async Task<int> GetPageCountAsync()
    {
        await _connection.OpenAsync();

        //query DB
        var query = "SELECT count(*) FROM safe_hashes";

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