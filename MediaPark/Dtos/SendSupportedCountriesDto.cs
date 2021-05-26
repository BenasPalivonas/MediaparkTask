using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
    public class SendSupportedCountriesDto
    {
        public string CountryCode { get; set; }
        public List<Region> Regions { get; set; }
        public List<HolidayType> HolidayTypes { get; set; }
        public string FullName { get; set; }
        public SendDateDto FromDate { get; set; }
        public SendDateDto ToDate { get; set; }
    }
}
