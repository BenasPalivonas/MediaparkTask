using MediaPark.Dtos.GetMonthsHolidays;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
   public class HolidaysByYearAndMonthInAGivenCountryDto
    {
        public GetDateDto Date { get; set; }
        public List<HolidayNameDto> Name { get; set; }
        public string HolidayType { get; set; }
        public string CountryCode { get; set; }
    }
}
