using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
    public class GetHolidaysForMonthForGivenCountryDto
    {
        public string CountryCode { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

    }
}
