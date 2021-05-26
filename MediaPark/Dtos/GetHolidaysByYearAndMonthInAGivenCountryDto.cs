using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
   public interface GetHolidaysByYearAndMonthInAGivenCountryDto
    {
        public SendDateDto Date { get; set; }
        public HolidayNameDto Name { get; set; }
        public HolidayType HolidayType { get; set; }
    }
}
