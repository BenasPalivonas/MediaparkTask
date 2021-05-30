using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        public string Date { get; set; }
        [Range(1,7)]
        public int DayOfTheWeek { get; set; }
        public List<HolidayName> HolidayName { get; set; }
        public int HolidayTypeId { get; set; }
        public HolidayType HolidayType { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
    }
}
