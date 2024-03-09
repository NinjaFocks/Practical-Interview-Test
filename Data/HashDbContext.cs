using Microsoft.EntityFrameworkCore;
using System;

namespace Rdessoy_MCMS_Practical_Interview_Test.Data;

/// <summary>
/// HashDbContext is no longer being used, but I've left this code in to illustrate the ORM
/// </summary>
public class HashDbContext : DbContext
{
    public HashDbContext(DbContextOptions<HashDbContext> options) : base(options)
    {
        Database.EnsureCreated();        
    }
    
    public DbSet<HashModel> Hashes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.LogTo(Console.WriteLine);
}