using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Board;
using F1Soft.Starmap.Service.Controllers.Groupware.Emp.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Emp;

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
public class EmpController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly IEmpService _empService;

    /// <summary>
    /// 캘린더 컨트롤러 생성자
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="empService"></param>
    public EmpController(ILogger<ApprovalController> logger, IEnvService envService, IEmpService empService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _empService = empService;

    }

    /// <summary>
    /// 조직도 (사원 리스트) 조회, totalSearch - 빈칸: 전체조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "totalSearch": "손규형",
    ///       "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetEmpList")]
    public async Task<IActionResult> GetEmpList([FromBody] EmpListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _empService.GetEmpList(request);

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
