using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Emp.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.DotNet.MSIdentity.Shared;
using NuGet.Packaging.Signing;
using System.Data;
using static System.Net.WebRequestMethods;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스
    /// </summary>
    public class EmpService : IEmpService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<BoardService> _logger;

        /// <summary>
        /// 캘린더 서비스 생성자
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public EmpService(IEnvService envService, ILogger<BoardService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 사원 정보 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object> GetEmpList(EmpListRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sFactoryCode = "000001" ?? string.Empty;
                    string sPlantCode = string.Empty;
                    string sTotalSearch = request.TotalSearch ?? string.Empty;
                    string sCheck01 = "1" ?? string.Empty;
                    string sCheck02 = "0" ?? string.Empty;
                    string sJoinDateFrom = string.Empty;
                    string sJoinDateTo = string.Empty;
                    string sRetireDateFrom = string.Empty;
                    string sRetireDateTo = string.Empty;
                    string sStandardDate = string.Empty;
                    string sEmployeeNo = request.EmployeeNo;

                    // 프로시저 호출 요청 객체
                    DbProcedureRequest employeeListRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WHR206_01_LIST",
                        Division = "employeeList",
                        Parameters = new Dictionary<string, object>()
                        {
                            { "FACTORY_CODE", sFactoryCode },
                            { "PLANT_CODE", sPlantCode },
                            { "TOTAL_SEARCH", sTotalSearch },
                            { "CHECK_01", sCheck01 },
                            { "CHECK_02", sCheck02 },
                            { "JOINDATE_FROM", sJoinDateFrom },
                            { "JOINDATE_TO", sJoinDateTo },
                            { "RETIREDATE_FROM", sRetireDateFrom },
                            { "RETIREDATE_TO", sRetireDateTo },
                            { "STANDARD_DATE", sStandardDate }
                        }
                    };

                    // 인사카드 조회 권한 확인 프로시저 호출 요청 객체
                    DbProcedureRequest authFlagRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmST106_GET_SYSTEM_AUTH_FLAG",
                        Division = "authFlag",
                      
                        Parameters = new Dictionary<string, object>()
                        {
                            { "CTRL_CODE", "IN0005" },
                            { "EMPLOYEE_NO", sEmployeeNo },
                            
                        }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { employeeListRequest, authFlagRequest });

                    if (result.Result)
                    {
                        _logger.LogDebug("직원 리스트 조회 완료");

                        var employeeResult = result.ResultList?.FirstOrDefault(r => r.Name == "SP_WHR206_01_LIST");
                        var authResult = result.ResultList?.FirstOrDefault(r => r.Name == "SP_WEB_FrmST106_GET_SYSTEM_AUTH_FLAG");


                        DataTable? dt = employeeResult?.DataTable;
                        DataTable? authDt = authResult?.DataTable;

                        bool authFlag = authDt != null && authDt.Rows.Count > 0; // 권한이 있는지 확인


                        DataTable filteredTable = new DataTable();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var selectedColumns = new string[]
                             {
                                "EMPLOYEE_NO",
                                "BASE_NAME",
                                "LEVEL_NAME",
                                "DEPARTMENT_CODE",
                                "DEPARTMENT_NAME",
                                "HANDPHONE_NO",
                                "E_MAIL",
                                "EMP_IMAGE_GUBUN"
                             };

                            // 새로운 DataTable 생성 및 필요한 컬럼만 추가
                            filteredTable = dt.DefaultView.ToTable(false, selectedColumns);

                            if (!filteredTable.Columns.Contains("USER_IMAGE_URL"))
                            {
                                filteredTable.Columns.Add("USER_IMAGE_URL", typeof(string));
                            }
                            if (!filteredTable.Columns.Contains("CARD_URL"))
                            {
                                filteredTable.Columns.Add("CARD_URL", typeof(string));
                            }


                            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                            foreach (DataRow row in filteredTable.Rows)
                            {
                                string employeeNo = row["EMPLOYEE_NO"]?.ToString()?.Trim() ?? string.Empty;
                                string empImageGubun = row["EMP_IMAGE_GUBUN"].ToString() ?? string.Empty;
                                string imageFtpUrl = _envService.GetImageUrl();
                                string empCardUrl = _envService.GetEmpCardUrl();

                                // 현재 시간을 밀리초 단위로 Unix Timestamp 변환

                                if (empImageGubun == "1")
                                {
                                    row["USER_IMAGE_URL"] = $"{imageFtpUrl}{employeeNo}?{timestamp}";
                                }
                                else
                                {
                                    row["USER_IMAGE_URL"] = $"{imageFtpUrl}0000?{timestamp}";
                                }

                                if (authFlag)
                                {
                                    row["CARD_URL"] = $"{empCardUrl}" +
                                                      $"FACTORY_CODE={sFactoryCode}" +
                                                      $"&EMPLOYEE_NO={employeeNo}" +
                                                      $"&IMGGBN={empImageGubun}" +
                                                      $"&DATEMSEC={timestamp}" +
                                                      $"&FTPURL={imageFtpUrl}" +
                                                      $"&OPMAN_CODE={employeeNo}";
                                }
                                else
                                {
                                    row["CARD_URL"] = string.Empty; // 권한이 없으면 빈 값
                                }

                            }

                            // `authFlag` 값을 DataTable로 변환
                            DataTable authTable = new DataTable();
                            authTable.Columns.Add("authFlag", typeof(bool));
                            authTable.Rows.Add(authFlag);

                            // 기존 ResultList 구조 유지하면서 `ReturnValue` 반영
                            var jsonResponse = new
                            {
                                Result = true,
                                ResultList = new List<object>
                                {
                                    new
                                    {
                                        Name = employeeResult?.Name ?? "SP_WHR206_01_LIST",
                                        Division = "employeeList",
                                        ReturnValue = employeeResult?.ReturnValue ?? 0, // 실제 ReturnValue 적용
                                        AuthFlag = authFlag, // authFlag 값을 여기에 추가
                                        DataTable = JsonHelper.ConvertDbResultToJsonObject(filteredTable)
                                    },
                                    //new
                                    //{
                                    //    Name = authResult?.Name ?? "SP_WEB_FrmST106_GET_SYSTEM_AUTH_FLAG",
                                    //    Division = "authFlag",
                                    //    ReturnValue = authResult?.ReturnValue ?? 0, // 실제 ReturnValue 적용
                                    //    DataTable = JsonHelper.ConvertDbResultToJsonObject(authTable)
                                    //},
                                    //new
                                    //{
                                    //    Name = employeeResult?.Name ?? "SP_WHR206_01_LIST",
                                    //    Division = "employeeList",
                                    //    ReturnValue = employeeResult?.ReturnValue ?? 0, // 실제 ReturnValue 적용
                                    //    DataTable = JsonHelper.ConvertDbResultToJsonObject(filteredTable)
                                    //}
                                    
                                }
                            };

                            return jsonResponse;


                        }
                        else
                        {
                            var jsonResponse = new
                            {
                                Result = true,
                                ResultList = new List<object>
                                {
                                    new
                                    {
                                        Name = employeeResult?.Name ?? "SP_WHR206_01_LIST",
                                        Division = "employeeList",
                                        ReturnValue = employeeResult?.ReturnValue ?? 0, // 실제 ReturnValue 적용
                                        AuthFlag = authFlag, // authFlag 값을 여기에 추가
                                        DataTable = JsonHelper.ConvertDbResultToJsonObject(filteredTable)
                                    },
                                    
                                }
                            };

                            return jsonResponse;
                        };
                        
                    }
                    else
                    {
                        throw new Exception("직원 리스트 조회 실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "직원 리스트 조회 중 오류 발생");
                throw new Exception("직원 리스트 조회 실패");
            }
        }
    }
}
