using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Setting.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using System.Data;

namespace F1Soft.Starmap.Service.Services.Groupware.Setting
{
    /// <summary>
    /// 공지사항 서비스
    /// </summary>
    public class SettingService : ISettingService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<SettingService> _logger;
        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public SettingService(IEnvService envService, ILogger<SettingService> logger)
        {
            _envService = envService;
            _logger = logger;
            _connectionString = _envService.GetConnectionString();
        }
        /// <summary>
        /// 설정사항 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> GetSettingList(SettingListRequest request)
        {
            string sTicker = request.ticker ?? string.Empty;
            string sEmployeeNo = request.employeeNo ?? string.Empty;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    DbProcedureRequest settingRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_TGI003_01_LIST",
                        Division = "setting",
                        Parameters = new Dictionary<string, object>()
                    {
                        { "TICKER", sTicker },
                        { "EMPLOYEE_NO", sEmployeeNo }
                    }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { settingRequest }
                    );

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        DataTable? dt = result.ResultList?.FirstOrDefault()?.DataTable;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            return JsonHelper.ConvertDbResultToJsonObject(result);
                        }
                        else
                        {
                            return new { Message = "데이터가 없습니다." };
                        }
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "설정 정보 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 설정사항 저장 ( 저장 호출시에 필히 플래그값 다보내줘야만함 )
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> SetSetting(SetSettingRequest request)
        {
            string sTicker = request.ticker ?? string.Empty;
            string sEmployeeNo = request.employeeNo ?? string.Empty;

            //토글 버튼 사용으로 파라미터를 하나만 넘길때 들어오지 않은 요청에 대해 DB_NULL 처리 -> 한개의 항목만 업데이트 가능
            object sApproval = string.IsNullOrWhiteSpace(request.approvalChkFlag) ? DBNull.Value : request.approvalChkFlag;
            object sBoard = string.IsNullOrWhiteSpace(request.boardChkFlag) ? DBNull.Value : request.boardChkFlag;
            object sNotify = string.IsNullOrWhiteSpace(request.notiChkFlag) ? DBNull.Value : request.notiChkFlag;
            object sSchedule = string.IsNullOrWhiteSpace(request.scheduleChkFlag) ? DBNull.Value : request.scheduleChkFlag;

            //string sApproval = request.approvalChkFlag ?? string.Empty;
            //string sBoard = request.boardChkFlag ?? string.Empty;
            //string sNotify = request.notiChkFlag ?? string.Empty;
            //string sSchedule = request.scheduleChkFlag ?? string.Empty;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    DbProcedureRequest setSetting = new DbProcedureRequest
                    {
                        ProcedureName = "SP_TGI003_01_IUD",
                        Division = "setting",
                        Parameters = new Dictionary<string, object>()
                    {
                        { "TICKER", sTicker },
                        { "EMPLOYEE_NO", sEmployeeNo },
                        { "APPROVAL_CHK_FLAG", sApproval },
                        { "BOARD_CHK_FLAG", sBoard },
                        { "NOTI_CHK_FLAG", sNotify },
                        { "SCHEDULE_CHK_FLAG", sSchedule },
                    }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { setSetting });

                    if (result == null)
                        throw new Exception("실패");

                    if (result.Result)
                    {
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
                _logger.LogError(ex, "FCM 열람 기록 저장 실패");
                throw new Exception("실패");
            }
        }
    }
}