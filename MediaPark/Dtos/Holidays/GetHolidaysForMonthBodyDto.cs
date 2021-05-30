using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos.Holidays
{
    public class GetHolidaysForMonthBodyDto
    {
        [MaxLength(3)]
        public string CountryCode { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
    }
}
