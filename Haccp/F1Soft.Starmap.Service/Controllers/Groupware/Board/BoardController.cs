using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board;

/// <summary>
/// 캘린더 컨트롤러
/// </summary>
/// 
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class BoardController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly IBoardService _boardService;

    /// <summary>
    /// 캘린더 컨트롤러 생성자
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="boardService"></param>
    public BoardController(ILogger<ApprovalController> logger, IEnvService envService, IBoardService boardService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _boardService = boardService;

    }

    /// <summary>
    /// 게시판 이슈(타이틀) 리스트 조회 ex) 공지사항, 행사 등
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetIssuesBoardList")]
    public async Task<IActionResult> GetBoardIssueList([FromBody] BoardIssueRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardIssueList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 이슈별 게시물 리스트 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "sDate": "20240907",
    ///       "eDate": "20250207",
    ///       "employeeNo": "19039",
    ///       "commentNo": ""
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetOneIssueBoardList")]
    public async Task<IActionResult> GetBoardList([FromBody] BoardListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 게시물 본문 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "commentNo": "202501150001",
    ///       "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetBoardDetail")]
    public async Task<IActionResult> GetBoardDetail([FromBody] BoardDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardDetail(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 게시물 댓글 작성 / iud (등록: "I", 삭제: "", 수정 없음) / 댓글 등록 성공 시 본문 내용 반환
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "commentNo": "202501150001",
    ///       "replyComment": "댓글내용",
    ///       "employeeNo": "19039",
    ///       "iud": "I"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("PostAnswer")]
    public async Task<IActionResult> PostAnswer([FromBody] BoardlAnswerRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.PostAnswer(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

}
