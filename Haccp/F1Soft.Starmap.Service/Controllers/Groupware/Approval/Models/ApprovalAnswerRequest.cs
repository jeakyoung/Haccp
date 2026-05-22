using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 댓글 작성 요청 모델
    /// </summary>
    public class ApprovalAnswerRequest
    {
        /// <summary>
        /// 업무 번호
        /// </summary>
        [Required]
        [StringLength(3, ErrorMessage = "업무 번호는 최대 3자리만 가능합니다.")]
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 전자문서 실행 ID
        /// </summary>
        [Required]
        [StringLength(12, ErrorMessage = "전자문서 실행 ID는 최대 12자리만 가능합니다.")]
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
        /// 답변 순번
        /// </summary>
        [Required]
        public int? AnswerSeq { get; set; } = 0;

        /// <summary>
        /// 답변 구분 이름
        /// </summary>
        [StringLength(20, ErrorMessage = "답변 구분 이름은 최대 20자리만 가능합니다.")]
        public string? AnswerGbnName { get; set; } = string.Empty;

        /// <summary>
        /// 답변 시간
        /// </summary>
        public string? AnswerTime { get; set; } = string.Empty;

        /// <summary>
        /// 답변 내용
        /// </summary>
        [StringLength(4000, ErrorMessage = "답변 내용은 최대 4000자리만 가능합니다.")]
        public string? AnswerCnt { get; set; } = string.Empty;

        /// <summary>
        /// 작업자 코드
        /// </summary>
        [StringLength(10, ErrorMessage = "작업자 코드는 최대 10자리만 가능합니다.")]
        public string? OpmanCode { get; set; } = string.Empty;

        /// <summary>
        /// 작업 시간
        /// </summary>
        public string? Optime { get; set; } = string.Empty;

        /// <summary>
        /// 메인 뷰 플래그
        /// </summary>
        [StringLength(1, ErrorMessage = "메인 뷰 플래그는 최대 1자리만 가능합니다.")]
        public string? MainViewFlag { get; set; } = "0";

        /// <summary>
        /// 작업 구분 (I/U/D)
        /// </summary>
        [StringLength(3, ErrorMessage = "작업 구분은 최대 3자리만 가능합니다.")]
        public string? Iud { get; set; } = string.Empty;
    }
}
