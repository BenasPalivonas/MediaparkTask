using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services.FetchData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Repositories
{
    public class CountryPublicHolidaysRepository : ICountryPublicHolidaysRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFetchData _fetchData;

        public CountryPublicHolidaysRepository(AppDbContext appDbContext,IFetchData fetchData)
        {
            _appDbContext = appDbContext;
            _fetchData = fetchData;
        }
        public async Task<List<SendSupportedCountriesDto>> GetAllCountries()
        {
            var countries = await Task.Run(() => _appDbContext.Countries.Select(c => new SendSupportedCountriesDto
            {
                CountryCode = c.CountryCode,
                Regions = c.Regions.Select(r=>r.Name).ToList(),
                HolidayTypes = c.Country_HolidayTypes.Select(c=>c.HolidayType.Name).ToList(),
                FullName = c.FullName,
                FromDate = new SendDateDto
                {
                    Day = c.FromDate.Day,
                    Month = c.FromDate.Month,
                    Year = c.FromDate.Year,
                },
                ToDate = new SendDateDto
                {
                    Day = c.ToDate.Day,
                    Month = c.ToDate.Month,
                    Year = c.ToDate.Year,
                }
            }).ToList());
            return countries;
        }

        public async Task<List<ReceiveHolidaysByYearAndMonthInAGivenCountryDto>> GetHolidaysForMonthForGivenCountry(GetHolidaysForMonthForGivenCountryDto getHolidays)
        {
            return await _fetchData.GetHolidaysForMonth(getHolidays);
        }
    }
}
