namespace F1Soft.Starmap.Service.Controllers.Setting.Models
{
    /// <summary>
    /// 게시물 List 요청 모델
    /// </summary>
    public class SettingListRequest
    {
        /// <summary>
        /// 회사 ticker
        /// </summary>
        public string ticker { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string employeeNo { get; set; } = string.Empty;

        

    }
}
