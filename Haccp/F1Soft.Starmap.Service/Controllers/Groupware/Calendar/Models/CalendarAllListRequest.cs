namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models
{
    /// <summary>
    /// 캘린더 전체 리스트 요청
    /// </summary>
    public class CalendarAllListRequest
    {
        /// <summary>
        /// 시작 날짜
        /// </summary>
        public string SDate { get; set; } = string.Empty;

        /// <summary>
        /// 종료 날짜
        /// </summary>
        public string EDate { get; set; } = string.Empty;

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        ///// <summary>
        ///// 조회자 ID
        ///// </summary>
        //public string ViewEmployeeNo { get; set; } = string.Empty;
    }
}
