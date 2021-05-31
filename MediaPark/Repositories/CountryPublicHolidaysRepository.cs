using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.Holidays;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Entities;
using MediaPark.Services.DatabaseHandler;
using MediaPark.Services.GetData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Repositories
{
    public class CountryPublicHolidaysRepository : ICountryPublicHolidaysRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHandleData _handleData;
        private readonly IDatabaseHandler _databaseHandler;
        private readonly string _publicHoliday = "Public holiday";
        private readonly string _workDay = "Work day";
        private readonly string _freeDay = "Free day";

        public CountryPublicHolidaysRepository(AppDbContext appDbContext, IHandleData handleData, IDatabaseHandler databaseHandler)
        {
            _appDbContext = appDbContext;
            _handleData = handleData;
            _databaseHandler = databaseHandler;
        }
        public async Task<List<SendSupportedCountriesDto>> GetAllCountries()
        {
            List<SendSupportedCountriesDto> countries = await ConvertCountriesForController();
            return countries;
        }

        private async Task<List<SendSupportedCountriesDto>> ConvertCountriesForController()
        {
            return await Task.Run(() => _appDbContext.Countries.Select(c => new SendSupportedCountriesDto
            {
                CountryCode = c.CountryCode.ToLower(),
                Regions = c.Regions.Select(r => r.Name).ToList(),
                HolidayTypes = c.Country_HolidayTypes.Select(c => c.HolidayType.Name).ToList(),
                FullName = c.FullName,
                FromDate = new DateDto
                {
                    Day = c.FromDate.Day,
                    Month = c.FromDate.Month,
                    Year = c.FromDate.Year,
                },
                ToDate = new DateDto
                {
                    Day = c.ToDate.Day,
                    Month = c.ToDate.Month,
                    Year = c.ToDate.Year,
                }
            }).ToList());
        }

        public async Task<List<SendHolidayDto>> GetHolidaysForMonthForGivenCountry(GetHolidaysForMonthBodyDto getHolidaysForMonth)
        {
            var databaseHolidays = await _databaseHandler.GetMonthsHolidaysFromDb(getHolidaysForMonth);
            if (databaseHolidays is not null)
            {
                return await FormatHolidaysForController(databaseHolidays);
            }
            var holidays = await _handleData.FetchHolidaysForMonth(getHolidaysForMonth);
            if (holidays is null)
            {
                return null;
            }
            var holidaysIEnumerable = await HolidaysDtoToHolidaysIEnumerable(holidays);
            await _databaseHandler.AddHolidaysToDatabase(holidaysIEnumerable);
            return holidays;
        }
        private static async Task<List<SendHolidayDto>> FormatHolidaysForController(List<Holiday> databaseHolidays)
        {
            return await Task.Run(() =>
            {
                return databaseHolidays.Select(h => new SendHolidayDto
                {
                    Date = h.Date,
                    DayOfTheWeek = h.DayOfTheWeek,
                    Name = h.HolidayName.Select(hn => new HolidayNameDto
                    {
                        Lang = hn.Lang,
                        Text = hn.Text,
                    }).ToList(),
                    HolidayType = h.HolidayType.Name,
                    CountryCode = h.CountryCode.ToLower()
                }).ToList();
            });
        }

        private async Task<IEnumerable<Holiday>> HolidaysDtoToHolidaysIEnumerable(List<SendHolidayDto> holidays)
        {

            return await Task.Run(() =>
            {
                return holidays.Select(h =>
                 new Holiday
                 {
                     Date = h.Date,
                     DayOfTheWeek = h.DayOfTheWeek,
                     HolidayName = h.Name.Select(hn => new HolidayName
                     {
                         Lang = hn.Lang,
                         Text = hn.Text
                     }).ToList(),
                     HolidayType = _appDbContext.HolidayTypes.SingleOrDefault(ht => ht.Name.Equals(h.HolidayType)),
                     CountryCode = h.CountryCode.ToLower(),
                     Country = _appDbContext.Countries.Find(h.CountryCode.ToLower()),
                     Day = new Day
                     {
                         DayOfTheMonth = Int32.Parse(h.Date.Split("-")[0]),
                         Month = Int32.Parse(h.Date.Split("-")[1]),
                         Year = Int32.Parse(h.Date.Split("-")[2]),
                         DayStatus = "Public holiday",
                     }
                 });
            });
        }

        public async Task<DayStatusAnswerDto> GetSpecificDayStatus(SpecificDayStatusDto getSpecificDayStatus)
        {
            var dbAnswer = await _databaseHandler.ReturnDayStatusFromDb(getSpecificDayStatus);
            if (dbAnswer is not null)
            {
                return dbAnswer;
            }
            var isPublicHoliday = await _handleData.FetchIsPublicHoliday(getSpecificDayStatus);
            if (isPublicHoliday is null) {
                return null;
            }
            if (isPublicHoliday.IsPublicHoliday == true)
            {
                await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _publicHoliday));
                return new DayStatusAnswerDto { DayStatus = _publicHoliday };
            }
            var isWorkDay = await _handleData.FetchIsWorkDay(getSpecificDayStatus);
            if (isWorkDay is null) {
                return null;
            }
            if (isWorkDay.IsWorkDay == true)
            {
                await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _workDay));

                return new DayStatusAnswerDto { DayStatus = _workDay };

            }
            await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _freeDay));
            return new DayStatusAnswerDto { DayStatus = _freeDay };
        }
        public async Task<List<Holiday>> GetHolidaysForYear(GetHolidaysForYearBodyDto getHolidaysForYear)
        {
            var dbAnswer = await _databaseHandler.GetYearsHolidaysFromDb(getHolidaysForYear);
            if (dbAnswer is not null)
            {
                return dbAnswer;
            }
            var holidaysForController = await _handleData.FetchHolidaysForYear(getHolidaysForYear);
            if (holidaysForController is null) {
                return null;
            }
            var holidays = (await HolidaysDtoToHolidaysIEnumerable(holidaysForController)).ToList();
            var dbHolidays = _appDbContext.Holidays.ToList();
            var distinctHolidaysForDb = GetDistinctHolidays(holidays, dbHolidays);
            await _databaseHandler.AddHolidaysToDatabase(distinctHolidaysForDb);
            await _databaseHandler.AddFullYearsOfHolidaysToCountry(getHolidaysForYear);
            return holidays.ToList();
        }

        private static List<Holiday> GetDistinctHolidays(List<Holiday> holidays, List<Holiday> dbHolidays)
        {
            var distinctHolidaysForDb = new List<Holiday>();
            bool contains = false;
            foreach (var holiday in holidays)
            {
                foreach (var dbHoliday in dbHolidays)
                {
                    if (holiday.Date.Equals(dbHoliday.Date) && holiday.CountryCode.ToLower().Equals(dbHoliday.CountryCode.ToLower()))
                    {
                        contains = true;
                        break;
                    }
                }
                if (!contains)
                {
                    distinctHolidaysForDb.Add(holiday);
                }
            }
            return distinctHolidaysForDb;
        }
        public async Task<SendMaximumNumberOfFreeDaysDto> GetMaximumNumberOfFreeDaysInHolidayList(List<Holiday> holidays)
        {
            string[] date;
            int maxFreeDays = 2;
            int maxDays;
            var dayStatus = new DayStatusAnswerDto();
            int day;
            int month;
            int year;
            foreach (var holiday in holidays)
            {
                dayStatus.DayStatus = "Public holiday";
                maxDays = 0;
                date = holiday.Date.Split("-");
                day = Int32.Parse(date[0]);
                month = Int32.Parse(date[1]);
                year = Int32.Parse(date[2]);
                while (dayStatus.DayStatus != "Work day")
                {
                    day++;
                    dayStatus = await GetSpecificDayStatus(new SpecificDayStatusDto()
                    {
                        DayOfTheMonth = day,
                        Month = month,
                        Year = year,
                        CountryCode = holiday.CountryCode.ToLower()
                    });
                    maxDays++;
                }
                if (maxDays >= maxFreeDays)
                {
                    maxFreeDays = maxDays;
                }
            }
            return new SendMaximumNumberOfFreeDaysDto()
            {
                FreeDays = maxFreeDays
            };
        }

    }
}
