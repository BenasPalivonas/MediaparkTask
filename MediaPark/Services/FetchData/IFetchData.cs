using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.FetchData
{
    public interface IFetchData
    {
        public Task<List<GetSupportedCountriesDto>> FetchSupportedCountries();
        public List<Country> GetCountryEntities(List<GetSupportedCountriesDto> countries);

        public Task<IEnumerable<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries);
    }
}
