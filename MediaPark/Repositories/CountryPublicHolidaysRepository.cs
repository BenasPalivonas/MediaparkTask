using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Entities;
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

        public CountryPublicHolidaysRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<List<SendSupportedCountriesDto>> GetAllCountries()
        {
            var countries = await Task.Run(() => _appDbContext.Countries.Select(c => new SendSupportedCountriesDto
            {
                CountryCode = c.CountryCode,
                Regions = c.Regions,
                HolidayTypes = c.HolidayTypes,
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
    }
}
