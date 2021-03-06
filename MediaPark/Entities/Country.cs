using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Country
    {
        [Key]
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public List<Region> Regions { get; set; }
        public List<Country_HolidayType> Country_HolidayTypes { get; set; }
        [Required]
        public string FullName { get; set; }
        public FromDate FromDate { get; set; }
        public ToDate ToDate { get; set; }
        public List<Holiday> Holiday { get; set; }
        public List<FullYearOfHolidays> FullYearOfHolidays { get; set; }
    }
}
