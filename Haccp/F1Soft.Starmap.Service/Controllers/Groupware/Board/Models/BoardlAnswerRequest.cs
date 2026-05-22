using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 댓글 작성 요청 모델
    /// </summary>
    public class BoardlAnswerRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// Comment 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;

        /// <summary>
        /// Reply 내용
        /// </summary>
        public string ReplyComment { get; set; } = string.Empty;

        /// <summary>
        /// 담당자 코드
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 작업 유형 (IUD)
        /// </summary>
        public string IUD { get; set; } = string.Empty;
    }
}
