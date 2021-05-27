using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services.ApiHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaPark.Services.FetchData
{
    public class FetchData : IFetchData
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";
        private readonly IApiHelper _apiHelper;
        private readonly AppDbContext _dbContext;

        public FetchData(IApiHelper apiHelper, AppDbContext dbContext)
        {
            _apiHelper = apiHelper;
            _dbContext = dbContext;
        }
        public async Task<List<GetSupportedCountriesDto>> FetchSupportedCountries()
        {
            _apiHelper.InitializeClient();
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_supportedCountriesUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var countries = await response.Content.ReadAsAsync<List<GetSupportedCountriesDto>>();
                    return countries;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public List<Country> GetCountryEntities(List<GetSupportedCountriesDto> countries)
        {
            return countries.Select(c => new Country
            {
                CountryCode = c.CountryCode,
                FullName = c.FullName,
                FromDate = c.FromDate,
                ToDate = c.ToDate,
                Regions = c.Regions.Select(r => new Region
                {
                    Name = r,
                }).ToList(),
                Country_HolidayTypes = c.HolidayTypes.Select(ht => new Country_HolidayType
                {
                    Country = _dbContext.Countries.Where(cdb => cdb.CountryCode == c.CountryCode).FirstOrDefault(),
                    HolidayType = _dbContext.HolidayTypes.Where(htdb => htdb.Name == ht).FirstOrDefault()
                }).ToList()
            }).ToList();
        }

        public async Task<IEnumerable<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries) {
            string[] holidayTypes = new string[] { };
            foreach (var holidayType in countries.Select(c => c.HolidayTypes))
            {
                holidayTypes = holidayTypes.Union(holidayType).ToArray();
            }
            return await Task.Run(() => holidayTypes.Select(ht => new HolidayType { Name = ht })); 
        }
    }
}
