using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시물 List 요청 모델
    /// </summary>
    public class BoardListRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// 시작 날짜
        /// </summary>
        public string SDate { get; set; } = string.Empty;
        /// <summary>
        /// 종료 날짜
        /// </summary>
        public string EDate { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 게시물 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;
        // 추후 사용 예정
        //public string title { get; set; } = string.Empty;
        //public string baseName { get; set; } = string.Empty;

    }
}
