using MediaPark.Dtos;
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
    }
}
