using MediaPark.Database;
using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.FetchData
{
    public interface IGetData
    {
        public Task<List<GetSupportedCountriesDto>> FetchSupportedCountries();
        public List<Country> GetCountryEntities(List<GetSupportedCountriesDto> countries);
        public Task<IEnumerable<HolidayType>> GetHolidayTypes(List<GetSupportedCountriesDto> countries);
        public Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonth(HolidaysForGivenCountryBodyDto getHolidays);
        public string ConfigureGetHolidaysForMonthUrl(HolidaysForGivenCountryBodyDto getHolidays);
        public Task<List<SendHolidaysInGivenCountryDto>> GetHolidaysForMonthInDatabase(HolidaysForGivenCountryBodyDto getHolidays);
        public Task AddHolidaysToDb(List<SendHolidaysInGivenCountryDto> getHolidays);
        public Task<IsPublicHolidayDto> GetIsPublicHoliday(SpecificDayStatusDto getDayStatus);
        public Task<IsWorkDayDto> GetIsWorkDay(SpecificDayStatusDto getDayStatus);
    }
}
