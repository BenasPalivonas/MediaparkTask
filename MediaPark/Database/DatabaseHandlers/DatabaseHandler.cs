using MediaPark.Database.DatabaseHandlers;
using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace MediaPark.Database.DatabaseHandler
{
    public class DatabaseHandler
    {
        public static async Task ClearAndUpdateDatabaseWithFetchedData(AppDbContext db)
        {
            if (db.Countries.Any())
            {
               await ClearDatabase(db);
            }
            ApiHelper.InitializeClient();
            var countries = await InitialDataHandler.FetchSupportedCountries();
            await db.AddRangeAsync(countries);
            await db.SaveChangesAsync();
        }
        public static async Task ClearDatabase(AppDbContext db)
        {
          await  db.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'");
            var tableNames =  db.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();
            foreach (var table in tableNames)
            {
              await  db.Database.ExecuteSqlRawAsync($"DELETE FROM {table}");
            }
           await db.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? CHECK CONSTRAINT all'");
        }
    }
}
