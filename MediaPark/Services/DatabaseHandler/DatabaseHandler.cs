using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
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
        private readonly IGetData _getData;
        private readonly AppDbContext _appDbContext;

        public DatabaseHandler(IGetData getData, AppDbContext appDbContext)
        {
            _getData = getData;
            _appDbContext = appDbContext;
        }

        public async Task AddPublicHolidays(IEnumerable<HolidayType> holidayTypes)
        {
            await _appDbContext.HolidayTypes.AddRangeAsync(holidayTypes);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task ClearAndUpdateDatabaseWithFetchedData()
        {
            if (_appDbContext.Countries.Any())
            {
                await ClearDatabase();
            }
            var countries = await _getData.FetchSupportedCountries();
            await AddPublicHolidays(await _getData.GetHolidayTypes(countries));
            await _appDbContext.AddRangeAsync(_getData.GetCountryEntities(countries));
            await _appDbContext.SaveChangesAsync();
        }
        public async Task ClearDatabase()
        {
            await _appDbContext.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? NOCHECK CONSTRAINT all'");
            var tableNames = _appDbContext.Model.GetEntityTypes()
            .Select(t => t.GetTableName())
            .Distinct()
            .ToList();
            foreach (var table in tableNames)
            {
                await _appDbContext.Database.ExecuteSqlRawAsync($"DELETE FROM {table}");
            }
            await _appDbContext.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable @command1 = 'ALTER TABLE ? CHECK CONSTRAINT all'");
        }
        public async Task AddHolidaysToDatabase(IEnumerable<Holiday> holidays) {
            await _appDbContext.Holidays.AddRangeAsync(holidays);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task AddDayToDatabase(Day day)
        {
            await _appDbContext.Days.AddAsync(day);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<DayStatusAnswerDto> ReturnDayStatusFromDb(SpecificDayStatusDto specificDay) {
            string dayStatus = await Task.Run(() =>
            {
               return _appDbContext.Days.Where(d=>d.Year==specificDay.Year
               &&d.Month==specificDay.Month
               &&d.DayOfTheMonth==specificDay.DayOfTheMonth).SingleOrDefault()?.DayStatus;
                
            });
            if (dayStatus is not null)
            {
                return new DayStatusAnswerDto { DayStatus = dayStatus };
            }
            return null;
        }
    }
}
