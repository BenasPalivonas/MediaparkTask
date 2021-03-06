using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos.MaximumNumberOfFreeDays
{
    public class GetHolidaysForYearBodyDto
    {
        [MaxLength(3)]
        public string CountryCode { get; set; }
        [Range(0, int.MaxValue)]
        public int Year { get; set; }
    }
}
