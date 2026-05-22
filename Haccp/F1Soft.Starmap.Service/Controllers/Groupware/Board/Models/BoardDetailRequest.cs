using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시물 본문 요청 모델
    /// </summary>
    public class BoardDetailRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// 게시물 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;

        /// <summary>
        /// 로그인 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty; 

    }
}
