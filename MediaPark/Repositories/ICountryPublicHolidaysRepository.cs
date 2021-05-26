using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Repositories
{
   public interface ICountryPublicHolidaysRepository
    {
        Task<IEnumerable<Country>> GetAllCountries();
    }
}
