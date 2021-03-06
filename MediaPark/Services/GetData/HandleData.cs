using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.Holidays;
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
using MediaPark.Dtos.MaximumNumberOfFreeDays;

namespace MediaPark.Services.GetData
{
    public class HandleData : IHandleData
    {
        private const string _supportedCountriesUrl = "json/v2.0/?action=getSupportedCountries";
        private readonly string _getHolidaysForMonthUrl = "json/v2.0?action=getHolidaysForMonth";
        private readonly string _isPublicHolidayUrl = "json/v2.0?action=isPublicHoliday";
        private readonly string _isWorkDayUrl = "json/v2.0?action=isWorkDay";
        private readonly string _getHolidaysForYearUrl = "json/v2.0?action=getHolidaysForYear";
        private readonly IApiHelper _apiHelper;
        private readonly AppDbContext _dbContext;

        public HandleData(IApiHelper apiHelper, AppDbContext dbContext)
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
                    try {
                    var countries = await response.Content.ReadAsAsync<List<GetSupportedCountriesDto>>();
                    return countries;  
                    }
                    catch
                    {
                        return null;
                    }
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
                CountryCode = c.CountryCode.ToLower(),
                FullName = c.FullName,
                FromDate = c.FromDate,
                ToDate = c.ToDate,
                Regions = c.Regions.Select(r => new Region
                {
                    Name = r,
                }).ToList(),
                Country_HolidayTypes = c.HolidayTypes.Select(ht => new Country_HolidayType
                {
                    Country = _dbContext.Countries.Where(cdb => cdb.CountryCode.ToLower() == c.CountryCode.ToLower()).FirstOrDefault(),
                    HolidayType = _dbContext.HolidayTypes.Where(htdb => htdb.Name == ht).FirstOrDefault()
                }).ToList()
            }).ToList();
        }

        public async Task<List<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries)
        {
            string[] holidayTypes = new string[] { };
            foreach (var holidayType in countries.Select(c => c.HolidayTypes))
            {
                holidayTypes = holidayTypes.Union(holidayType).ToArray();
            }
            return await Task.Run(() => holidayTypes.Select(ht => new HolidayType { Name = ht }).ToList());
        }

        public async Task<List<SendHolidayDto>> FetchHolidaysForMonth(GetHolidaysForMonthBodyDto getHolidays)
        {
            _apiHelper.InitializeClient();
            string url = ConfigureGetHolidaysForMonthUrl(getHolidays);
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var holidays = await response.Content.ReadAsAsync<List<ReadHolidaysInGivenCountry>>();
                        holidays.ForEach(c => c.CountryCode = getHolidays.CountryCode.ToLower());
                        return FormatHolidaysForSending(holidays);
                    }
                    catch {
                        return null;
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private string ConfigureGetHolidaysForMonthUrl(GetHolidaysForMonthBodyDto getHolidays)
        {
            var url = $"{_getHolidaysForMonthUrl}&month={getHolidays.Month}&year={getHolidays.Year}&country={getHolidays.CountryCode}";
            var getCountry = _dbContext.Countries.Include(c => c.Regions).Where(c => c.CountryCode.ToLower().Equals(getHolidays.CountryCode.ToLower()))?.SingleOrDefault();
            if (getCountry is null) {
                return url;
            }
            foreach (var regionName in getCountry.Regions.Select(r => r.Name))
            {
                url += $"&region={regionName}";
            }
            return url;
        }
        public async Task<IsPublicHolidayDto> FetchIsPublicHoliday(SpecificDayStatusDto getDayStatus)
        {
            _apiHelper.InitializeClient();
            var url = $"{_isPublicHolidayUrl}&date={getDayStatus.DayOfTheMonth}-{getDayStatus.Month}-{getDayStatus.Year}&country={getDayStatus.CountryCode}";
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var isPublicHoliday = await response.Content.ReadAsAsync<IsPublicHolidayDto>();
                        return isPublicHoliday;
                    }
                    catch{
                        return null;
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public async Task<IsWorkDayDto> FetchIsWorkDay(SpecificDayStatusDto getDayStatus)
        {
            _apiHelper.InitializeClient();
            var url = $"{_isWorkDayUrl}&date={getDayStatus.DayOfTheMonth}-{getDayStatus.Month}-{getDayStatus.Year}&country={getDayStatus.CountryCode}";
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var isWorkDay = await response.Content.ReadAsAsync<IsWorkDayDto>();
                        return isWorkDay;
                    }
                    catch {
                        return null;
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public async Task<Day> CreateDayEntity(SpecificDayStatusDto getSpecificDayStatusDto, string DayStatus)
        {
            return await Task.Run(() =>
            {
                return new Day
                {
                    DayOfTheMonth = getSpecificDayStatusDto.DayOfTheMonth,
                    Month = getSpecificDayStatusDto.Month,
                    Year = getSpecificDayStatusDto.Year,
                    DayStatus = DayStatus
                };
            });
        }
        public async Task<List<SendHolidayDto>> FetchHolidaysForYear(GetHolidaysForYearBodyDto getMaximumNumberOfFreeDaysInYear)
        {
            _apiHelper.InitializeClient();
            var url = ConfigureGetHolidaysForYearUrl(getMaximumNumberOfFreeDaysInYear);
            using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    try { 
                    var holidays = await response.Content.ReadAsAsync<List<ReadHolidaysInGivenCountry>>();
                    holidays.ForEach(c => c.CountryCode = getMaximumNumberOfFreeDaysInYear.CountryCode.ToLower());
                    return FormatHolidaysForSending(holidays);
                    }
                    catch{
                        return null;
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }

        private static List<SendHolidayDto> FormatHolidaysForSending(List<ReadHolidaysInGivenCountry> holidays)
        {
            return holidays.Select(h => new SendHolidayDto
            {
                Date = $"{h.Date.Day}-{h.Date.Month}-{h.Date.Year}",
                DayOfTheWeek = h.Date.DayOfWeek,
                Name = h.Name.Select(hn => new HolidayNameDto
                {
                    Lang = hn.Lang,
                    Text = hn.Text,
                }).ToList(),
                HolidayType = h.HolidayType,
                CountryCode = h.CountryCode.ToLower()
            }).ToList();
        }

        private string ConfigureGetHolidaysForYearUrl(GetHolidaysForYearBodyDto getMaximumNumberOfFreeDaysInYear)
        {
            string url = $"{_getHolidaysForYearUrl}&year={getMaximumNumberOfFreeDaysInYear.Year}&country={getMaximumNumberOfFreeDaysInYear.CountryCode}";
            var getCountry = _dbContext.Countries.Include(c => c.Regions).Where(c => c.CountryCode.ToLower().Equals(getMaximumNumberOfFreeDaysInYear.CountryCode.ToLower()))?.SingleOrDefault();
            if (getCountry is null) {
                return url;
            }
            foreach (var regionName in getCountry.Regions.Select(r => r.Name))
            {
                url += $"&region={regionName}";
            }
            url += "&holidayType=all";
            return url;
        }

    }
}
