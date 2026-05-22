//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Oracle.ManagedDataAccess.Client;
//using System.Data;
//using System.Text;

//namespace F1Soft.Starmap.Service.Controllers.Groupware.Sample;

///// <summary>
///// 기본 컨트롤러
///// </summary>
//[ApiController]
//[Route("api/[controller]")]
//public class TPMController : Controller
//{
//    private readonly ILogger<TPMController> _logger;
//    private readonly string _connectionString;

//    /// <summary>
//    /// SQLServerQueryController 생성자
//    /// </summary>
//    /// <param name="configuration"></param>
//    /// <param name="logger"></param>
//    public TPMController(IConfiguration configuration, ILogger<TPMController> logger)
//    {
//        _connectionString = configuration.GetConnectionString("MsSqlProdConnection")!;
//        _logger = logger;
//    }


//    //[HttpGet]
//    //[Route("CallProcedure")]
//    //public async Task<IActionResult> CallProcedure([FromBody] List<DbProcedureRequest> dbProcs)
//    //{
//    //    try
//    //    {
//    //        _logger.LogInformation("CallStoredProcedure 실행 시작"); // 정보 로그 작성





//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        _logger.LogError(ex, "오류 발생");
//    //        return StatusCode(500, ex.Message);
//    //    }
//    //}




//}