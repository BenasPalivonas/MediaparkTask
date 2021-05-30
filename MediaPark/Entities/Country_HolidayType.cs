using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Country_HolidayType
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public int HolidayTypeId { get; set; }
        public HolidayType HolidayType { get; set; }

    }
}
