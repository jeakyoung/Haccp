using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Users.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using Newtonsoft.Json;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Auth.Models
{
    /// <summary>
    /// 사용자 관리 모듈
    /// </summary>
    public class UserManager
    {
        private readonly string _connectionString;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// 사용자 관리 서비스
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public UserManager(ILogger<AuthController> logger, IEnvService envService)
        {
            _connectionString = envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// db에서 사용자 정보 인증 (패스워트 인증 포함)
        /// </summary>
        /// <param name="auth"></param>
        /// <returns></returns>
        public async Task<AuthResponse?> GetAppUserAsync(AuthRequest auth)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    DbProcedureRequest request = new DbProcedureRequest();
                    request.ProcedureName = "SP_WEB_LOGIN";
                    request.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", auth.UserID!},
                        { "USER_PW", auth.UserPassword! },
                        { "IP_INFO", "" }
                    };


                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request });

                    if (result.Result)
                    {
                        if (result.ResultList![0].DataTable!.Rows.Count == 0)
                        {
                            //유효하지 않은 사용자
                            return null;
                        }

                        //AppUser appUser = result.ResultList![0].DataTable!.AsEnumerable().Select(row => new AppUser
                        //{
                        //    Id = row.Field<string>("USER_ID")!,
                        //    UserName = row.Field<string>("CHARGE_NAME"),
                        //    Email = "test@test.com"
                        //}).ToList().FirstOrDefault()!;

                        AuthResponse authResponse = result.ResultList![0].DataTable!.AsEnumerable().Select(row => new AuthResponse 
                        {
                            EmployeeNo = row.Field<string>("EMPLOYEE_NO") ?? string.Empty,
                            ChargeName = row.Field<string>("CHARGE_NAME") ?? string.Empty,
                            FactoryCode = row.Field<string>("FACTORY_CODE") ?? string.Empty,
                            Grade = row.Field<string>("GRADE") ?? string.Empty,
                            UserId = row.Field<string>("USER_ID") ?? string.Empty,
                            // UserPw = row.Field<string>("USER_PW") ?? string.Empty,
                            DepartmentCode = row.Field<string>("DEPARTMENT_CODE") ?? string.Empty,
                            PlantCode = row.Field<string>("PLANT_CODE") ?? string.Empty,
                            CodeNameFull = row.Field<string>("CODE_NAME_FULL") ?? string.Empty,
                            PlantName = row.Field<string>("PLANT_NAME") ?? string.Empty,
                            DepartmentName = row.Field<string>("DEPARTMENT_NAME") ?? string.Empty,
                            Imabmp2 = row.Field<string>("IMABMP2") ?? string.Empty,
                            SignImage = row.Field<string>("SIGN_IMAGE") ?? string.Empty,
                            DefaultSignFlag = row.Field<string>("DEFAULT_SIGN_FLAG") ?? string.Empty,
                            HandphoneNo = row.Field<string>("HANDPHONE_NO") ?? string.Empty,
                            AddrMail = row.Field<string>("ADDR_MAIL") ?? string.Empty,
                            AddrBtms = row.Field<string>("ADDR_BTMS") ?? string.Empty,
                            ChagepwFlag = row.Field<string>("CHAGEPW_FLAG") ?? string.Empty,
                            Email = row.Field<string>("E_MAIL") ?? string.Empty,
                            EmailPw = row.Field<string>("EMAIL_PW") ?? string.Empty,
                            PublicEmailCnt = row.Field<int?>("PUBLIC_EMAIL_CNT") ?? 0,
                            //AnnualCntText = row.Field<string>("ANNUAL_CNT_TEXT") ?? string.Empty,
                        }).ToList().FirstOrDefault()!;

                        _logger.LogDebug(" 완료");
                        return authResponse;
                    }
                    else
                    {
                        //유효하지 않은 요청
                        return null;
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "실패");
                return null;
            }
            
        }

        /// <summary>
        /// 사용자 패스워드 유효성 검사
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> CheckPasswordAsync(AppUser user, string password)
        {
            await Task.Delay(10);
            //db에서 체크하는 기능 추가 (비동기 방식으로 변경)
            // 예시: await _context.Users.FirstOrDefaultAsync(u => u.Id == userID);

            return true;
        }



    }
}
