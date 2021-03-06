using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
    public class SendSupportedCountriesDto
    {
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public List<string> Regions { get; set; }
        public List<string> HolidayTypes { get; set; }
        public string FullName { get; set; }
        public DateDto FromDate { get; set; }
        public DateDto ToDate { get; set; }
    }
}
