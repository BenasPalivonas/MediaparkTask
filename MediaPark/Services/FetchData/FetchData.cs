using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services.ApiHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaPark.Services.FetchData
{
    public class FetchData : IFetchData
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";
        private readonly IApiHelper _apiHelper;

        public FetchData(IApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public async Task<List<Country>> FetchSupportedCountries()
        {
            _apiHelper.InitializeClient();
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(_supportedCountriesUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    var countries = await response.Content.ReadAsAsync<List<GetSupportedCountriesDto>>();
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
                        HolidayTypes = c.HolidayTypes.Select(h => new HolidayType
                        {
                            Name = h,
                        }).ToList(),
                    }).ToList();
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
