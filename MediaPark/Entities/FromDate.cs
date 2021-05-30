using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaPark.Entities
{
    public class FromDate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Range(1, 31)]
        public int Day { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
        public Country Country { get; set; }
        [MaxLength(3)]
        public string CountryCode { get; set; }
    }
}
