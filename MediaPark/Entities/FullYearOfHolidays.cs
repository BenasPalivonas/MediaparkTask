using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class FullYearOfHolidays
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Range(1,int.MaxValue)]
        public int Year { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
        public Country Country { get; set; }
    }
}
