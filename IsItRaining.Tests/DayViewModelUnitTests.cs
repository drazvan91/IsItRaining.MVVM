using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IsItRaining.Components;

namespace IsItRaining.Tests
{
    [TestClass]
    public class DayViewModelUnitTests
    {
        private DayViewModel viewModel;

        [TestInitialize]
        public void TestInit()
        {
            viewModel = new DayViewModel();
        }

        [TestMethod]
        public void WhenImageIsNullThenImageUrlIsEmpty()
        {
            viewModel.Image = null;
            Assert.AreEqual(string.Empty, viewModel.ImageUrl);
        }

        [TestMethod]
        public void WhenImageHasValueThenImageUrlIsValid()
        {
            viewModel.Image = "clounds";
            Assert.AreEqual("http://openweathermap.org/img/w/clounds.png", viewModel.ImageUrl);

            viewModel.Image = "wind";
            Assert.AreEqual("http://openweathermap.org/img/w/wind.png", viewModel.ImageUrl);
        }

        [TestMethod]
        public void WhenDayIsNotSetTheTitleIsNotAvailable()
        {
            Assert.AreEqual("NA", viewModel.Title);
        }

        [TestMethod]
        public void WhenDayIsSetTheTitleIsValid()
        {
            viewModel.Day = new DateTime(2018, 8, 20);
            Assert.AreEqual("Mon, 20", viewModel.Title);

            viewModel.Day = new DateTime(2018, 8, 23);
            Assert.AreEqual("Thu, 23", viewModel.Title);
        }

        [TestMethod]
        public void WhenMaxMinTemperaturesAreNotSetTheirTextsAreNotAvailable()
        {
            viewModel.MaxTemperature = null;
            viewModel.MinTemperature = null;

            Assert.AreEqual("NA", viewModel.MaxTemperatureText);
            Assert.AreEqual("NA", viewModel.MinTemperatureText);
        }

        [TestMethod]
        public void WhenMaxMinTemperaturesAreSetTheirTextsAreAvailable()
        {
            viewModel.MaxTemperature = 23.234;
            viewModel.MinTemperature = 12.2;

            Assert.AreEqual("23.2C", viewModel.MaxTemperatureText);
            Assert.AreEqual("12.2C", viewModel.MinTemperatureText);

            viewModel.MaxTemperature = 23;
            viewModel.MinTemperature = -3.43421;

            Assert.AreEqual("23.0C", viewModel.MaxTemperatureText);
            Assert.AreEqual("-3.4C", viewModel.MinTemperatureText);
        }

        [TestMethod]
        public void WhenMaxMinTemperaturesAreSetTheirTextsAreAvailableAndRounded()
        {
            viewModel.MaxTemperature = 23.2784;
            viewModel.MinTemperature = 12.2;

            Assert.AreEqual("23.3C", viewModel.MaxTemperatureText);
            Assert.AreEqual("12.2C", viewModel.MinTemperatureText);

            viewModel.MaxTemperature = 23;
            viewModel.MinTemperature = -3.47421;

            Assert.AreEqual("23.0C", viewModel.MaxTemperatureText);
            Assert.AreEqual("-3.5C", viewModel.MinTemperatureText);
        }
    }
}
