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
        public string CountryCode { get; set; }
        public List<Region> Regions { get; set; }
        public List<HolidayType> HolidayTypes { get; set; }
        [Required]
        public string FullName { get; set; }
        public FromDate FromDate { get; set; }
        public ToDate ToDate { get; set; }
     
    }
}
