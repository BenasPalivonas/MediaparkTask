using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Entities;
using MediaPark.Services.DatabaseHandler;
using MediaPark.Services.FetchData;
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
        private readonly IGetData _getData;
        private readonly IDatabaseHandler _databaseHandler;
        private readonly string _publicHoliday = "Public holiday";
        private readonly string _workDay = "Work day";
        private readonly string _freeDay = "Free day";

        public CountryPublicHolidaysRepository(AppDbContext appDbContext, IGetData getData, IDatabaseHandler databaseHandler)
        {
            _appDbContext = appDbContext;
            _getData = getData;
            _databaseHandler = databaseHandler;
        }
        public async Task<List<SendSupportedCountriesDto>> GetAllCountries()
        {
            var countries = await Task.Run(() => _appDbContext.Countries.Select(c => new SendSupportedCountriesDto
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
            return countries;
        }

        public async Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonthForGivenCountry(HolidaysForGivenCountryBodyDto getHolidays)
        {
            var data = await _getData.GetHolidaysForMonth(getHolidays);
            return data;
        }
        public async Task<DayStatusAnswerDto> GetSpecificDayStatus(SpecificDayStatusDto getSpecificDayStatus)
        {
            var dbAnswer = await _databaseHandler.ReturnDayStatusFromDb(getSpecificDayStatus);
            if(dbAnswer is not null)
            {
                return dbAnswer;
            }
            var isPublicHoliday = await _getData.GetIsPublicHoliday(getSpecificDayStatus);
            if (isPublicHoliday.IsPublicHoliday == true)
            {
                await _databaseHandler.AddDayToDatabase(await CreateDayEntity(getSpecificDayStatus, _publicHoliday));
                return new DayStatusAnswerDto { DayStatus = _publicHoliday };
            }
            var isWorkDay = await _getData.GetIsWorkDay(getSpecificDayStatus);
            if (isWorkDay.IsWorkDay == true)
            {
                await _databaseHandler.AddDayToDatabase(await CreateDayEntity(getSpecificDayStatus, _workDay));

                return new DayStatusAnswerDto { DayStatus = _workDay };

            }
            await _databaseHandler.AddDayToDatabase(await CreateDayEntity(getSpecificDayStatus, _freeDay));
            return new DayStatusAnswerDto { DayStatus = _freeDay };
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
    }
}
