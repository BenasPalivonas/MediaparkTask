using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class Day
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Range(1, 31)]
        public int DayOfTheMonth { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
        public string DayStatus { get; set; }
        public Holiday Holiday { get; set; }
    }
}
