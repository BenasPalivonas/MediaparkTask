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


namespace MediaPark.Database
{
    public class GetData
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";
        public static async Task FetchDataFromApi(AppDbContext db)
        {
            if (db.Countries.Any())
            {
                ClearDatabase(db);
            }
            ApiHelper.InitializeClient();
            await AddSupportedCountries(db);
            await db.SaveChangesAsync();
        }
        public static void ClearDatabase(AppDbContext db)
        {
            db.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'");
            var tableNames = db.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();
            foreach (var table in tableNames)
            {
                db.Database.ExecuteSqlRaw($"DELETE FROM {table}");
            }
            db.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? CHECK CONSTRAINT all'");
        }
        public static async Task AddSupportedCountries(AppDbContext db)
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(_supportedCountriesUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var countries = await response.Content.ReadAsAsync<List<getSupportedCountriesDto>>();
                    var countriesForDb = countries.Select(c => new Country
                    {
                        CountryCode = c.CountryCode,
                        FullName = c.FullName,
                        FromDate = c.FromDate,
                        ToDate = c.ToDate,
                        Regions = c.Regions.Select(r => new Region
                        {
                            Name = r,
                        }).ToList(),
                        HolidayTypes = c.HolidayTypes.Select(h=>new HolidayType { 
                        Name=h,
                        }).ToList(),
                    }).ToList();
                    await db.Countries.AddRangeAsync(countriesForDb);
                    await db.SaveChangesAsync();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
