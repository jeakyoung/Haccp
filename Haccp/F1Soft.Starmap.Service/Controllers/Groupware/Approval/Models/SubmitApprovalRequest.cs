using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 파라미터 요청 모델
    /// </summary>
    public class SubmitApprovalRequest
    {
        /// <summary>
        /// 다중승인 플래그
        /// </summary>
        [Required]
        public string? MuldecFlag { get; set; } = string.Empty;

        /// <summary>
        /// 최종 승인자 플래그
        /// </summary>
        public string? LastCnfrmerFlag { get; set; } = string.Empty;

        /// <summary>
        /// 최종 소유자 플래그
        /// </summary>
        public string? LastOwnerFlag { get; set; } = string.Empty;

        /// <summary>
        /// 전자문서 실행 ID
        /// </summary>
        [Required]
        public string? EaExeId { get; set; } = string.Empty;

        /// <summary>
        /// 구분 코드
        /// </summary>
        [Required]
        public int? GbnCode { get; set; }

        /// <summary>
        /// 실행 순번
        /// </summary>
        [Required]
        public int? ExeSeq { get; set; }

        /// <summary>
        /// 주문 순번
        /// </summary>
        [Required]
        public int? OrderSeq { get; set; }

        /// <summary>
        /// 옵션 이름
        /// </summary>
        public string? OptionName { get; set; } = string.Empty;

        /// <summary>
        /// 승인 플래그
        /// </summary>
        public string? AppFlag { get; set; } = string.Empty;

        /// <summary>
        /// 업무 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 직원 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EmployeeNo { get; set; } = string.Empty;
    }
}
