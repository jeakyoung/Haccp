//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using F1Soft.Starmap.Service.Helper;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
//using Microsoft.VisualBasic;
//using Newtonsoft.Json;
//using System.Data;
//using System.Text;

//namespace F1Soft.Starmap.Service.Controllers.Database.SQLServer;

///// <summary>
///// MS-SQL Stored Procedure 호출
///// </summary>
//[ApiController]
//[Route("api/[controller]")]
//public class SQLServerQueryController : Controller
//{
//    private readonly ILogger<SQLServerQueryController> _logger;
//    private readonly string _connectionString;

//    /// <summary>
//    /// SQLServerQueryController 생성자
//    /// </summary>
//    /// <param name="configuration"></param>
//    /// <param name="logger"></param>
//    public SQLServerQueryController(IConfiguration configuration, ILogger<SQLServerQueryController> logger)
//    {
//        _connectionString = configuration.GetConnectionString("MsSqlProdConnection")!;
//        _logger = logger;
//    }

//    /// <summary>
//    /// Stored Procedure 호출
//    /// </summary>
//    /// <param name="dbProcs"></param>
//    /// <returns></returns>
//    /// <remarks>
//    /// 
//    /// Sample request:
//    /// 
//    /// [
//    ///  {
//    ///    "procedureName": "SP_WEB_LOGIN",
//    ///    "isChangeProcedure": false,
//    ///    "parameters": {
//    ///      "USER_ID": "0000",
//    ///      "USER_PW": "5183"
//    ///    }
//    ///  }
//    ///]
//    /// 
//    /// </remarks>
//    [Authorize]
//    [HttpPost]
//    [Route("CallProcedure")]
//    public async Task<IActionResult> CallProcedure([FromBody] List<DbProcedureRequest> dbProcs)
//    {
//        try
//        {
//            _logger.LogInformation("CallStoredProcedure 실행 시작"); // 정보 로그 작성
            
//            // DatabaseHelper를 사용하여 데이터베이스 작업 수행
//            using (var helper = new SQLServerHelper(_connectionString, _logger))
//            {
//                var result = await helper.CallProcedureAsync(dbProcs);

//                if (result.Result)
//                {
//                    string jsonContent = JsonConvert.SerializeObject(result);
//                    _logger.LogDebug("CallStoredProcedure 완료");
//                    return Ok(JsonHelper.ConvertDbResultToJsonObject(result));
//                }
//                else
//                {
//                    return StatusCode(500, result.ErrorMessage);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "오류 발생");
//            return StatusCode(500, ex.Message);
//        }
//    }


//    /// <summary>
//    /// 데이터베이스 연결 테스트 API
//    /// </summary>
//    /// <returns>데이터베이스 연결 가능 여부</returns>
//    [Authorize]
//    [HttpGet("TestDbConnection")]
//    public async Task<IActionResult> TestDbConnection()
//    {
//        try
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();  // 데이터베이스 연결 시도
//                return Ok(new { message = "데이터베이스 연결 성공" });
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "데이터베이스 연결 실패");
//            return StatusCode(500, new { message = "데이터베이스 연결 실패", error = ex.Message });
//        }
//    }


//    /// <summary>
//    /// 데이터베이스 시간 가져오기 API
//    /// </summary>
//    /// <returns>데이터베이스 서버의 현재 시간</returns>
//    [Authorize]
//    [HttpGet("GetDbTime")]
//    public async Task<IActionResult> GetDbTime()
//    {
//        string sql = @"SELECT GETDATE() DbDate;";

//        return await ExecuteSingleSelectQuery("GetDbTime", new SqlCommand() { CommandText = sql });
//    }

//    /// <summary>
//    /// 데이터베이스 이름 가져오기 API
//    /// </summary>
//    /// <returns>연결된 데이터베이스의 이름</returns>
//    [Authorize]
//    [HttpGet("GetDbName")]
//    public async Task<IActionResult> GetDbName()
//    {
//        try
//        {
//            using (var connection = new SqlConnection(_connectionString))
//            {
//                await connection.OpenAsync();
//                var dbName = connection.Database;  // SqlConnection 객체의 Database 속성 사용
//                return Ok(new { dbName });
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "데이터베이스 이름 가져오기 실패");
//            return StatusCode(500, new { message = "데이터베이스 이름 가져오기 실패", error = ex.Message });
//        }
//    }

//    /// <summary>
//    /// 데이터베이스 버전 정보 확인 API
//    /// </summary>
//    /// <returns>데이터베이스 서버의 버전 정보</returns>
//    [Authorize]
//    [HttpGet("GetDbVersion")]
//    public async Task<IActionResult> GetDbVersion()
//    {
//        string sql = @"SELECT SERVERPROPERTY('productversion');";

//        return await ExecuteSingleSelectQuery("GetDbVersion", new SqlCommand() { CommandText = sql });
//    }

//    /// <summary>
//    /// 특정 테이블 스키마 정보 확인 API
//    /// </summary>
//    /// <param name="schemaName">스키마 이름 dbo</param>
//    /// <param name="tableName">테이블 이름</param>
//    /// <returns>테이블 스키마 정보</returns>
//    /// <remarks>
//    /// Sample request:
//    ///
//    ///     {
//    ///         "schemaName": "dbo",
//    ///         "tableName": "TIN305D"
//    ///     }
//    ///
//    /// </remarks>
//    [Authorize]
//    [HttpGet("GetTableSchema")]
//    public async Task<IActionResult> GetTableSchema(string schemaName, string tableName)
//    {
//        string sql = @"SELECT 
//                            COLUMN_NAME,
//                            DATA_TYPE,
//                            CHARACTER_MAXIMUM_LENGTH,
//                            IS_NULLABLE
//                        FROM INFORMATION_SCHEMA.COLUMNS
//                        WHERE TABLE_SCHEMA = @SchemaName AND TABLE_NAME = @TableName;";
//        var command = new SqlCommand(sql);
//        command.Parameters.AddWithValue("@SchemaName", schemaName);
//        command.Parameters.AddWithValue("@TableName", tableName);

//        return await ExecuteSingleSelectQuery("GetTableSchema", command);
//    }


//    /// <summary>
//    /// 데이터베이스 상태 정보 API
//    /// </summary>
//    /// <returns>데이터베이스의 상태 정보</returns>
//    [Authorize]
//    [HttpGet("GetDbStatus")]
//    public async Task<IActionResult> GetDbStatus()
//    {
//        string sql = @"SELECT object_name, counter_name, cntr_value, cntr_type
//                             FROM sys.dm_os_performance_counters
//                             WHERE counter_name IN (
//                                 'User Connections',
//                                 'Page life expectancy',
//                                 'Batch Requests/sec',
//                                 'Page splits/sec'
//                             );";

//        return await ExecuteSingleSelectQuery("GetDbStatus", new SqlCommand() { CommandText = sql });
//    }

//    /// <summary>
//    /// CPU 사용량 상위 20개 쿼리 정보 API
//    /// </summary>
//    /// <returns>CPU 사용량 상위 20개 쿼리 정보</returns>
//    [Authorize]
//    [HttpGet("GetTopCpuQueries")]
//    public async Task<IActionResult> GetTopCpuQueries()
//    {
//        string sql = @"SELECT TOP 20
//                            total_worker_time / qs.execution_count AS 'Average CPU used',
//                            total_worker_time AS 'Total CPU used',
//                            last_worker_time AS 'Last CPU used',
//                            max_worker_time AS 'MAX CPU used',
//                            qs.execution_count AS 'Execution count',
//                            SUBSTRING(qt.text, qs.statement_start_offset / 2,
//                                (CASE WHEN qs.statement_end_offset = -1 
//                                    THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2 
//                                    ELSE qs.statement_end_offset END - qs.statement_start_offset) / 2) AS 'Individual Query',
//                            qt.text AS 'Parent Query',
//                            DB_NAME(qt.dbid) AS DatabaseName,
//                            qs.creation_time,
//                            qs.last_execution_time
//                        FROM sys.dm_exec_query_stats qs
//                        CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) AS qt
//                        ORDER BY 'Average CPU used' DESC; ";

//        return await ExecuteSingleSelectQuery("GetTopCpuQueries", new SqlCommand() { CommandText = sql });
//    }


//    /// <summary>
//    /// 테이블 목록 조회
//    /// </summary>
//    /// <returns></returns>
//    [Authorize]
//    [HttpGet("GetTableList")]
//    public async Task<IActionResult> GetTableList()
//    {
//        string sql = @"SELECT TABLE_NAME 
//                    FROM INFORMATION_SCHEMA.TABLES 
//                    WHERE TABLE_TYPE = 'BASE TABLE'";

//        return await ExecuteSingleSelectQuery("GetTableList", new SqlCommand() { CommandText = sql });
//    }


//    private async Task<IActionResult> ExecuteSingleSelectQuery(string apiName, SqlCommand query)
//    {
//        try
//        {
//            using (var helper = new SQLServerHelper(_connectionString, _logger))
//            {

//                var result = await helper.ExecuteSqlCommandAsync(new List<DbSqlRequest>() { new DbSqlRequest() { SqlCommand = query, IsChangeProcedure = false } });

//                if (result.Result)
//                {
//                    string jsonContent = JsonConvert.SerializeObject(result);
//                    _logger.LogDebug($"{apiName} 완료");
//                    return Ok(jsonContent);
//                }
//                else
//                {
//                    return StatusCode(500, result.ErrorMessage);
//                }
//            }
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, $"{apiName} 실패");
//            return StatusCode(500, new { message = $"{apiName} 실패", error = ex.Message });
//        }
//    }

//}
