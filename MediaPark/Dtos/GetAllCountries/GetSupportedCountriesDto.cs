using MediaPark.Entities;
using System.ComponentModel.DataAnnotations;

namespace MediaPark.Dtos
{
    public class GetSupportedCountriesDto
    {
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public string[] Regions { get; set; }
        public string[] HolidayTypes { get; set; }
        public string FullName { get; set; }
        public FromDate FromDate { get; set; }
        public ToDate ToDate { get; set; }
    }
}
