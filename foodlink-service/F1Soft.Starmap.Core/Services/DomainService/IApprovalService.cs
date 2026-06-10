namespace F1Soft.Starmap.Core.Services.DomainService;
{
    /// <summary>
    /// 결재문서 서비스 인터페이스
    /// </summary>
    public interface IApprovalService
    {
        /// <summary>
        /// 미결함, 보관함, 기안함, 결재함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalAllList(ApprovalRequest request);

        /// <summary>
        /// 본문 상세 조회 인터페이스
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalDetailList(ApprovalDetailRequest request);

        /// <summary>
        /// 문서 열람시간 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> CheckOpenTime(ApprovalDetailRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> ConfirmApproval(SubmitApprovalRequest request);

        /// <summary>
        /// 댓글 작성
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> PostAnswer(ApprovalAnswerRequest request);

    }
}
