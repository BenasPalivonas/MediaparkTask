using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Holiday
    {
        public int Id { get; set; }
        public HolidayDate HolidayDate {get;set;}
        public HolidayName HolidayName { get; set; }
        public HolidayType HolidayType { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
    }
}
