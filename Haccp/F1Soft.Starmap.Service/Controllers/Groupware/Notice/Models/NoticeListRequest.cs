using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Notice.Models
{
    /// <summary>
    /// 게시물 List 요청 모델
    /// </summary>
    public class NoticeRequest
    {
        /// <summary>
        /// 종료 날짜 (파라미터상 시작, 종료 꼬여있음 가져올때 다시꼬아서 원래대로 가져오게 수정)
        /// </summary>
        public string EDate { get; set; } = string.Empty;

        /// <summary>
        /// 시작 날짜 (파라미터상 시작, 종료 꼬여있음 가져올때 다시꼬아서 원래대로 가져오게 수정)
        /// </summary>
        public string SDate { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 사원 이름
        /// </summary>
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>
        /// 게시물 제목
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 상태 구분
        /// </summary>
        public string StatusGbn { get; set; } = string.Empty;

    }
}
