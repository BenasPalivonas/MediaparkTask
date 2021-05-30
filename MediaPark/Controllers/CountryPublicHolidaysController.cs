using MediaPark.Entities;
using MediaPark.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaPark.Dtos;
using MediaPark.Dtos.GetSpecificDayStatus;
using MediaPark.Dtos.MaximumNumberOfFreeDays;
using MediaPark.Dtos.Holidays;

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
        public async Task<ActionResult<List<Country>>> GetAllCountries()
        {
            try
            {
                var countries = await _countryPublicHolidaysRepository.GetAllCountries();
                if (!countries.Any())
                {
                    return NotFound(404);
                }
                return Ok(countries);
            }
            catch
            {
                return StatusCode(500);
            }
        }
        [HttpPost]
        public async Task<ActionResult<List<ReadHolidaysInGivenCountry>>> GetAllHolidaysForMonth(HolidaysForGivenCountryMonthBodyDto getHolidays)
        {
            try
            {
                var response = await _countryPublicHolidaysRepository.GetHolidaysForMonthForGivenCountry(getHolidays);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<DayStatusAnswerDto>> SpecificDayStatus(SpecificDayStatusDto dayStatusDto) {
            try
            {
                var response = await _countryPublicHolidaysRepository.GetSpecificDayStatus(dayStatusDto);
                return Ok(response);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }            
        }
        [HttpPost]
        public async  Task<ActionResult<List<SendHolidayDto>>> GetMaximumNumberOfFreeDaysInYear(GetHolidaysForYearBodyDto getHolidaysForYear) {
            try {
                var holidays = await _countryPublicHolidaysRepository.GetHolidaysForYear(getHolidaysForYear);
                return holidays;
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}
