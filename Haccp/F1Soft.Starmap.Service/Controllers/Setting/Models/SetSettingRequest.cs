namespace F1Soft.Starmap.Service.Controllers.Setting.Models
{
    /// <summary>
    /// 설정정보 저장 모델
    /// </summary>
    public class SetSettingRequest
    {
        /// <summary>
        /// 회사 ticker
        /// </summary>
        public string ticker { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string employeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 결재 알림
        /// </summary>
        public string approvalChkFlag { get; set; } = string.Empty;

        /// <summary>
        /// 게시 현황 알림
        /// </summary>
        public string boardChkFlag { get; set; } = string.Empty;

        /// <summary>
        /// 공지 알림
        /// </summary>
        public string notiChkFlag { get; set; } = string.Empty;

        /// <summary>
        /// 일정 알림
        /// </summary>
        public string scheduleChkFlag { get; set; } = string.Empty;

    }
}
