using MediaPark.Database;
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

        public CountryPublicHolidaysRepository(AppDbContext appDbContext) {
            _appDbContext = appDbContext;
        }
        public async Task<IEnumerable<Country>> GetAllCountries()
        {
            var countries = await Task.Run(() => _appDbContext.Countries
            .Include(r => r.Regions)
            .Include(r => r.HolidayTypes)
            .Include(r=>r.FromDate)
            .AsEnumerable()) ;
            return countries;
        }
    }
}
