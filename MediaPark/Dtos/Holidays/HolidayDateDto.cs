using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos
{
    public class HolidayDateDto
    {
        [Range(1, 7)]
        public int Day { get; set; }
        [Range(1, 31)]
        public int Month { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
        [Range(1, 7)]
        public int DayOfWeek { get; set; }
    }
}
