﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Dtos.MaximumNumberOfFreeDays
{
    public class SendMaximumNumberOfFreeDaysInYearDto
    {
        public int MaximumNumberOfFreeDaysInARow { get; set; }
        public string DateInterval { get; set; }
    }
}
