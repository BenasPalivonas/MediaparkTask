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
                CountryCode = c.CountryCode,
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
            await _databaseHandler.AddHolidaysToDatabase(await HolidaysDtoToHolidaysIEnumerable(holidays));
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
                    CountryCode = h.CountryCode
                }).ToList();
            });
        }

        private async Task<IEnumerable<Holiday>> HolidaysDtoToHolidaysIEnumerable(List<SendHolidayDto> holidays)
        {
            return await Task.Run(() =>
            {
                return holidays.Select(h => new Holiday
                {
                    Date = h.Date,
                    HolidayName = h.Name.Select(hn => new HolidayName
                    {
                        Lang = hn.Lang,
                        Text = hn.Text
                    }).ToList(),
                    HolidayType = _appDbContext.HolidayTypes.SingleOrDefault(ht => ht.Name.Equals(h.HolidayType)),
                    CountryCode = h.CountryCode,
                    Country = _appDbContext.Countries.Find(h.CountryCode)
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
            if (isPublicHoliday.IsPublicHoliday == true)
            {
                await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _publicHoliday));
                return new DayStatusAnswerDto { DayStatus = _publicHoliday };
            }
            var isWorkDay = await _handleData.FetchIsWorkDay(getSpecificDayStatus);
            if (isWorkDay.IsWorkDay == true)
            {
                await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _workDay));

                return new DayStatusAnswerDto { DayStatus = _workDay };

            }
            await _databaseHandler.AddDayToDatabase(await _handleData.CreateDayEntity(getSpecificDayStatus, _freeDay));
            return new DayStatusAnswerDto { DayStatus = _freeDay };
        }
        public async Task<List<SendHolidayDto>> GetHolidaysForYear(GetHolidaysForYearBodyDto getHolidaysForYear)
        {
            var dbAnswer = await _databaseHandler.GetYearsHolidaysFromDb(getHolidaysForYear);
            if (dbAnswer is not null)
            {
                return await FormatHolidaysForController(dbAnswer);
            }
            var holidaysForController = await _handleData.FetchHolidaysForYear(getHolidaysForYear);
            var holidays = (await HolidaysDtoToHolidaysIEnumerable(holidaysForController)).ToList();
            var dbHolidays = _appDbContext.Holidays.ToList();
            var distinctHolidaysForDb = GetDistinctHolidays(holidays, dbHolidays);
            await _databaseHandler.AddHolidaysToDatabase(distinctHolidaysForDb);
            await _databaseHandler.AddFullYearsOfHolidaysToCountry(getHolidaysForYear);
            return holidaysForController;
        }

        private static List<Holiday> GetDistinctHolidays(List<Holiday> holidays, List<Holiday> dbHolidays)
        {
            var distinctHolidaysForDb = new List<Holiday>();
            bool contains = false;
            foreach (var holiday in holidays)
            {
                foreach (var dbHoliday in dbHolidays)
                {
                    if (holiday.Date.Equals(dbHoliday.Date) && holiday.CountryCode.Equals(dbHoliday.CountryCode))
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
        public async Task<SendMaximumNumberOfFreeDaysDto> getMaximumNumberOfFreeDaysInHolidayList(List<Holiday> holidays) {
        
        }

    }
}
