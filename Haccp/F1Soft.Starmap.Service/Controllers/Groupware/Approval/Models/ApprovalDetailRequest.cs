using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 본문 요청 모델
    /// </summary>
    public class ApprovalDetailRequest
    {
        /// <summary>
        /// 전자문서 실행 ID (모든 요청에서 사용)
        /// </summary>
        [Required]
        public string? EaExeId { get; set; } = string.Empty;

        /// <summary>
        /// 업무 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 직원 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 구분 코드 (결재의견에서 사용)
        /// </summary>
        public int? GbnCode { get; set; }

        /// <summary>
        /// 결재순번
        /// </summary>
        public int? ExeSeq { get; set; }


    }
}
