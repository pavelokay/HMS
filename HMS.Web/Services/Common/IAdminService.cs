using HMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Web.Services.Common
{
    public interface IAdminService
    {
        Task<List<Room>> GetRoomStatisticsAsync(int? modeId, int? hotelId, string roleName, string outDay, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate);
        Task<List<User>> GetUserStatisticsAsync(int? modeId, int? hotelId, string userId, string roleName, int? minReservationRoomCount, string dateRange, string fullPeriod, string dateRange2, int? minFloorNumber, int? maxFloorNumber, int? minGuest, int? maxGuest, int? roomStatusId, decimal? minRate, decimal? maxRate, string dateRangeReport, string dateRangeReport2, string reportFlag, int? topPlaceCount);
        Task<List<Transaction>> GetReservationStatisticsAsync(int? modeId, string userLogin, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport);
        Task<List<RoomType>> GetRoomTypeStatisticsAsync(int? modeId);
        Task<List<Transaction>> GetFinanceStatisticsAsync(string roleName, string dateRange, string fullPeriod, string reportFlag, string dateRangeReport);
    }
}
