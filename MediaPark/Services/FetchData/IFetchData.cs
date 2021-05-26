﻿using MediaPark.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Services.FetchData
{
    public interface IFetchData
    {
        public Task<List<Country>> FetchSupportedCountries();
    }
}