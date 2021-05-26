using MediaPark.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.DatabaseHandler
{
   public interface IDatabaseHandler
    {
        public Task ClearAndUpdateDatabaseWithFetchedData(AppDbContext db);
        public Task ClearDatabase(AppDbContext db);
    }
}
