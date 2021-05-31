using MediaPark.Database;
using MediaPark.Services.DatabaseHandler;
using MediaPark.Services.GetData;
using MediaPark.Dtos.Holidays;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using MediaPark.Entities;
using MediaPark.Repositories;
using System.Diagnostics;
using MediaPark.Dtos.GetSpecificDayStatus;

namespace MediaPark.tests
{
    public class RepositoryTests
    {
        private readonly AppDbContext _dbContext;
        private readonly IHandleData _handleData;
        private readonly IDatabaseHandler _databaseHandler;
        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().Options;
            var context = new AppDbContext(options);
            _dbContext = context;
            var HandleDataMock = new Mock<IHandleData>();
            var DatabaseHandlerMock = new Mock<IDatabaseHandler>();
           var list = Task.Run(() => {
            return new List<Holiday> {
            new Holiday{
                CountryCode="ltu",
                Date="01-01-2020",
                DayOfTheWeek=2,
                HolidayType=new HolidayType{ Name="public_holiday"},
                HolidayName=new List<HolidayName>{ new HolidayName { Lang="Lithuanian", Text="Svente"} }
            }
            };
            });
          
            DatabaseHandlerMock.Setup(h => h.GetMonthsHolidaysFromDb(It.IsAny<GetHolidaysForMonthBodyDto>())).Returns(list);
            var dbAnswerForDayStatus = Task.Run(() =>
            {
                var answer = new DayStatusAnswerDto();
                answer = null;
                return answer;
            });
            DatabaseHandlerMock.Setup(h => h.ReturnDayStatusFromDb(It.IsAny<SpecificDayStatusDto>()))
                .Returns(dbAnswerForDayStatus);
            var dbIsPublicHoliday = Task.Run(() =>
            {
                return new IsPublicHolidayDto { IsPublicHoliday=true };
            });
            HandleDataMock.Setup(h => h.FetchIsPublicHoliday(It.IsAny<SpecificDayStatusDto>()))
                .Returns(dbIsPublicHoliday);
            _handleData = HandleDataMock.Object;
            _databaseHandler = DatabaseHandlerMock.Object;

        }
        [Fact]
        public async void GetHolidaysForMonthForGivenCountryFromDb() {
            var repository = new CountryPublicHolidaysRepository(_dbContext,_handleData,_databaseHandler);

            var response = await repository.GetHolidaysForMonthForGivenCountry(new GetHolidaysForMonthBodyDto());

            Assert.Equal("01-01-2020",response[0].Date);
            Assert.Equal(2, response[0].DayOfTheWeek);
            Assert.Equal("Svente", response[0].Name[0].Text);
            Assert.Equal("public_holiday", response[0].HolidayType);
        }
        [Fact]
        public async void GetMaximumNumberOfFreeDaysInHolidaysList_IfNoHolidays()
        {
            var repository = new CountryPublicHolidaysRepository(_dbContext, _handleData, _databaseHandler);
            var holidays = new List<Holiday>(){ };
            var response = await repository.GetMaximumNumberOfFreeDaysInHolidayList(holidays);
            Debug.WriteLine(response);
            Assert.Equal(2, response.FreeDays);
        }
        [Fact]
        public async void GetSpecificDayStatus_IsPublicHoliday() {
            var repository = new CountryPublicHolidaysRepository(_dbContext, _handleData, _databaseHandler);
            var answer = await repository.GetSpecificDayStatus(new SpecificDayStatusDto());
            Assert.Equal("Public holiday", answer.DayStatus);
        }
    }
}
