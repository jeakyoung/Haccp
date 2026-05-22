using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval;

/// <summary>
/// 기본 컨트롤러
/// [Authorize]: 모든 기능은 인증된 유저(로그인 성공)만 사용 가능하게 제한 (단, DEBUG 모드 예외)
/// </summary>
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class ApprovalController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly IApprovalService _approvalService;

    /// <summary>
    /// SQLServerQueryController 생성자
    /// </summary>
    /// <param name="envService"></param>
    /// <param name="logger"></param>
    /// <param name="approvalService"></param>
    public ApprovalController(ILogger<ApprovalController> logger, IEnvService envService, IApprovalService approvalService)
    {
        _approvalService = approvalService;
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
    }

    /// <summary>
    /// 본문내용.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///      "eaExeId": "202410140039",
    ///      "eabusNo": "001",
    ///      "employeeNo": "19039",
    ///      "gbnCode": 1,
    ///      "exeSeq": 7
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("GetApprovalDetailList")]
    public async Task<IActionResult> GetApprovalDetailList([FromBody] ApprovalDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }


            var result = await _approvalService.GetApprovalDetailList(request);

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
    /// 미결함, 보관함, 기안함, 결재함
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///       "userID": "19039",
    ///       "todate": "20250204",
    ///       "fromdate": "20250104"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("GetApprovalAllList")]
    public async Task<IActionResult> GetApprovalAllList([FromBody] ApprovalRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.GetApprovalAllList(request);

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
    /// 문서 열람 시간 등록
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Param은 GetApprovalDetailList와 동일
    /// </remarks>
    [HttpPost("CheckOpenTime")]
    public async Task<IActionResult> CheckOpenTime([FromBody] ApprovalDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.CheckOpenTime(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "실패", Error = ex.Message });
        }
    }


    /// <summary>
    /// 승인, 확인, 반송, 합의
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///       "muldecFlag": "1",
    ///       "lastCnfrmerFlag": "1",
    ///       "lastOwnerFlag": "0",
    ///       "eaExeId": "202501310010",
    ///       "gbnCode": 1,
    ///       "exeSeq": 2,
    ///       "orderSeq": 2,
    ///       "optionName": "승인",
    ///       "appFlag": "1",
    ///       "eabusNo": "001",
    ///       "employeeNo": "19039"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("ConfirmApproval")]
    public async Task<IActionResult> ConfirmApproval([FromBody] SubmitApprovalRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.ConfirmApproval(request);

            _logger.LogDebug("결재 성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "결재 실패");
            return StatusCode(500, new { Message = "결재 실패", Error = ex.Message });
        }
    }

    /// <summary>
    /// 댓글 작성
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "eabusNo": "001",                     
    ///         "eaExeId": "202501220005",            
    ///         "gbnCode": 1,                         
    ///         "exeSeq": 1,                          
    ///         "answerCnt": "댓글 테스트",           
    ///         "opmanCode": "0000",                  
    ///         "mainViewFlag": "0"                  
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("PostAnswer")]
    public async Task<IActionResult> PostAnswer([FromBody] ApprovalAnswerRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.EaExeId))
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다. (EaExeId 필수)" });
            }

            var result = await _approvalService.PostAnswer(request);

            _logger.LogDebug("댓글작성 완료");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "댓글작성 실패");
            return StatusCode(500, new { Message = "실패", Error = ex.Message });
        }
    }

}