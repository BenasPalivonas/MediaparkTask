using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services;
using MediaPark.Services.FetchData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace MediaPark.Services.DatabaseHandler
{
    public class DatabaseHandler : IDatabaseHandler
    {
        private readonly IFetchData _fetchData;

        public DatabaseHandler(IFetchData fetchData)
        {
            _fetchData = fetchData;
        }

        public async Task ClearAndUpdateDatabaseWithFetchedData(AppDbContext db)
        {
            if (db.Countries.Any())
            {
               await ClearDatabase(db);
            }
            var countries = await _fetchData.FetchSupportedCountries();
            await db.AddRangeAsync(countries);
            await db.SaveChangesAsync();
        }
        public async Task ClearDatabase(AppDbContext db)
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
