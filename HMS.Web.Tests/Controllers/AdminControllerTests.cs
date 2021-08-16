using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HMS.Web.Tests.Controllers
{
    public class AdminControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }

        public AdminControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        //[Fact]
        //public async Task Admin_Controller_Tests()
        //{
        //    // Arrange
        //    var response = await Client.GetAsync("/hotel");
        //    response.EnsureSuccessStatusCode();

        //    // Act
        //    var stringResponse = await response.Content.ReadAsStringAsync();

        //    // Assert
        //    Assert.Contains("Moscow", stringResponse);
        //}
    }
}
