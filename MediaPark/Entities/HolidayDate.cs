using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class HolidayDate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Range(1,31)]
        public int Day { get; set; }
        [Range(1, 12)]
        public int Month { get; set; }
        [Range(1,int.MaxValue)]
        public int Year { get; set; }
        [Range(1,7)]
        public int DayOfWeek { get; set; }
        public int HolidayId { get; set; }
        public Holiday Holidays { get; set; }
    }
}
