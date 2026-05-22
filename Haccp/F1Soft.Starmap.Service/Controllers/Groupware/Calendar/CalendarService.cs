using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar
{
    /// <summary>
    /// 캘린더 서비스
    /// </summary>
    public class CalendarService : ICalendarService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<CalendarService> _logger;

        /// <summary>
        /// 캘린더 서비스 생성자
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public CalendarService(IEnvService envService, ILogger<CalendarService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 캘린더 전체 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object> GetCalendarAllList(CalendarAllListRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sDate = request.SDate ?? string.Empty;
                    string eDate = request.EDate ?? string.Empty;
                    string employeeNo = request.EmployeeNo ?? string.Empty;
                    string viewEmployeeNo = request.EmployeeNo ?? string.Empty;

                    // 개인
                    DbProcedureRequest personalRequest = new DbProcedureRequest();
                    personalRequest.ProcedureName = "SP_WEB_FrmBP109_01_LIST2";
                    personalRequest.Division = "personal";
                    personalRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "@SDATE", sDate },
                        { "@EDATE", eDate },
                        { "@EMPLOYEE_NO", employeeNo },
                        { "@VIEW_EMPLOYEE_NO", viewEmployeeNo }
                    };

                    // 전사
                    DbProcedureRequest enterpriseRequest = new DbProcedureRequest();
                    enterpriseRequest.ProcedureName = "SP_WEB_FrmBP109_01_LIST4";
                    enterpriseRequest.Division = "enterprise";
                    enterpriseRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "@SDATE", sDate },
                        { "@EDATE", eDate },
                        { "@EMPLOYEE_NO", employeeNo },
                        { "@VIEW_EMPLOYEE_NO", viewEmployeeNo }
                    };

                    // 공유
                    DbProcedureRequest shareResult = new DbProcedureRequest();
                    shareResult.ProcedureName = "SP_WEB_FrmBP109_01_LIST5";
                    shareResult.Division = "share";
                    shareResult.Parameters = new Dictionary<string, object>()
                    {
                        { "@SDATE", sDate },
                        { "@EDATE", eDate },
                        { "@EMPLOYEE_NO", employeeNo },
                        { "@VIEW_EMPLOYEE_NO", viewEmployeeNo }
                    };


                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { personalRequest, enterpriseRequest, shareResult });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        return JsonHelper.ConvertDbResultToJsonObject(result);
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "캘린더 데이터 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

    }
}
