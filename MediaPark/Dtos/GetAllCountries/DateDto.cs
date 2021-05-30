using System;
using System.ComponentModel.DataAnnotations;

namespace MediaPark.Dtos
{
    public class DateDto
    {
        [Range(1,31)]
        public int Day { get; set; }
        [Range(1,31)]
        public int Month { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
    }
}
