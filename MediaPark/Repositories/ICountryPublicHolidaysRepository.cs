using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.Holidays;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Repositories
{
    public interface ICountryPublicHolidaysRepository
    {
        public Task<List<SendSupportedCountriesDto>> GetAllCountries();
        public Task<List<SendHolidayDto>> GetHolidaysForMonthForGivenCountry(GetHolidaysForMonthBodyDto getHolidaysForMonth);
        public Task<DayStatusAnswerDto> GetSpecificDayStatus(SpecificDayStatusDto getSpecificDayStatus);
        public Task<List<Holiday>> GetHolidaysForYear(GetHolidaysForYearBodyDto getHolidaysForYear);
        public Task<SendMaximumNumberOfFreeDaysDto> GetMaximumNumberOfFreeDaysInHolidayList(List<Holiday> holidays);
    }
}
