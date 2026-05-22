using F1Soft.Starmap.Service.Controllers.Groupware.Notice.Models;

namespace F1Soft.Starmap.Service.Services.Groupware.Notice
{
    /// <summary>
    /// 공지사항 인터페이스
    /// </summary>
    public interface INoticeService
    {
        /// <summary>
        /// 공지사항 조회 서비스 인터페이스 호출
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetNoticeList(NoticeRequest request);

    }
}
