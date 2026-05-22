namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시판 Isuue Title 요청 모델
    /// </summary>
    public class BoardIssueRequest
    {
        ///// <summary>
        ///// 게시판 이름
        ///// </summary>
        //public string title { get; set; } = string.Empty;

        ///// <summary>
        ///// 게시판 구분
        ///// </summary>
        //public string statusGbn { get; set; } = string.Empty;

        ///// <summary>
        ///// 필요 없음
        ///// </summary>
        //public string baseName { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;
    }
}
