using Microsoft.EntityFrameworkCore;
using MushroomPocket;
using System;
using System.Collections.Generic;

public class MushroomPocketContext : DbContext{
    public DbSet<Character> Characters {get;set;}

     public string DbPath { get; }
     public MushroomPocketContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "mushroom.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}