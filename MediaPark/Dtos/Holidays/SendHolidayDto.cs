using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos.Holidays
{
    public class SendHolidayDto
    {
        public string Date { get; set; }
        [Range(1, 7)]
        public int DayOfTheWeek { get; set; }
        public List<HolidayNameDto> Name { get; set; }
        public string HolidayType { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
    }
}
