using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models

{
    /// <summary>
    /// 미결함 열람 요청 모델
    /// </summary>
    public class ApprovalRequest
    {
        /// <summary>
        /// 사용자 ID
        /// </summary>
        [Required]
        [StringLength(10, ErrorMessage = "사용자 ID는 최대 10자리만 가능합니다.")]
        public string? UserId { get; set; } = string.Empty;

        /// <summary>
        /// 시작 날짜
        /// </summary>
        [StringLength(8, ErrorMessage = "시작 날짜는 최대 8자리만 가능합니다.")]
        public string? FromDate { get; set; } = string.Empty;

        /// <summary>
        /// 종료 날짜
        /// </summary>
        [StringLength(8, ErrorMessage = "종료 날짜는 최대 8자리만 가능합니다.")]
        public string? ToDate { get; set; } = string.Empty;

        #region 추후 사용 할 수 있음

        ///// <summary>
        ///// 등록자명
        ///// </summary>
        //[StringLength(10, ErrorMessage = "등록자명은 최대 10자리만 가능합니다.")]
        //public string? RegmanName { get; set; } = string.Empty;

        ///// <summary>
        ///// 제목
        ///// </summary>
        //[StringLength(200, ErrorMessage = "제목은 최대 200자리만 가능합니다.")]
        //public string? EaTitle { get; set; } = string.Empty;

        ///// <summary>
        ///// 실행 ID
        ///// </summary>
        //[StringLength(12, ErrorMessage = "실행 ID는 최대 12자리만 가능합니다.")]
        //public string? EaExeId { get; set; } = string.Empty;


        ///// <summary>
        ///// 업무 번호
        ///// </summary>
        //public string? EabusNo { get; set; } = "";
        #endregion

    }
}
