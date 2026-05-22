using F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// 캘린더 전체 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetCalendarAllList(CalendarAllListRequest request);
    }
}
