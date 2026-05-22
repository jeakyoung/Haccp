using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface IBoardService
    {
        /// <summary>
        /// 캘린더 전체 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardIssueList(BoardIssueRequest request);

        /// <summary>
        /// 이슈(타이틀)별 게시글 리스트 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardList(BoardListRequest request);

        /// <summary>
        /// 게시물 본문 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardDetail(BoardDetailRequest request);

        /// <summary>
        /// 게시물 댓글 작성
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> PostAnswer(BoardlAnswerRequest request);


    }
}
