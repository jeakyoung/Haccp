//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Oracle.ManagedDataAccess.Client;
//using System.Data;
//using System.Text;

//namespace F1Soft.Starmap.Service.Controllers.DatabaseQuery.SQLServer
//{
//    /// <summary>
//    /// MS-SQL Stored Procedure 호출
//    /// </summary>
//    [ApiController]
//    [Route("api/[controller]")]
//    public class OracleQueryController : Controller
//    {
//        private readonly ILogger<OracleQueryController> _logger;
//        private readonly string _connectionString;

//        /// <summary>
//        /// SQLServerQueryController 생성자
//        /// </summary>
//        /// <param name="configuration"></param>
//        /// <param name="logger"></param>
//        public OracleQueryController(IConfiguration configuration, ILogger<OracleQueryController> logger)
//        {
//            _connectionString = configuration.GetConnectionString("OracleProdConnection")!;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Stored Procedure 호출
//        /// </summary>
//        /// <param name="dbProcs"></param>
//        /// <returns></returns>
//        [HttpPost]
//        [Route("CallProcedure")]
//        public async Task<IActionResult> CallProcedure([FromBody] List<DbProcedureRequest> dbProcs)
//        {
//            try
//            {
//                _logger.LogInformation("CallStoredProcedure 실행 시작"); // 정보 로그 작성

//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    using (var transaction = connection.BeginTransaction())
//                    {
//                        DbResult result = new DbResult();
//                        result.Result = false;
//                        result.ResultList = new List<DbResultItem>();
//                        StringBuilder errorMessages = new StringBuilder();

//                        foreach (var procInfo in dbProcs)
//                        {
//                            var procedureName = procInfo.ProcedureName;
//                            var parameters = procInfo.Parameters;
//                            DbResultItem sqlDataItem = new DbResultItem();
//                            sqlDataItem.DataTable = new DataTable();
//                            sqlDataItem.Name = procedureName;

//                            var command = new OracleCommand(procedureName, connection)
//                            {
//                                CommandType = CommandType.StoredProcedure
//                            };

//                            if (procInfo.Parameters != null)
//                            {
//                                //숫자, 날짜 파라미터 테스트 필요
//                                foreach (var param in procInfo.Parameters)
//                                {
//                                    command.Parameters.Add(param.Key, param.Value.ToString());
//                                }
//                            }

//                            if (procInfo.IsChangeProcedure)
//                            {
//                                try
//                                {
//                                    sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
//                                }
//                                catch (OracleException ex)
//                                {
//                                    errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
//                                    transaction.Rollback();
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    using (var reader = await command.ExecuteReaderAsync())
//                                    {
//                                        DataTable dt = new DataTable();

//                                        for (int i = 0; i < reader.FieldCount; i++)
//                                        {
//                                            int suffix = 1;
//                                            string newColumnName = reader.GetName(i);

//                                            while (dt.Columns.Contains(newColumnName))
//                                            {
//                                                newColumnName = $"{reader.GetName(i)}_{suffix}";
//                                                suffix++;
//                                            }

//                                            dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
//                                        }

//                                        while (await reader.ReadAsync())
//                                        {
//                                            object[] values = new object[reader.FieldCount];
//                                            reader.GetValues(values);
//                                            dt.Rows.Add(values);
//                                        }
//                                        sqlDataItem.DataTable = dt;
//                                    }
//                                }
//                                catch (OracleException ex)
//                                {
//                                    errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
//                                    transaction.Rollback();
//                                }
//                            }
//                            result.ResultList.Add(sqlDataItem);
//                        }

//                        if (errorMessages.Length > 0)
//                        {
//                            return StatusCode(500, errorMessages.ToString());
//                        }

//                        result.Result = true;
//                        transaction.Commit();
//                        string jsonContent = JsonConvert.SerializeObject(result);

//                        _logger.LogDebug("CallStoredProcedure 완료");
//                        return Ok(jsonContent);
//                    }

//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "오류 발생");
//                return StatusCode(500, ex.Message);
//            }
//        }


//        /// <summary>
//        /// 데이터베이스 연결 테스트 API
//        /// </summary>
//        /// <returns>데이터베이스 연결 가능 여부</returns>
//        [HttpGet("TestDbConnection")]
//        public async Task<IActionResult> TestDbConnection()
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();  // 데이터베이스 연결 시도
//                    return Ok(new { message = "데이터베이스 연결 성공" });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "데이터베이스 연결 실패");
//                return StatusCode(500, new { message = "데이터베이스 연결 실패", error = ex.Message });
//            }
//        }


//        /// <summary>
//        /// 데이터베이스 시간 가져오기 API
//        /// </summary>
//        /// <returns>데이터베이스 서버의 현재 시간</returns>
//        [HttpGet("GetDbTime")]
//        public async Task<IActionResult> GetDbTime()
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var command = new OracleCommand("SELECT GETDATE(); ", connection);  // SQL Server의 GETDATE() 함수 사용
//                    var dbTime = await command.ExecuteScalarAsync();
//                    return Ok(new { dbTime });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "데이터베이스 시간 가져오기 실패");
//                return StatusCode(500, new { message = "데이터베이스 시간 가져오기 실패", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// 데이터베이스 이름 가져오기 API
//        /// </summary>
//        /// <returns>연결된 데이터베이스의 이름</returns>
//        [HttpGet("GetDbName")]
//        public async Task<IActionResult> GetDbName()
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var dbName = connection.Database;  // OracleConnection 객체의 Database 속성 사용
//                    return Ok(new { dbName });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "데이터베이스 이름 가져오기 실패");
//                return StatusCode(500, new { message = "데이터베이스 이름 가져오기 실패", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// 데이터베이스 버전 정보 확인 API
//        /// </summary>
//        /// <returns>데이터베이스 서버의 버전 정보</returns>
//        [HttpGet("GetDbVersion")]
//        public async Task<IActionResult> GetDbVersion()
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var command = new OracleCommand("SELECT BANNER FROM V$VERSION", connection);
//                    var dbVersion = await command.ExecuteScalarAsync();
//                    return Ok(new { dbVersion });
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "데이터베이스 버전 정보 가져오기 실패");
//                return StatusCode(500, new { message = "데이터베이스 버전 정보 가져오기 실패", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// 특정 테이블 스키마 정보 확인 API
//        /// </summary>
//        /// <param name="schemaName">스키마 이름 dbo</param>
//        /// <param name="tableName">테이블 이름</param>
//        /// <returns>테이블 스키마 정보</returns>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     {
//        ///         "schemaName": "SM",
//        ///         "tableName": "TIN305D"
//        ///     }
//        ///
//        /// </remarks>
//        [HttpGet("GetTableSchema")]
//        public async Task<IActionResult> GetTableSchema(string schemaName, string tableName)
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();

//                    // INFORMATION_SCHEMA.COLUMNS 뷰를 사용하여 스키마 정보 조회
//                    var query = $@"
//                        SELECT 
//                            COLUMN_NAME,
//                            DATA_TYPE,
//                            DATA_LENGTH,
//                            NULLABLE
//                        FROM ALL_TAB_COLUMNS
//                        WHERE OWNER = @SchemaName AND TABLE_NAME = @TableName;
//                    ";


//                    var command = new OracleCommand(query, connection);
//                    command.Parameters.Add("@SchemaName", schemaName);
//                    command.Parameters.Add("@TableName", tableName);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        var dt = new DataTable();
//                        dt.Load(reader);
//                        string jsonContent = JsonConvert.SerializeObject(dt);
//                        return Ok(new { jsonContent });
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "테이블 스키마 정보 가져오기 실패");
//                return StatusCode(500, new { message = "테이블 스키마 정보 가져오기 실패", error = ex.Message });
//            }
//        }


//        /// <summary>
//        /// 데이터베이스 상태 정보 API
//        /// </summary>
//        /// <returns>데이터베이스의 상태 정보</returns>
//        [HttpGet("GetDbStatus")]
//        public async Task<IActionResult> GetDbStatus()
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();

//                    // v$sysstat 뷰를 사용하여 성능 지표 수집
//                    var query = @"
//                        SELECT 
//                            NAME,
//                            VALUE
//                        FROM V$SYSSTAT
//                        WHERE NAME IN (
//                            'user commits',
//                            'user rollbacks',
//                            'logons current',
//                            'opened cursors current'
//                        );
//                    ";

//                    var command = new OracleCommand(query, connection);
//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        var dt = new DataTable();
//                        dt.Load(reader);



//                        return Ok(JsonConvert.SerializeObject(dt));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "데이터베이스 상태 정보 가져오기 실패");
//                return StatusCode(500, new { message = "데이터베이스 상태 정보 가져오기 실패", error = ex.Message });
//            }
//        }

//        /// <summary>
//        /// CPU 사용량 상위 20개 쿼리 정보 API
//        /// </summary>
//        /// <returns>CPU 사용량 상위 20개 쿼리 정보</returns>
//        /// <remarks>
//        /// Sample request:
//        ///
//        ///     {
//        ///         "topCount": 10
//        ///     }
//        ///
//        /// </remarks>
//        [HttpGet("GetTopCpuQueries")]
//        public async Task<IActionResult> GetTopCpuQueries(int topCount = 10)
//        {
//            try
//            {
//                using (var connection = new OracleConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var query = $@"
//                    SELECT * 
//                    FROM (
//                        SELECT 
//                            SQL_TEXT,
//                            CPU_TIME,
//                            ELAPSED_TIME,
//                            EXECUTIONS
//                        FROM V$SQLAREA
//                        ORDER BY CPU_TIME DESC
//                    )
//                    WHERE ROWNUM <= :TopCount";

//                    var command = new OracleCommand(query, connection);
//                    command.Parameters.Add(":TopCount", topCount);

//                    using (var reader = await command.ExecuteReaderAsync())
//                    {
//                        var dt = new DataTable();
//                        dt.Load(reader);
//                        return Ok(JsonConvert.SerializeObject(dt));
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "CPU 사용량 상위 쿼리 정보 가져오기 실패");
//                return StatusCode(500, new { message = "CPU 사용량 상위 쿼리 정보 가져오기 실패", error = ex.Message });
//            }
//        }


//    }
//}
