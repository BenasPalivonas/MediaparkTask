using MediaPark.Database;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.Holidays;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.DatabaseHandler
{
    public interface IDatabaseHandler
    {
        public Task PopulateDb();
        public Task ClearDatabase();
        public Task AddPublicHolidays(IEnumerable<HolidayType> holidayTypes);
        public Task<List<Holiday>> GetMonthsHolidaysFromDb(GetHolidaysForMonthBodyDto getHolidays);
        public Task AddHolidaysToDatabase(IEnumerable<Holiday> holidays);
        public Task AddDayToDatabase(Day day);
        public Task<DayStatusAnswerDto> ReturnDayStatusFromDb(SpecificDayStatusDto specificDay);
        public Task AddFullYearsOfHolidaysToCountry(GetHolidaysForYearBodyDto getHolidays);
        public Task<List<Holiday>> GetYearsHolidaysFromDb(GetHolidaysForYearBodyDto getHoliday);
    }
}
