using MediaPark.Entities;
using MediaPark.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaPark.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CountryPublicHolidaysController : ControllerBase
    {
        private readonly ICountryPublicHolidaysRepository _countryPublicHolidaysRepository;

        public CountryPublicHolidaysController(ICountryPublicHolidaysRepository countryPublicHolidaysRepository)
        {
            _countryPublicHolidaysRepository = countryPublicHolidaysRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Country>>> GetAllCountries() {
            try
            {
                var countries = await _countryPublicHolidaysRepository.GetAllCountries();
                return Ok(countries);
            }
            catch (Exception ex) {
                return StatusCode(500);
            }
        }
    }
}
