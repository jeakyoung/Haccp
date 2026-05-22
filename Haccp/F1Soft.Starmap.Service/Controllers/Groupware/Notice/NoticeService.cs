using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Notice.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using System.Data;

namespace F1Soft.Starmap.Service.Services.Groupware.Notice
{
    /// <summary>
    /// 공지사항 서비스
    /// </summary>
    public class NoticeService : INoticeService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<NoticeService> _logger;
        /// <summary>
        /// Logger
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public NoticeService(IEnvService envService, ILogger<NoticeService> logger)
        {
            _envService = envService;
            _logger = logger;
            _connectionString = _envService.GetConnectionString();
        }
        /// <summary>
        /// 공지사항 리스트 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> GetNoticeList(NoticeRequest request)
        {
            string EDate = request.EDate ?? string.Empty;
            string SDate = request.SDate ?? string.Empty;
            string EmployeeNo = request.EmployeeNo ?? string.Empty;
            string EmployeeName = request.EmployeeName ?? string.Empty;
            string Title = request.Title ?? string.Empty;
            string StatusGbn = request.StatusGbn ?? string.Empty;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    
                    DbProcedureRequest noticeRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP103_01_LIST",
                        Division = "NoticeList",
                        Parameters = new Dictionary<string, object>()
                    {
                        { "TODATE1", SDate },
                        { "FROMDATE1", EDate },
                        { "EMPLOYEE_NO", EmployeeNo },
                        { "EMP_NAME", EmployeeName },
                        { "TITLE_NAME", Title },
                        { "STATUS_GBN", StatusGbn }
                    }
                    };

                    var result = await helper.CallProcedureAsync( new List<DbProcedureRequest> { noticeRequest }
                    );

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        DataTable? dt = result.ResultList?.FirstOrDefault()?.DataTable;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            if (dt.Columns.Contains("COMMENT"))
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    row["COMMENT"] = JsCleaner(row["COMMENT"]?.ToString());
                                }
                            }

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
                _logger.LogError(ex, "공지사항 데이터 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }



        // JS 요소 원문 변환기
        private string JsCleaner(string? html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            string decoded = System.Web.HttpUtility.HtmlDecode(html);

            decoded = decoded
                .Replace("\n", "")
                .Replace("\t", " ");

            //html 태그 삭제기 ( img 태그 삭제 )
            //decoded = System.Text.RegularExpressions.Regex.Replace(decoded, "<img.*?>", string.Empty);
            decoded = decoded.Replace("&nbsp;", " ");

            return decoded.Trim();
        }
    }
}