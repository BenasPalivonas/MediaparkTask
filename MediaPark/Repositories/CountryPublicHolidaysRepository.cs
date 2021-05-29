using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetMonthsHolidays;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Entities;
using MediaPark.Services.DatabaseHandler;
using MediaPark.Services.GetData;
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

        public async Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonthForGivenCountry(HolidaysForGivenCountryBodyDto getHolidaysForMonth)
        {
            var databaseHolidays = await _databaseHandler.GetHolidaysFromDb(getHolidaysForMonth);
            if (databaseHolidays is null)
            {
                var holidays = await _handleData.FetchHolidaysForMonth(getHolidaysForMonth);
                await _databaseHandler.AddHolidaysToDatabase(await HolidaysDtoToHolidaysIEnumerable(holidays));
                return holidays;
            }
            return await FormatHolidaysForController(databaseHolidays);
        }

        private static async Task<List<SendHolidaysInGivenCountryDto>> FormatHolidaysForController(List<Holiday> databaseHolidays)
        {
            return await Task.Run(() =>
            {
                return databaseHolidays.Select(h => new SendHolidaysInGivenCountryDto
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

        private async Task<IEnumerable<Holiday>> HolidaysDtoToHolidaysIEnumerable(List<SendHolidaysInGivenCountryDto> holidays)
        {
            return await Task.Run(() =>
            {
                return holidays.Select(h => new Holiday
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
                    HolidayType = _appDbContext.HolidayTypes.SingleOrDefault(ht => ht.Name.Equals(h.HolidayType)),
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
        public async Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForYear(GetHolidaysForYear getHolidaysForYear) {
            var holidays = await _handleData.FetchHolidaysForYear(getHolidaysForYear);
            await _databaseHandler.AddHolidaysToDatabase(await HolidaysDtoToHolidaysIEnumerable(holidays));
            return holidays;
        }


    }
}
