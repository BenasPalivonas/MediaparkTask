using MediaPark.Dtos;
using MediaPark.Entities;
using MediaPark.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MediaPark.Database.DatabaseHandlers
{
    public class InitialDataHandler
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";

        public static async Task<List<Country>> FetchSupportedCountries()
        {
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(_supportedCountriesUrl))
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
