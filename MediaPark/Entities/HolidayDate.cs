using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Entities
{
    public class HolidayDate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int DayOfWeek { get; set; }
        public int HolidayId { get; set; }
        public Holiday Holidays { get; set; }
    }
}
