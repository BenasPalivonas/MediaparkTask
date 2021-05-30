using MediaPark.Dtos.Holidays;
using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
    public class ReadHolidaysInGivenCountry
    {
        public HolidayDateDto Date { get; set; }
        public List<HolidayNameDto> Name { get; set; }
        public string HolidayType { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
    }
}
