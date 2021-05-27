using MediaPark.Database;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.DatabaseHandler
{
    public interface IDatabaseHandler
    {
        public Task ClearAndUpdateDatabaseWithFetchedData();
        public Task ClearDatabase();
        public Task AddPublicHolidays(IEnumerable<HolidayType> holidayTypes);
    }
}
