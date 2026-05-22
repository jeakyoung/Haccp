//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Npgsql;
//using Newtonsoft.Json;
//using System.Data;
//using System.Text;

//namespace F1Soft.Starmap.Service.Controllers.Database.PostgreSQL
//{
//    /// <summary>
//    /// MS-SQL Stored Procedure 호출
//    /// </summary>
//    [ApiController]
//    [Route("api/[controller]")]
//    public class PostgreSQLQueryController : Controller
//    {
//        private readonly ILogger<PostgreSQLQueryController> _logger;
//        private readonly string _connectionString;

//        /// <summary>
//        /// PostgreSQLQueryController 생성자
//        /// </summary>
//        /// <param name="configuration"></param>
//        /// <param name="logger"></param>
//        public PostgreSQLQueryController(IConfiguration configuration, ILogger<PostgreSQLQueryController> logger)
//        {
//            _connectionString = configuration.GetConnectionString("NpgProdConnection")!;
//            _logger = logger;
//        }

//        /// <summary>
//        /// Stored Procedure 호출
//        /// </summary>
//        /// <param name="dbProcs"></param>
//        /// <returns></returns>
//        [Authorize]
//        [HttpPost]
//        [Route("CallProcedure")]
//        public async Task<IActionResult> CallProcedure([FromBody] List<DbProcedureRequest> dbProcs)
//        {
//            try
//            {
//                _logger.LogInformation("CallStoredProcedure 실행 시작"); // 정보 로그 작성

//                using (var connection = new NpgsqlConnection(_connectionString))
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

//                            var command = new NpgsqlCommand(procedureName, connection, transaction)
//                            {
//                                CommandType = CommandType.StoredProcedure
//                            };

//                            if (procInfo.Parameters != null)
//                            {
//                                //숫자, 날짜 파라미터 테스트 필요
//                                foreach (var param in procInfo.Parameters)
//                                {
//                                    command.Parameters.AddWithValue(param.Key, param.Value.ToString()!);
//                                }
//                            }

//                            if (procInfo.IsChangeProcedure)
//                            {
//                                try
//                                {
//                                    sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
//                                }
//                                catch (NpgsqlException ex)
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
//                                catch (NpgsqlException ex)
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
//        [Authorize]
//        [HttpGet("TestDbConnection")]
//        public async Task<IActionResult> TestDbConnection()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
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
//        [Authorize]
//        [HttpGet("GetDbTime")]
//        public async Task<IActionResult> GetDbTime()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var command = new NpgsqlCommand("SELECT GETDATE(); ", connection);  // SQL Server의 GETDATE() 함수 사용
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
//        [Authorize]
//        [HttpGet("GetDbName")]
//        public async Task<IActionResult> GetDbName()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var dbName = connection.Database;  // npgsqlConnection 객체의 Database 속성 사용
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
//        [Authorize]
//        [HttpGet("GetDbVersion")]
//        public async Task<IActionResult> GetDbVersion()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var command = new NpgsqlCommand("SELECT SERVERPROPERTY('productversion'); ", connection);
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
//        ///         "schemaName": "dbo",
//        ///         "tableName": "TIN305D"
//        ///     }
//        ///
//        /// </remarks>
//        [Authorize]
//        [HttpGet("GetTableSchema")]
//        public async Task<IActionResult> GetTableSchema(string schemaName, string tableName)
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();

//                    // INFORMATION_SCHEMA.COLUMNS 뷰를 사용하여 스키마 정보 조회
//                    var query = $@"
//                        SELECT 
//                            COLUMN_NAME,
//                            DATA_TYPE,
//                            CHARACTER_MAXIMUM_LENGTH,
//                            IS_NULLABLE
//                        FROM INFORMATION_SCHEMA.COLUMNS
//                        WHERE TABLE_SCHEMA = @SchemaName AND TABLE_NAME = @TableName;
//                    ";

//                    var command = new NpgsqlCommand(query, connection);
//                    command.Parameters.AddWithValue("@SchemaName", schemaName);
//                    command.Parameters.AddWithValue("@TableName", tableName);

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
//        [Authorize]
//        [HttpGet("GetDbStatus")]
//        public async Task<IActionResult> GetDbStatus()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();

//                    // sys.dm_os_performance_counters DMV를 사용하여 성능 지표 수집
//                    var query = @"
//                             SELECT object_name, counter_name, cntr_value, cntr_type
//                             FROM sys.dm_os_performance_counters
//                             WHERE counter_name IN (
//                                 'User Connections',
//                                 'Page life expectancy',
//                                 'Batch Requests/sec',
//                                 'Page splits/sec',
//                             );
//                    ";

//                    var command = new NpgsqlCommand(query, connection);
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
//        [Authorize]
//        [HttpGet("GetTopCpuQueries")]
//        public async Task<IActionResult> GetTopCpuQueries()
//        {
//            try
//            {
//                using (var connection = new NpgsqlConnection(_connectionString))
//                {
//                    await connection.OpenAsync();
//                    var query = @"
//                        SELECT TOP 20
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
//                        ORDER BY 'Average CPU used' DESC   
//;
//                    ";

//                    var command = new NpgsqlCommand(query, connection);
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
