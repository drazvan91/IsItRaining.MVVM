using Caliburn.Micro;
using IsItRaining.Exceptions;
using IsItRaining.Pages;
using IsItRaining.Services;
using IsItRaining.Services.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace IsItRaining.Tests
{
    [TestClass]
    public class HomePageViewModelUnitTests
    {
        private HomePageViewModel viewModel;

        private Mock<IWeatherService> weatherServiceMock;
        private Mock<IGpsLocatorService> gpsLocatorMock;

        [TestInitialize]
        public void TestInit()
        {
            weatherServiceMock = new Mock<IWeatherService>();
            gpsLocatorMock = new Mock<IGpsLocatorService>();
            viewModel = new HomePageViewModel(gpsLocatorMock.Object, weatherServiceMock.Object);
        }

        [TestMethod]
        public void WhenActivated_ItSetsTheLocationText()
        {
            gpsLocatorMock.Setup(service => service.GetCurrentLocationAsync()).ReturnsAsync(new GpsLocation(1,2));
            gpsLocatorMock.Setup(service => service.GetCivilAddressAsync()).ReturnsAsync("Cluj Napoca");
            

            IActivate activateableVm = viewModel as IActivate;
            Assert.IsTrue(viewModel.LocationText.Contains("please wait"));

            activateableVm.Activate();

            Assert.AreEqual("Cluj Napoca", viewModel.LocationText);
        }

        [TestMethod]
        public void WhenActivated_ItLoadsTheWeather()
        {
            gpsLocatorMock.Setup(service => service.GetCurrentLocationAsync()).ReturnsAsync(new GpsLocation(1, 2));
            gpsLocatorMock.Setup(service => service.GetCivilAddressAsync()).ReturnsAsync("Cluj Napoca");

            var randomWeather = WeatherResponseFactory.GenerateRandom(DateTime.Today, 5);
            weatherServiceMock.Setup(service => service.GetWeatherAsync(DateTime.Today, 5, 1, 2)).ReturnsAsync(randomWeather);

            (viewModel as IActivate).Activate();

            AssertDataIsBasedOnWeatherResponse(randomWeather);
        }

        private void AssertDataIsBasedOnWeatherResponse(WeatherResponse randomWeather)
        {
            for (int i = 0; i < viewModel.Days.Count; i++)
            {
                var dayVM = viewModel.Days[i];
                var dayReponse = randomWeather.Days[i];

                Assert.AreEqual(dayReponse.Date, dayVM.Day);
                Assert.AreEqual(dayReponse.Description, dayVM.Description);
                Assert.AreEqual(dayReponse.MaxTemperature, dayVM.MaxTemperature);
                Assert.AreEqual(dayReponse.MinTemperature, dayVM.MinTemperature);
            }
        }

        [TestMethod]
        public void WhenInvalidGps_AnErrorIsDisplayed()
        {
            gpsLocatorMock.Setup(service => service.GetCurrentLocationAsync()).ThrowsAsync(new GpsNotFoundException());

            (viewModel as IActivate).Activate();

            Assert.AreEqual("Your position cound not be located", viewModel.LocationText);
        }

        [TestMethod]
        public void WhenAddressNotFound_AnErrorIsDisplayed()
        {
            gpsLocatorMock.Setup(service => service.GetCurrentLocationAsync()).ReturnsAsync(new GpsLocation(1, 2));
            gpsLocatorMock.Setup(service => service.GetCivilAddressAsync()).ThrowsAsync(new UnknownAddressException());

            (viewModel as IActivate).Activate();

            Assert.IsTrue(viewModel.LocationText.Contains("Your location cannot be identified but your GPS is"));
        }

        [TestMethod]
        public void OnNextDaysAndPreviousDays_DataIsUpdated()
        {
            gpsLocatorMock.Setup(service => service.GetCurrentLocationAsync()).ReturnsAsync(new GpsLocation(1, 2));
            gpsLocatorMock.Setup(service => service.GetCivilAddressAsync()).ReturnsAsync("Cluj Napoca");

            var todayRandomWeather = WeatherResponseFactory.GenerateRandom(DateTime.Today, 5);
            weatherServiceMock.Setup(service => service.GetWeatherAsync(DateTime.Today, 5, 1, 2)).ReturnsAsync(todayRandomWeather);

            var next5DaysRandomWeather = WeatherResponseFactory.GenerateRandom(DateTime.Today.AddDays(5), 5);
            weatherServiceMock.Setup(service => service.GetWeatherAsync(DateTime.Today.AddDays(5), 5, 1, 2)).ReturnsAsync(next5DaysRandomWeather);

            (viewModel as IActivate).Activate();
            viewModel.NextDays();

            AssertDataIsBasedOnWeatherResponse(next5DaysRandomWeather);

            viewModel.PreviousDays();
            AssertDataIsBasedOnWeatherResponse(todayRandomWeather);
        }
    }
}
