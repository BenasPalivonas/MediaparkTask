using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetMonthsHolidays;
using MediaPark.Entities;
using MediaPark.Services.ApiHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaPark.Dtos.GetSpecificDayStatus;

namespace MediaPark.Services.FetchData
{
    public class GetData : IGetData
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";
        private readonly string _getHolidaysForMonthUrl = "json/v2.0?action=getHolidaysForMonth";
        private readonly string _IsPublicHolidayUrl = "json/v2.0?action=isPublicHoliday";
        private readonly string _IsWorkDayUrl = "json/v2.0?action=isWorkDay";
        private readonly IApiHelper _apiHelper;
        private readonly AppDbContext _dbContext;

        public GetData(IApiHelper apiHelper, AppDbContext dbContext)
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

        public async Task<IEnumerable<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries)
        {
            string[] holidayTypes = new string[] { };
            foreach (var holidayType in countries.Select(c => c.HolidayTypes))
            {
                holidayTypes = holidayTypes.Union(holidayType).ToArray();
            }
            return await Task.Run(() => holidayTypes.Select(ht => new HolidayType { Name = ht }));
        }

        public async Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonth(HolidaysForGivenCountryBodyDto getHolidays)
        {
            var holidaysFromDb = await GetHolidaysForMonthInDatabase(getHolidays);
            if (holidaysFromDb is not null)
            {
                return holidaysFromDb;
            }
            _apiHelper.InitializeClient();
            string url = ConfigureGetHolidaysForMonthUrl(getHolidays);
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var holidays = await response.Content.ReadAsAsync<List<SendHolidaysInGivenCountryDto>>();
                    holidays.ForEach(c => c.CountryCode = getHolidays.CountryCode);
                    await AddHolidaysToDb(holidays);
                    return holidays;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        public string ConfigureGetHolidaysForMonthUrl(HolidaysForGivenCountryBodyDto getHolidays)
        {
            var url = $"{_getHolidaysForMonthUrl}&month={getHolidays.Month}&year={getHolidays.Year}&country={getHolidays.CountryCode}";
            var getCountry = _dbContext.Countries.Include(c => c.Regions).Where(c => c.CountryCode.Equals(getHolidays.CountryCode)).SingleOrDefault();
            foreach (var regionName in getCountry.Regions.Select(r => r.Name))
            {
                url += $"&region={regionName}";
            }
            return url;
        }
        public async Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonthInDatabase(HolidaysForGivenCountryBodyDto getHolidays)
        {
            var holidays = _dbContext.Holidays.Include(h => h.HolidayDate).Include(h => h.HolidayType).Include(h => h.HolidayName)
                .Where(h => h.CountryCode == getHolidays.CountryCode)
                .Where(h => h.HolidayDate.Year == getHolidays.Year && h.HolidayDate.Month == getHolidays.Month).ToList();
            if (holidays.Any())
            {
                return await Task.Run(() =>
                {
                    return holidays.Select(h => new SendHolidaysInGivenCountryDto
                    {
                        Date = new DateWithDayOfWeekDto
                        {
                            Day = h.HolidayDate.Day,
                            Month = h.HolidayDate.Month,
                            Year = h.HolidayDate.Year,
                            DayOfWeek = h.HolidayDate.DayOfWeek
                        },
                        Name = h.HolidayName.Select(hn => new HolidayNameDto
                        {
                            Lang = hn.Lang,
                            Text = hn.Text,
                        }).ToList(),
                        HolidayType = h.HolidayType.Name,
                        CountryCode = h.CountryCode
                    }).ToList();
                });

            }

            return null;
        }
        public async Task AddHolidaysToDb(List<SendHolidaysInGivenCountryDto> getHolidays)
        {
            IEnumerable<Holiday> holidays = await Task.Run(() =>
            {
                return getHolidays.Select(h => new Holiday
                {
                    HolidayDate = new HolidayDate
                    {
                        Day = h.Date.Day,
                        Month = h.Date.Month,
                        Year = h.Date.Year,
                        DayOfWeek = h.Date.DayOfWeek,
                    },
                    HolidayName = h.Name.Select(hn => new HolidayName
                    {
                        Lang = hn.Lang,
                        Text = hn.Text
                    }).ToList(),
                    HolidayType = _dbContext.HolidayTypes.SingleOrDefault(ht => ht.Name.Equals(h.HolidayType)),
                    Country = _dbContext.Countries.Find(h.CountryCode)
                });
            });
            await _dbContext.Holidays.AddRangeAsync(holidays);
            await _dbContext.SaveChangesAsync();
        }
        public async Task<IsPublicHolidayDto> GetIsPublicHoliday(SpecificDayStatusDto getDayStatus)
        {
            _apiHelper.InitializeClient();
            var url = $"{_IsPublicHolidayUrl}&date={getDayStatus.DayOfTheMonth}-{getDayStatus.Month}-{getDayStatus.Year}&country={getDayStatus.CountryCode}";
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var isPublicHoliday = await response.Content.ReadAsAsync<IsPublicHolidayDto>();
                    Debug.WriteLine(isPublicHoliday.IsPublicHoliday);
                    return isPublicHoliday;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public async Task<IsWorkDayDto> GetIsWorkDay(SpecificDayStatusDto getDayStatus)
        {
            _apiHelper.InitializeClient();
            var url = $"{_IsWorkDayUrl}&date={getDayStatus.DayOfTheMonth}-{getDayStatus.Month}-{getDayStatus.Year}&country={getDayStatus.CountryCode}";
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var isWorkDay = await response.Content.ReadAsAsync<IsWorkDayDto>();
                    Debug.WriteLine(isWorkDay);
                    return isWorkDay;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

    }
}
