using MediaPark.Database;
using MediaPark.Services.ApiHelper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MediaPark.Services.GetData;
using MediaPark.Dtos;
using MediaPark.Entities;
using Xunit;
using MediaPark.Dtos.GetSpecificDayStatus;

namespace MediaPark.tests
{
    public class DataHandlerTests
    {
        private readonly AppDbContext _dbContext;
        private readonly IApiHelper _apiHelper;
        public DataHandlerTests()
        {
            var apiHelperMoq = new Mock<IApiHelper>();
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            var context = new AppDbContext(options);
            _apiHelper = apiHelperMoq.Object;
            _dbContext = context;
        }
        [Fact]
        public async void GetHolidayTypes()
        {
            var data = new List<GetSupportedCountriesDto>()
            {
                new GetSupportedCountriesDto{
                CountryCode="ltu",
                FromDate=new FromDate{
                CountryCode="ltu",
                Day=1,
                Month=1,
                Year=2022
                },
                FullName="Lithuania",
                Regions=new string[]{
                "vienas","du"
                },
                ToDate=new ToDate{ CountryCode="ltu", Day=2,Month=2,Year=2022},
                HolidayTypes=new string[]{
                    "public_holidays",
                    "test_holidays"
                }
                },

            };
            var DataHandler = new HandleData(_apiHelper, _dbContext);
            var HolidayTypes = await DataHandler.GetHolidayTypes(data);

            Assert.Equal("public_holidays", HolidayTypes[0].Name);
            Assert.Equal("test_holidays", HolidayTypes[1].Name);
        }
        [Fact]
        public async void CreateDayEntity()
        {
            var DataHandler = new HandleData(_apiHelper, _dbContext);
            var SpecificDayStatusDto = new SpecificDayStatusDto() { 
            DayOfTheMonth=1,
            Month=1,
            Year=2020
            };
            var Day = await DataHandler.CreateDayEntity(SpecificDayStatusDto, "Test Day");
            Assert.Equal("Test Day", Day.DayStatus);
            Assert.Equal(1, Day.Month);
            Assert.Equal(2020, Day.Year);
        }

    }
}
