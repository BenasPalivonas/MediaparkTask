using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Holiday
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Date { get; set; }
        [Range(1, 7)]
        public int DayOfTheWeek { get; set; }
        public List<HolidayName> HolidayName { get; set; }
        public int HolidayTypeId { get; set; }
        public HolidayType HolidayType { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public int DayId { get; set; }
        public Day Day { get;set; }
    }
}
