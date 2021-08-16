using HMS.Domain.Models;
using HMS.Domain.UnitOfWork;
using HMS.Web.Controllers;
using HMS.Web.Services;
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
    public class ReservationControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public HttpClient Client { get; }

        public ReservationControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            Client = factory.CreateClient();
        }

        [Fact]
        public async Task Reservation_Controller_Tests()
        {
            // Arrange
            var mock = new Mock<IUnitOfWork>();
            mock.Setup(uow => uow.Hotels.GetAllAsync().Result).Returns(GetTestHotels());
            mock.Setup(uow => uow.Reservations.GetAllAsync().Result).Returns(GetTestReservations());
            mock.Setup(uow => uow.Rooms.GetAllAsync().Result).Returns(GetTestRooms());
            mock.Setup(uow => uow.RoomTypes.GetAllAsync().Result).Returns(GetTestRoomTypes());
            //mock.Setup(uow => uow.Users.GetAllAsync().Result).Returns(GetTestUsers());


            var controller = new ReservationController(mock.Object, new ReservationService(mock.Object));
            var response = await Client.GetAsync("/");
            response.EnsureSuccessStatusCode();

            // Act
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result1 = controller.ChooseRoom(1, DateTime.Today.ToString() + " - " + DateTime.Today.AddDays(3).ToString(), 1, 1);
            var result2 = controller.ChooseRoom(1, DateTime.Today.AddDays(3).ToString() + " - " + DateTime.Today.AddDays(6).ToString(), 2, 1);

            // Assert
            //Assert.Contains("January", stringResponse);
            var viewResult1 = Assert.IsType<ViewResult>(result1);
            var model1 = Assert.IsAssignableFrom<ICollection<RoomType>>(viewResult1.Model);
            var viewResult2 = Assert.IsType<ViewResult>(result2);
            var model2 = Assert.IsAssignableFrom<ICollection<RoomType>>(viewResult2.Model);
            Assert.NotEmpty(model1);
            Assert.Empty(model2);

        }

        private List<Hotel> GetTestHotels()
        {
            var hotels = new List<Hotel>
            {
                new Hotel {Id=1, Name="Sae", Address="S", Country="USA", Town="NY", Description="test", FloorCount=5, Site="sa@fds", PhoneNumber="21321", Email="ssa@fd.com", Fax="34123"}
            };
            return hotels;
        }
        private List<Reservation> GetTestReservations()
        {
            var reservations = new List<Reservation>
            {
                new Reservation {Id=1, Rooms=GetTestRooms().Where(r => r.Id == 1).ToList(), CheckIn = DateTime.Today.AddDays(2), CheckOut = DateTime.Today.AddDays(4)},
                new Reservation {Id=2, Rooms=GetTestRooms().Where(r => r.Id == 2).ToList(), CheckIn = DateTime.Today.AddDays(5), CheckOut = DateTime.Today.AddDays(7)}
            };
            return reservations;
        }
        private List<Room> GetTestRooms()
        {
            var rooms = new List<Room>
            {
                new Room {Id=1, Number=101, RoomTypeId=1},
                new Room {Id=2, Number=202, RoomTypeId=1}
            };
            return rooms;
        }
        private List<RoomType> GetTestRoomTypes()
        {
            var roomTypes = new List<RoomType>
            {
                new RoomType {Id=1, HotelId=1, MaxGuest=2, Name="Lala"}
            };
            return roomTypes;
        }
        //private List<User> GetTestUsers()
        //{
        //    var users = new List<User>
        //    {
        //        new User {Id="1", Email="sd@google.com", UserName="sddsfds"}
        //    };
        //    return users;
        //}

    }
}
