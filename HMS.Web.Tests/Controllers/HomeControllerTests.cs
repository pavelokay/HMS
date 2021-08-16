using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HMS.Web.Tests.Controllers
{
    public class HomeControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }

        public HomeControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task Home_Controller_Tests()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup( uow => uow.Hotels.GetAllAsync().Returns(GetTestHotels());
            var controller = new HomeController(mock.Object);
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            // Act
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = controller.FindHotel();

            // Assert
            Assert.Contains("January", stringResponse);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Hotel>>(viewResult.Model);
            Assert.Equal(GetTestHotels().Count, model.Count());
        }

        private List<Hotel> GetTestHotels()
        {
            var hotels = new List<Hotel>
            {
                new Hotel {Id=1, Name="Sae", Address="S", Country="USA", Town="NY", Description="test", FloorCount=5, Site="sa@fds", PhoneNumber="21321", Email="ssa@fd.com", Fax="34123"}
            };
            return hotels;
        }
    }
}
