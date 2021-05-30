using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.Holidays;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.GetData
{
    public interface IHandleData
    {
        public Task<List<GetSupportedCountriesDto>> FetchSupportedCountries();
        public List<Country> GetCountryEntities(List<GetSupportedCountriesDto> countries);
        public Task<IEnumerable<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries);
        public Task<List<SendHolidayDto>> FetchHolidaysForMonth(GetHolidaysForMonthBodyDto getHolidays);
        public Task<IsPublicHolidayDto> FetchIsPublicHoliday(SpecificDayStatusDto getDayStatus);
        public Task<IsWorkDayDto> FetchIsWorkDay(SpecificDayStatusDto getDayStatus);
        public Task<Day> CreateDayEntity(SpecificDayStatusDto getSpecificDayStatusDto, string DayStatus);
        public Task<List<SendHolidayDto>> FetchHolidaysForYear(GetHolidaysForYearBodyDto getMaximumNumberOfFreeDaysInYear);

    }
}
