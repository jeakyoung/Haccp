# Solution: F1Soft.Starmap.Service

## Solution File: F1Soft.Starmap.Service.sln

```
Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 17
VisualStudioVersion = 17.11.35208.52
MinimumVisualStudioVersion = 10.0.40219.1
Project("{9A19103F-16F7-4668-BE54-9A1E7A4F7556}") = "F1Soft.Starmap.Service", "F1Soft.Starmap.Service\F1Soft.Starmap.Service.csproj", "{1FEEC658-1D2C-404C-904D-28206FFAE714}"
EndProject
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "F1Soft.Starmap.Core", "F1Soft.Starmap.Core\F1Soft.Starmap.Core.csproj", "{1759AA3C-F625-4EA4-9DDA-0959F075284C}"
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
		Staging|Any CPU = Staging|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Release|Any CPU.Build.0 = Release|Any CPU
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Staging|Any CPU.ActiveCfg = Staging|Any CPU
		{1FEEC658-1D2C-404C-904D-28206FFAE714}.Staging|Any CPU.Build.0 = Staging|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Release|Any CPU.Build.0 = Release|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Staging|Any CPU.ActiveCfg = Debug|Any CPU
		{1759AA3C-F625-4EA4-9DDA-0959F075284C}.Staging|Any CPU.Build.0 = Debug|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {1CA130F8-61D1-4705-82D6-67D7519C05B9}
	EndGlobalSection
EndGlobal

```

## Project: F1Soft.Starmap.Service

### Project File: F1Soft.Starmap.Service.csproj

```<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <UserSecretsId>32d02311-005f-4441-bd9c-6ebfece82ae5</UserSecretsId>
    <DockerDefaultTargetOS>Linux</DockerDefaultTargetOS>
    <DockerfileContext>.</DockerfileContext>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <Configurations>Debug;Release;Staging</Configurations>
  </PropertyGroup>

  <ItemGroup>
    <Compile Remove="Migrations\**" />
    <Compile Remove="Services\NewFolder\**" />
    <Content Remove="Migrations\**" />
    <Content Remove="Services\NewFolder\**" />
    <EmbeddedResource Remove="Migrations\**" />
    <EmbeddedResource Remove="Services\NewFolder\**" />
    <None Remove="Migrations\**" />
    <None Remove="Services\NewFolder\**" />
  </ItemGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.10" />
    <PackageReference Include="Microsoft.AspNetCore.SignalR" Version="1.2.0" />
    <PackageReference Include="Microsoft.Data.SqlClient" Version="5.2.2" />
    <PackageReference Include="Microsoft.VisualStudio.Azure.Containers.Tools.Targets" Version="1.21.0" />
    <PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="9.0.0" />
    <PackageReference Include="Npgsql" Version="8.0.5" />
    <PackageReference Include="Oracle.ManagedDataAccess.Core" Version="23.6.0" />
    <PackageReference Include="SapNwRfc" Version="1.4.0" />
    <PackageReference Include="Swashbuckle.AspNetCore" Version="6.8.1" />
    <PackageReference Include="Swashbuckle.AspNetCore.Annotations" Version="6.9.0" />
  </ItemGroup>

  <ItemGroup>
    <Folder Include="Controllers\Database\MySQL\" />
    <Folder Include="Controllers\Groupware\EA\" />
    <Folder Include="Controllers\Groupware\TPM\" />
  </ItemGroup>

</Project>

```

### UserManager.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### AuthController.cs

```using F1Soft.Starmap.Service.Controllers.Auth.Models;
using F1Soft.Starmap.Service.Controllers.Users.Models;
using F1Soft.Starmap.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace F1Soft.Starmap.Service.Controllers.Auth;

/// <summary>
/// JWT 권한 인증
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly TokenService _tokenService;
    private readonly UserManager _userManager;

    /// <summary>
    /// 권한 인증 생성자
    /// </summary>
    /// <param name="tokenService"></param>
    /// <param name="logger"></param>
    /// <param name="userManager"></param>
    public AuthController(TokenService tokenService, ILogger<AuthController> logger, UserManager userManager)
    {
        _logger = logger;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    /// <summary>
    /// 인증 - 사용자 로그인 (로그인 시 JWT 토큰 반환)
    /// </summary>
    /// <param name="request">유효한 ID와 Password를 입력하세요</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "userID": "19039",
    ///         "userPassword": "f1soft@6"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.GetAppUserAsync(request);

        if (managedUser is null)
            return BadRequest("Bad credentials");

        //토큰 무조건 발행 (같은 날이면 그대로 사용??)
        var accessToken = _tokenService.CreateToken(managedUser);

        managedUser.Token = accessToken;

        return Ok(managedUser);
    }

    /// <summary>
    /// 자동 로그인
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("AutoLogin")]
    public async Task<ActionResult<AuthResponse>> AutoLogin()
    {
        string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        AuthRequest authRequest = new AuthRequest();
        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        //토큰이 유효하지 않으면 401 반환
        if(!isValid)
            return Unauthorized(new { message = "Invalid token" });


        //jwt를 authResponse로 변환
        var claims = _tokenService.DecodeToken(token);
        var authResponse = _tokenService.GetAppUserFromClaimsPrincipal(claims);
        authRequest.UserID = authResponse.UserId;
        authRequest.UserPassword = "f1soft@6";

        return await Authenticate(authRequest);
    }


    /// <summary>
    /// 인증 테스트 API (JWT 인증 받은 사용자만 API 호출 가능)
    /// </summary>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpGet]
    [Route("TestAuthorize")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    public async Task<ActionResult<AuthResponse>> TestAuthorize()
    {
        await Task.Delay(100);
        return Ok("authorize success");
    }

    /// <summary>
    /// 토큰이 유효한지 확인 (파라미터 사용)
    /// </summary>
    /// <param name="token">JWT 토큰</param>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpPost]
    [Route("ValidateStringToken")]
    public IActionResult ValidateStringToken(string token)
    {
        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        if (isValid)
        {
            // 토큰이 유효하면 토큰 정보 반환
            // (예: 토큰에 포함된 사용자 정보 등)
            return Ok(new { message = "Token is valid", token = token });
        }
        else
        {
            return Unauthorized(new { message = "Invalid token" });
        }
    }

    /// <summary>
    /// 토큰이 유효한지 확인 (헤더 사용)
    /// </summary>
    /// <returns></returns>
    /// <returns>인증 성공 여부</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///
    /// </remarks>
    [HttpPost]
    [Route("ValidateHeaderToken")]
    public IActionResult ValidateHeaderToken()
    {
        // Authorization 헤더에서 토큰 가져오기
        string token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        // 토큰이 유효한지 확인
        bool isValid = ValidateJwtToken(token);

        if (isValid)
        {
            // 토큰이 유효하면 토큰 정보 반환
            // (예: 토큰에 포함된 사용자 정보 등)
            return Ok(new { message = "Token is valid", token = token });
        }
        else
        {
            return Unauthorized(new { message = "Invalid token" });
        }
    }

    private bool ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
                ValidAudience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }

}
```

### DbProcedure.cs

```using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Database.Models
{
    /// <summary>
    /// Database Procedure 요청
    /// </summary>
    public class DbProcedureRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public string? ProcedureName { get; set; }

        /// <summary>
        /// DataTable 구분
        /// </summary>
        public string? Division { get; set; }

        /// <summary>
        /// 변경 프로시져 여부
        /// </summary>
        public bool IsChangeProcedure { get; set; } = false;

        /// <summary>
        /// 파라미터 리스트
        /// </summary>
        public Dictionary<string, object>? Parameters { get; set; }
    }

    /// <summary>
    /// Database Command 요청
    /// </summary>
    public class DbSqlRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public object? SqlCommand { get; set; }

        /// <summary>
        /// 변경 프로시져 여부
        /// </summary>
        public bool IsChangeProcedure { get; set; } = false;
    }

    /// <summary>
    /// Database 호출 결과
    /// </summary>
    public class DbResult
    {
        /// <summary>
        /// 쿼리 결과 (true 성공, false 실패)
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 결과 리스트
        /// </summary>
        public List<DbResultItem>? ResultList { get; set; }
        /// <summary>
        /// 에러 메세지
        /// </summary>
        public string? ErrorMessage { get; set; }
    }

    /// <summary>
    /// Database 호출 결과 항목
    /// </summary>
    public class DbResultItem
    {
        /// <summary>
        /// 결과 이름 (테이블 명)
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// DataTable 구분
        /// </summary>
        public string? Division { get; set; }

        /// <summary>
        /// 결과 count (변경된 행 갯수 or SELECT 된 행 갯수)
        /// </summary>
        public int ReturnValue { get; set; }

        /// <summary>
        /// 결과 값 (테이블)
        /// </summary>
        public DataTable? DataTable { get; set; }

        /// <summary>
        /// 항목 에레 메세지
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}

```

### OracleQueryController.cs

```//using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### PostgreSQLController.cs

```//using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### SQLServerQueryController.cs

```//using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### ApprovalAnswerRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 댓글 작성 요청 모델
    /// </summary>
    public class ApprovalAnswerRequest
    {
        /// <summary>
        /// 업무 번호
        /// </summary>
        [Required]
        [StringLength(3, ErrorMessage = "업무 번호는 최대 3자리만 가능합니다.")]
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 전자문서 실행 ID
        /// </summary>
        [Required]
        [StringLength(12, ErrorMessage = "전자문서 실행 ID는 최대 12자리만 가능합니다.")]
        public string? EaExeId { get; set; } = string.Empty;

        /// <summary>
        /// 구분 코드
        /// </summary>
        [Required]
        public int? GbnCode { get; set; }

        /// <summary>
        /// 실행 순번
        /// </summary>
        [Required]
        public int? ExeSeq { get; set; }

        /// <summary>
        /// 답변 순번
        /// </summary>
        [Required]
        public int? AnswerSeq { get; set; } = 0;

        /// <summary>
        /// 답변 구분 이름
        /// </summary>
        [StringLength(20, ErrorMessage = "답변 구분 이름은 최대 20자리만 가능합니다.")]
        public string? AnswerGbnName { get; set; } = string.Empty;

        /// <summary>
        /// 답변 시간
        /// </summary>
        public string? AnswerTime { get; set; } = string.Empty;

        /// <summary>
        /// 답변 내용
        /// </summary>
        [StringLength(4000, ErrorMessage = "답변 내용은 최대 4000자리만 가능합니다.")]
        public string? AnswerCnt { get; set; } = string.Empty;

        /// <summary>
        /// 작업자 코드
        /// </summary>
        [StringLength(10, ErrorMessage = "작업자 코드는 최대 10자리만 가능합니다.")]
        public string? OpmanCode { get; set; } = string.Empty;

        /// <summary>
        /// 작업 시간
        /// </summary>
        public string? Optime { get; set; } = string.Empty;

        /// <summary>
        /// 메인 뷰 플래그
        /// </summary>
        [StringLength(1, ErrorMessage = "메인 뷰 플래그는 최대 1자리만 가능합니다.")]
        public string? MainViewFlag { get; set; } = "0";

        /// <summary>
        /// 작업 구분 (I/U/D)
        /// </summary>
        [StringLength(3, ErrorMessage = "작업 구분은 최대 3자리만 가능합니다.")]
        public string? Iud { get; set; } = string.Empty;
    }
}

```

### ApprovalDetailRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 본문 요청 모델
    /// </summary>
    public class ApprovalDetailRequest
    {
        /// <summary>
        /// 전자문서 실행 ID (모든 요청에서 사용)
        /// </summary>
        [Required]
        public string? EaExeId { get; set; } = string.Empty;

        /// <summary>
        /// 업무 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 직원 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 구분 코드 (결재의견에서 사용)
        /// </summary>
        public int? GbnCode { get; set; }

        /// <summary>
        /// 결재순번
        /// </summary>
        public int? ExeSeq { get; set; }


    }
}

```

### ApprovalRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models

{
    /// <summary>
    /// 미결함 열람 요청 모델
    /// </summary>
    public class ApprovalRequest
    {
        /// <summary>
        /// 사용자 ID
        /// </summary>
        [Required]
        [StringLength(10, ErrorMessage = "사용자 ID는 최대 10자리만 가능합니다.")]
        public string? UserId { get; set; } = string.Empty;

        /// <summary>
        /// 시작 날짜
        /// </summary>
        [StringLength(8, ErrorMessage = "시작 날짜는 최대 8자리만 가능합니다.")]
        public string? FromDate { get; set; } = string.Empty;

        /// <summary>
        /// 종료 날짜
        /// </summary>
        [StringLength(8, ErrorMessage = "종료 날짜는 최대 8자리만 가능합니다.")]
        public string? ToDate { get; set; } = string.Empty;

        #region 추후 사용 할 수 있음

        ///// <summary>
        ///// 등록자명
        ///// </summary>
        //[StringLength(10, ErrorMessage = "등록자명은 최대 10자리만 가능합니다.")]
        //public string? RegmanName { get; set; } = string.Empty;

        ///// <summary>
        ///// 제목
        ///// </summary>
        //[StringLength(200, ErrorMessage = "제목은 최대 200자리만 가능합니다.")]
        //public string? EaTitle { get; set; } = string.Empty;

        ///// <summary>
        ///// 실행 ID
        ///// </summary>
        //[StringLength(12, ErrorMessage = "실행 ID는 최대 12자리만 가능합니다.")]
        //public string? EaExeId { get; set; } = string.Empty;


        ///// <summary>
        ///// 업무 번호
        ///// </summary>
        //public string? EabusNo { get; set; } = "";
        #endregion

    }
}

```

### SubmitApprovalRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 파라미터 요청 모델
    /// </summary>
    public class SubmitApprovalRequest
    {
        /// <summary>
        /// 다중승인 플래그
        /// </summary>
        [Required]
        public string? MuldecFlag { get; set; } = string.Empty;

        /// <summary>
        /// 최종 승인자 플래그
        /// </summary>
        public string? LastCnfrmerFlag { get; set; } = string.Empty;

        /// <summary>
        /// 최종 소유자 플래그
        /// </summary>
        public string? LastOwnerFlag { get; set; } = string.Empty;

        /// <summary>
        /// 전자문서 실행 ID
        /// </summary>
        [Required]
        public string? EaExeId { get; set; } = string.Empty;

        /// <summary>
        /// 구분 코드
        /// </summary>
        [Required]
        public int? GbnCode { get; set; }

        /// <summary>
        /// 실행 순번
        /// </summary>
        [Required]
        public int? ExeSeq { get; set; }

        /// <summary>
        /// 주문 순번
        /// </summary>
        [Required]
        public int? OrderSeq { get; set; }

        /// <summary>
        /// 옵션 이름
        /// </summary>
        public string? OptionName { get; set; } = string.Empty;

        /// <summary>
        /// 승인 플래그
        /// </summary>
        public string? AppFlag { get; set; } = string.Empty;

        /// <summary>
        /// 업무 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EabusNo { get; set; } = string.Empty;

        /// <summary>
        /// 직원 번호 (결재내용, 첨부파일, 결재의견에서 사용)
        /// </summary>
        public string? EmployeeNo { get; set; } = string.Empty;
    }
}

```

### ApprovalController.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval;

/// <summary>
/// 기본 컨트롤러
/// [Authorize]: 모든 기능은 인증된 유저(로그인 성공)만 사용 가능하게 제한 (단, DEBUG 모드 예외)
/// </summary>
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class ApprovalController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly IApprovalService _approvalService;

    /// <summary>
    /// SQLServerQueryController 생성자
    /// </summary>
    /// <param name="envService"></param>
    /// <param name="logger"></param>
    /// <param name="approvalService"></param>
    public ApprovalController(ILogger<ApprovalController> logger, IEnvService envService, IApprovalService approvalService)
    {
        _approvalService = approvalService;
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
    }

    /// <summary>
    /// 본문내용.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///      "eaExeId": "202410140039",
    ///      "eabusNo": "001",
    ///      "employeeNo": "19039",
    ///      "gbnCode": 1,
    ///      "exeSeq": 7
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("GetApprovalDetailList")]
    public async Task<IActionResult> GetApprovalDetailList([FromBody] ApprovalDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }


            var result = await _approvalService.GetApprovalDetailList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 미결함, 보관함, 기안함, 결재함
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///       "userID": "19039",
    ///       "todate": "20250204",
    ///       "fromdate": "20250104"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("GetApprovalAllList")]
    public async Task<IActionResult> GetApprovalAllList([FromBody] ApprovalRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.GetApprovalAllList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }


    /// <summary>
    /// 문서 열람 시간 등록
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Param은 GetApprovalDetailList와 동일
    /// </remarks>
    [HttpPost("CheckOpenTime")]
    public async Task<IActionResult> CheckOpenTime([FromBody] ApprovalDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.CheckOpenTime(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "실패", Error = ex.Message });
        }
    }


    /// <summary>
    /// 승인, 확인, 반송, 합의
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///       "muldecFlag": "1",
    ///       "lastCnfrmerFlag": "1",
    ///       "lastOwnerFlag": "0",
    ///       "eaExeId": "202501310010",
    ///       "gbnCode": 1,
    ///       "exeSeq": 2,
    ///       "orderSeq": 2,
    ///       "optionName": "승인",
    ///       "appFlag": "1",
    ///       "eabusNo": "001",
    ///       "employeeNo": "19039"
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("ConfirmApproval")]
    public async Task<IActionResult> ConfirmApproval([FromBody] SubmitApprovalRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _approvalService.ConfirmApproval(request);

            _logger.LogDebug("결재 성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "결재 실패");
            return StatusCode(500, new { Message = "결재 실패", Error = ex.Message });
        }
    }

    /// <summary>
    /// 댓글 작성
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     {
    ///         "eabusNo": "001",                     
    ///         "eaExeId": "202501220005",            
    ///         "gbnCode": 1,                         
    ///         "exeSeq": 1,                          
    ///         "answerCnt": "댓글 테스트",           
    ///         "opmanCode": "0000",                  
    ///         "mainViewFlag": "0"                  
    ///     }
    ///
    /// </remarks>
    [HttpPost]
    [Route("PostAnswer")]
    public async Task<IActionResult> PostAnswer([FromBody] ApprovalAnswerRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.EaExeId))
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다. (EaExeId 필수)" });
            }

            var result = await _approvalService.PostAnswer(request);

            _logger.LogDebug("댓글작성 완료");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "댓글작성 실패");
            return StatusCode(500, new { Message = "실패", Error = ex.Message });
        }
    }

}
```

### ApprovalService.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval
{
    /// <summary>
    /// 결재 컨트롤에서 사용할 서비스
    /// </summary>
    public class ApprovalService : IApprovalService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<ApprovalService> _logger;

        /// <summary>
        /// Approval 서비스
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public ApprovalService(IEnvService envService, ILogger<ApprovalService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 문서 열람시간 등록
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> CheckOpenTime(ApprovalDetailRequest request)
        {
            try
            {
                string sEaExeId = request.EaExeId?.ToString() ?? string.Empty;
                string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
                string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;
                int? iGbnCode = request.GbnCode ?? 0;
                int? iExeSeq = request.ExeSeq ?? 0;

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_STORE_FUNCTION_ChkOpenTime";
                    request1.Division = "doc";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "GBN_CODE", iGbnCode },
                        { "EABUS_NO", sEabusNo },
                        { "EXE_SEQ",  iExeSeq},
                        { "USERID", sEmployeeNo },
                    };
                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request1 });
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
                _logger.LogError(ex, "실패");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 승인, 확인, 반송 [결재 기능] + 합의 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> ConfirmApproval(SubmitApprovalRequest request)
        {
            try
            {
                string sMuldecFlag = request.MuldecFlag ?? string.Empty;
                string sLastCnfrmerFlag = request.LastCnfrmerFlag ?? string.Empty;
                string sLastOwnerFlag = request.LastOwnerFlag ?? string.Empty;
                string sEaExeId = request.EaExeId ?? string.Empty;
                int iGbnCode = request.GbnCode ?? 0;
                int iExeSeq = request.ExeSeq ?? 0;
                int iOrderSeq = request.OrderSeq ?? 0;
                string sOptionName = request.OptionName ?? string.Empty;
                string sAppFlag = request.AppFlag ?? string.Empty;

                string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
                string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;

                string sAnswerGbnName = string.Empty;
                string sAnswerCnt = string.Empty;

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    DbProcedureRequest procedureRequest;
                    // 결재 구분에 따른 답변 구분 이름 및 내용 설정
                    if (sAppFlag == "1" && sMuldecFlag == "4")
                    {
                        sOptionName = "합의";
                        sAnswerGbnName = "합의";
                        sAnswerCnt = "ⓜ[합의 하였습니다.]";
                    }
                    else if (sAppFlag == "1")
                    {
                        sAnswerGbnName = "승인";
                        sAnswerCnt = "ⓜ[승인 하였습니다.]";
                    }
                    else // sAppFlag == "2"
                    {
                        sAnswerGbnName = "확인";
                        sAnswerCnt = "ⓜ[확인 하였습니다.]";
                    }

                    if (sAppFlag == "1" || sAppFlag == "2")
                    {
                        procedureRequest = new DbProcedureRequest
                        {
                            ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_IUD",
                            Division = "agree",
                            Parameters = new Dictionary<string, object>
                            {
                                { "MULDEC_FLAG", sMuldecFlag },             // 결재구분
                                { "LAST_CNFRMER_FLAG", sLastCnfrmerFlag },  // 최종결재자 flag
                                { "LAST_OWNER_FLAG", sLastOwnerFlag },      // 최종소유자 flag
                                { "EA_EXE_ID", sEaExeId },                  // 기안번호
                                { "GBN_CODE", iGbnCode },                   // 기안, 조치 구분코드
                                { "EXE_SEQ", iExeSeq },                     // 결재순번
                                { "ORDER_SEQ", iOrderSeq },                 // 결재순번
                                { "OPTION_NAME", sOptionName },             // 결재 명칭 (승인, 확인, 반송 등)
                                { "APP_FLAG", sAppFlag },                   // 결재 flag(1: 승인, 2: 확인, 3: 반송 등)
                            }
                        };


                    }
                    else if (sAppFlag == "3")
                    {
                        procedureRequest = new DbProcedureRequest
                        {
                            ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_IUD2",
                            Division = "agree",
                            Parameters = new Dictionary<string, object>
                            {
                                { "MULDEC_FLAG", sMuldecFlag },             // 결재구분
                                { "LAST_CNFRMER_FLAG", sLastCnfrmerFlag },  // 최종결재자 flag
                                { "LAST_OWNER_FLAG", sLastOwnerFlag },      // 최종소유자 flag
                                { "EA_EXE_ID", sEaExeId },                  // 기안번호
                                { "GBN_CODE", iGbnCode },                   // 기안, 조치 구분코드
                                { "EXE_SEQ", iExeSeq },                     // 결재순번
                                { "ORDER_SEQ", iOrderSeq },                 // 결재순번
                                { "OPTION_NAME", sOptionName },             // 결재 명칭 (승인, 확인, 반송 등)
                                { "EMPLOYEE_NO", sEmployeeNo },
                            }
                        };
                        sAnswerGbnName = "반송";
                        sAnswerCnt = "ⓜ[반송 하였습니다.]";
                    }
                    else
                    {
                        throw new Exception("appFlag를 정확히 입력해 주세요(1: 승인, 2: 확인, 3: 반송, 4: 합의)");
                    }




                    // 프로시저 실행
                    var iudResult = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { procedureRequest });

                    if (!iudResult.Result)
                    {
                        _logger.LogError("결재 승인 프로시저 실패: {0}", iudResult.ErrorMessage);
                        throw new Exception("결재 승인 처리 중 오류 발생");
                    }

                    int answerSeq = await GetAnswerSeq(helper, sEaExeId, sEabusNo, iGbnCode, iExeSeq);

                    ApprovalAnswerRequest answerRequest = new ApprovalAnswerRequest
                    {
                        EabusNo = sEabusNo,
                        EaExeId = sEaExeId,
                        GbnCode = iGbnCode,
                        ExeSeq = iExeSeq,
                        AnswerSeq = answerSeq,
                        AnswerGbnName = sAnswerGbnName,
                        AnswerTime = "", // 사용 안함
                        AnswerCnt = sAnswerCnt,
                        OpmanCode = sEmployeeNo,
                        Optime = "", // 사용 안함
                        MainViewFlag = "0",
                        Iud = "I"
                    };

                    var answerResult = await PostAnswer(answerRequest);
                    if (answerResult == null)
                    {
                        _logger.LogError("댓글 등록 실패");
                        throw new Exception("댓글 등록 실패");
                    }

                    var result = await GetApprovalDetailList(new ApprovalDetailRequest
                    {
                        EaExeId = request.EaExeId,
                        EabusNo = request.EabusNo,
                        EmployeeNo = request.EmployeeNo,
                        GbnCode = request.GbnCode,
                        ExeSeq = request.ExeSeq
                    });

                    if (result == null)
                    {
                        _logger.LogError("결재 문서 조회 실패");
                        throw new Exception("결재 문서 정보를 찾을 수 없습니다.");
                    }

                    _logger.LogDebug("결재 승인 및 재조회 성공");
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"오류 발생: {ex.Message}");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 미결함, 보관함, 기안함, 결재함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetApprovalAllList(ApprovalRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {

                    string sUserId = request.UserId ?? string.Empty;
                    string sFromDate = request.FromDate ?? string.Empty;
                    string sToDate = request.ToDate ?? string.Empty;
                    string sRegmanName = "";
                    string sEaTitle = "";
                    string sEaExeId = "";
                    string sEabusNo = "";

                    // 추후에 사용하게 되면 model도 주석 풀고 실행해야 함
                    //string sRegmanName = request.RegmanName ?? string.Empty;
                    //string sEaTitle = request.EaTitle ?? string.Empty;
                    //string sEaExeId = request.EaExeId ?? string.Empty;
                    //string sEabusNo = request.EabusNo ?? string.Empty;

                    int iKeepCode = 1;

                    // 미결함 - fromdate, todate 반대로 넣어야 함
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_FrmEA121_01_LIST";
                    request1.Division = "dependence";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "REGMANNAME", sRegmanName },
                        { "EA_TITLE", sEaTitle },
                        { "EA_EXE_ID", sEaExeId },
                        { "FROMDATE", sToDate },
                        { "TODATE",  sFromDate},
                        { "EABUS_NO", sEabusNo }
                    };

                    // 보관함
                    DbProcedureRequest request2 = new DbProcedureRequest();
                    request2.ProcedureName = "SP_WEB_DASHBOARD_DASH_11";
                    request2.Division = "keep";
                    request2.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "KEEP_CODE", iKeepCode },
                    };

                    // 기안함
                    DbProcedureRequest request3 = new DbProcedureRequest();
                    request3.ProcedureName = "SP_WEB_FrmEA123_01_LIST";
                    request3.Division = "drafting";
                    request3.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "EA_EXE_ID", "" },
                        { "TODATE1", sFromDate },
                        { "FROMDATE1", sToDate },
                        { "REG_NAME", "" },
                        { "CUSTOMER_NAME", "" },
                        { "MATERIAL_CODE", "" },
                        { "MATERIAL_NAME", "" },
                        { "EA_TITLE", "" },
                        { "EABUS_NO", "" },
                    };

                    // 결재함
                    DbProcedureRequest request4 = new DbProcedureRequest();
                    request4.ProcedureName = "SP_WEB_FrmEA122_01_LIST";
                    request4.Division = "approval";
                    request4.Parameters = new Dictionary<string, object>()
                    {
                        { "USER_ID", sUserId },
                        { "EA_EXE_ID", sEaExeId },
                        { "TODATE1", sFromDate },
                        { "FROMDATE1", sToDate },
                        { "REG_NAME", "" },
                        { "TODATE2", "" },
                        { "FROMDATE2", "" },
                        { "CUSTOMER_NAME", "" },
                        { "MATERIAL_CODE", "" },
                        { "MATERIAL_NAME", "" },
                        { "EA_TITLE", sEaTitle },
                        { "EABUS_NO", sEabusNo },
                        { "RADIO", "2" },
                        { "CHECK", "0" }
                    };


                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request1, request2, request3, request4 });

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
                _logger.LogError(ex, "미결함, 보관함, 기안함, 결재함 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 결재 문서 상세 정보 (본문 내용)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<object> GetApprovalDetailList(ApprovalDetailRequest request)
        {

            string sEaExeId = request.EaExeId?.ToString() ?? string.Empty;
            string sEabusNo = request.EabusNo?.ToString() ?? string.Empty;
            string sEmployeeNo = request.EmployeeNo?.ToString() ?? string.Empty;
            int? iGbnCode = request.GbnCode ?? 0;
            int? iExeSeq = request.ExeSeq ?? 0;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    // 결재 내용
                    DbProcedureRequest request1 = new DbProcedureRequest();
                    request1.ProcedureName = "SP_WEB_FrmEA101_Window_01_LIST2";
                    request1.Division = "main";
                    request1.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "EMPLOYEE_NO", sEmployeeNo },
                    };

                    // 첨부파일
                    DbProcedureRequest request2 = new DbProcedureRequest();
                    request2.ProcedureName = "SP_WEB_FrmEA101_07_LIST";
                    request2.Division = "file";
                    request2.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "EMP_CODE", sEmployeeNo },
                    };

                    // 결재선
                    DbProcedureRequest request3 = new DbProcedureRequest();
                    request3.ProcedureName = "SP_WEB_FrmEA101_Window_02_LIST";
                    request3.Division = "sign";
                    request3.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                    };

                    // 댓글 리스트
                    DbProcedureRequest request4 = new DbProcedureRequest();
                    request4.ProcedureName = "SP_WEB_FrmEA101_Window_04_LIST";
                    request4.Division = "review";
                    request4.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "GBN_CODE", iGbnCode },
                        { "EMPLOYEE_NO", sEmployeeNo },

                    };

                    // 문서에 대한 나의 역할 및 상태
                    DbProcedureRequest request5 = new DbProcedureRequest();
                    request5.ProcedureName = "SP_WEB_STORE_FUNCTION_ChkAnswer_List";
                    request5.Division = "level";
                    request5.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId },
                        { "EABUS_NO", sEabusNo },
                        { "GBN_CODE", iGbnCode },
                        { "EXE_SEQ",  iExeSeq },
                        { "EMPLOYEE_NO", sEmployeeNo },
                    };

                    // 합의자 리스트
                    DbProcedureRequest request6 = new DbProcedureRequest();
                    request6.ProcedureName = "SP_WEB_FUNCTION_GET_DISCUSSION_USER_LIST";
                    request6.Division = "discussion";
                    request6.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", sEaExeId }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request1, request2, request3, request4, request5, request6 });

                    if (result.Result)
                    {

                        var markAsReadList = new List<(int exeSeq, int answerSeq)>();

                        var commentList = result?.ResultList?.FirstOrDefault(r => r.Division == "review")?.DataTable;
                        if (commentList != null)
                        {
                            foreach (DataRow row in commentList.Rows)
                            {
                                string answerEmp = row["EMPLOYEE_NO"].ToString() ?? string.Empty;

                                if (answerEmp != sEmployeeNo)
                                {
                                    int answerSeq = Convert.ToInt32(row["ANSWER_SEQ"]);
                                    int exeSeq = Convert.ToInt32(row["EXE_SEQ"]);
                                    markAsReadList.Add((exeSeq, answerSeq));
                                    //await MarkAsReadAsync(helper, sEaExeId, sEabusNo, iGbnCode, exeSeq, answerSeq, sEmployeeNo);
                                }
                                else if (answerEmp == sEmployeeNo)
                                {
                                    row["ME_VIEW_FLAG"] = "1";
                                }
                            }
                        }

                        var jsonResponse = JsonHelper.ConvertDbResultToJsonObject(result!);

                        foreach (var (exeSeq, answerSeq) in markAsReadList)
                        {
                            await MarkAsReadAsync(helper, sEaExeId, sEabusNo, iGbnCode, exeSeq, answerSeq, sEmployeeNo);
                        }
                        _logger.LogDebug("완료");
                        return jsonResponse;
                    }
                    else
                    {
                        throw new Exception("결재 문서 상세 정보 조회 중 오류 발생");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "결재 문서 상세 정보 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 작성 (댓글 등록)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> PostAnswer(ApprovalAnswerRequest request)
        {
            try
            {
                string sEabusNo = request.EabusNo ?? string.Empty;
                string sEaExeId = request.EaExeId ?? string.Empty;
                int iGbnCode = request.GbnCode ?? 0;
                int iExeSeq = request.ExeSeq ?? 0;

                string sAnswerGbnName = request.AnswerGbnName ?? "작성"; // 승인, 작성, 승인 반송 

                string sAnswerTime = request.AnswerTime ?? string.Empty;
                string sAnswerCnt = request.AnswerCnt ?? string.Empty;
                string sOpmanCode = request.OpmanCode ?? string.Empty;
                string sOptime = string.Empty;
                string sMainViewFlag = request.MainViewFlag ?? "0";
                string sIud = "I";

                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    int iAnswerSeq = await GetAnswerSeq(helper, sEaExeId, sEabusNo, iGbnCode, iExeSeq);
                    DbProcedureRequest procedureRequest = new DbProcedureRequest
                    {

                        ProcedureName = "SP_WEB_FrmEA101_Window_04_IUD",
                        Division = "answer",
                        Parameters = new Dictionary<string, object>
                        {
                            { "EABUS_NO", sEabusNo },                // 업무 번호
                            { "EA_EXE_ID", sEaExeId },               // 전자문서 실행 ID
                            { "GBN_CODE", iGbnCode },                // 구분 코드
                            { "EXE_SEQ", iExeSeq },                  // 실행 순번
                            { "ANSWER_SEQ", iAnswerSeq },            // 답변 순번
                            { "ANSWER_GBN_NAME", sAnswerGbnName },   // 답변 구분 이름
                            { "ANSWER_TIME", sAnswerTime },          // 답변 시간
                            { "ANSWER_CNT", sAnswerCnt },            // 답변 내용
                            { "OPMAN_CODE", sOpmanCode },            // 작업자 코드
                            { "OPTIME", sOptime },                   // 작업 시간
                            { "MAIN_VIEW_FLAG", sMainViewFlag },     // 메인 뷰 플래그
                            { "IUD", sIud }                          // 작업 구분 (I/U/D)
                        }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { procedureRequest });

                    if (!result.Result)
                    {
                        _logger.LogError($"프로시저 호출 실패: {result.ErrorMessage}");
                        throw new Exception("댓글 등록 실패");
                    }

                    // 댓글 작성 성공 후 본문 내용 최신화 (자동 호출)
                    _logger.LogDebug("댓글 작성 완료. 최신 본문 내용 조회 시작.");
                    var detailResult = await GetApprovalDetailList(new ApprovalDetailRequest
                    {
                        EaExeId = request.EaExeId,
                        EabusNo = request.EabusNo,
                        EmployeeNo = request.OpmanCode,
                        GbnCode = request.GbnCode,
                        ExeSeq = request.ExeSeq
                    });

                    return detailResult;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"오류 발생: {ex.Message}");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 순번 반환
        /// </summary>
        private async Task<int> GetAnswerSeq(SQLServerHelper helper, string EaExeId, string EabusNo, int? GbnCode, int? ExeSeq)
        {
            string sEaExeId = EaExeId ?? string.Empty;
            string sEabusNo = EabusNo ?? string.Empty;
            int iGbnCode = GbnCode ?? 0;
            int iExeSeq = ExeSeq ?? 0;

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_STORE_MAX_01",
                Division = "",
                Parameters = new Dictionary<string, object>
                {
                    { "EA_EXE_ID", sEaExeId },
                    { "EABUS_NO", sEabusNo },
                    { "GBN_CODE", iGbnCode },
                    { "EXE_SEQ",  iExeSeq }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            if (result.Result && result.ResultList != null && result.ResultList.Count > 0)
            {
                var dataTable = result.ResultList[0].DataTable;
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    return Convert.ToInt32(dataTable.Rows[0][0]);
                }
            }

            return 1; // 기본값 반환
        }

        /// <summary>
        /// 페이지 진입 시 읽음 처리
        /// </summary>
        private async Task<bool> MarkAsReadAsync(SQLServerHelper helper, string EaExeId, string EabusNo, int? GbnCode, int? ExeSeq, int? AnswerSeq, string UserId)
        {
            string sEaExeId = EaExeId ?? string.Empty;
            string sEabusNo = EabusNo ?? string.Empty;
            int iGbnCode = GbnCode ?? 0;
            int iExeSeq = ExeSeq ?? 0;
            int iAnswerSeq = AnswerSeq ?? 0;
            string sUserId = UserId ?? string.Empty;

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_FrmEA101_Window_08_LIST",
                Division = "",
                Parameters = new Dictionary<string, object>
                {
                    { "EA_EXE_ID", sEaExeId },
                    { "GBN_CODE", iGbnCode },
                    { "EABUS_NO", sEabusNo },
                    { "EXE_SEQ", iExeSeq },
                    { "ANSWER_SEQ", iAnswerSeq },
                    { "USERID", sUserId }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            return result.Result; // 프로시저 실행 결과 반환
        }


    }

}

```

### IApprovalService.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval
{
    /// <summary>
    /// 결재문서 서비스 인터페이스
    /// </summary>
    public interface IApprovalService
    {
        /// <summary>
        /// 미결함, 보관함, 기안함, 결재함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalAllList(ApprovalRequest request);

        /// <summary>
        /// 본문 상세 조회 인터페이스
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalDetailList(ApprovalDetailRequest request);

        /// <summary>
        /// 문서 열람시간 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> CheckOpenTime(ApprovalDetailRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> ConfirmApproval(SubmitApprovalRequest request);

        /// <summary>
        /// 댓글 작성
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> PostAnswer(ApprovalAnswerRequest request);

    }
}

```

### BoardDetailRequest.cs

```using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시물 본문 요청 모델
    /// </summary>
    public class BoardDetailRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// 게시물 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;

        /// <summary>
        /// 로그인 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty; 

    }
}

```

### BoardIssueRequest.cs

```namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시판 Isuue Title 요청 모델
    /// </summary>
    public class BoardIssueRequest
    {
        ///// <summary>
        ///// 게시판 이름
        ///// </summary>
        //public string title { get; set; } = string.Empty;

        ///// <summary>
        ///// 게시판 구분
        ///// </summary>
        //public string statusGbn { get; set; } = string.Empty;

        ///// <summary>
        ///// 필요 없음
        ///// </summary>
        //public string baseName { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;
    }
}

```

### BoardlAnswerRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models
{
    /// <summary>
    /// 댓글 작성 요청 모델
    /// </summary>
    public class BoardlAnswerRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// Comment 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;

        /// <summary>
        /// Reply 내용
        /// </summary>
        public string ReplyComment { get; set; } = string.Empty;

        /// <summary>
        /// 담당자 코드
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 작업 유형 (IUD)
        /// </summary>
        public string IUD { get; set; } = string.Empty;
    }
}

```

### BoardListRequest.cs

```using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board.Models
{
    /// <summary>
    /// 게시물 List 요청 모델
    /// </summary>
    public class BoardListRequest
    {
        /// <summary>
        /// Issue 번호
        /// </summary>
        public string IssueNo { get; set; } = string.Empty;

        /// <summary>
        /// 시작 날짜
        /// </summary>
        public string SDate { get; set; } = string.Empty;
        /// <summary>
        /// 종료 날짜
        /// </summary>
        public string EDate { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        /// <summary>
        /// 게시물 번호
        /// </summary>
        public string CommentNo { get; set; } = string.Empty;
        // 추후 사용 예정
        //public string title { get; set; } = string.Empty;
        //public string baseName { get; set; } = string.Empty;

    }
}

```

### BoardController.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board;

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
public class BoardController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly IBoardService _boardService;

    /// <summary>
    /// 캘린더 컨트롤러 생성자
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="boardService"></param>
    public BoardController(ILogger<ApprovalController> logger, IEnvService envService, IBoardService boardService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _boardService = boardService;

    }

    /// <summary>
    /// 게시판 이슈(타이틀) 리스트 조회 ex) 공지사항, 행사 등
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetIssuesBoardList")]
    public async Task<IActionResult> GetBoardIssueList([FromBody] BoardIssueRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardIssueList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 이슈별 게시물 리스트 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "sDate": "20240907",
    ///       "eDate": "20250207",
    ///       "employeeNo": "19039",
    ///       "commentNo": ""
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetOneIssueBoardList")]
    public async Task<IActionResult> GetBoardList([FromBody] BoardListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardList(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 게시물 본문 조회
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "commentNo": "202501150001",
    ///       "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetBoardDetail")]
    public async Task<IActionResult> GetBoardDetail([FromBody] BoardDetailRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.GetBoardDetail(request);

            _logger.LogDebug("성공");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return StatusCode(500, new { Message = "서버 오류 발생", Error = ex.Message });
        }
    }

    /// <summary>
    /// 게시물 댓글 작성 / iud (등록: "I", 삭제: "", 수정 없음) / 댓글 등록 성공 시 본문 내용 반환
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///       "issueNo": "202402130001",
    ///       "commentNo": "202501150001",
    ///       "replyComment": "댓글내용",
    ///       "employeeNo": "19039",
    ///       "iud": "I"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("PostAnswer")]
    public async Task<IActionResult> PostAnswer([FromBody] BoardlAnswerRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _boardService.PostAnswer(request);

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

```

### BoardService.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스
    /// </summary>
    public class BoardService : IBoardService
    {
        private readonly IEnvService _envService;
        private readonly string _connectionString;
        private readonly ILogger<BoardService> _logger;

        /// <summary>
        /// 캘린더 서비스 생성자
        /// </summary>
        /// <param name="envService"></param>
        /// <param name="logger"></param>
        public BoardService(IEnvService envService, ILogger<BoardService> logger)
        {
            _envService = envService;
            _connectionString = _envService.GetConnectionString();
            _logger = logger;
        }

        /// <summary>
        /// 게시물 본문 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetBoardDetail(BoardDetailRequest request)
        {
            string sIssueNo = request.IssueNo;
            string sCommentNo = request.CommentNo;
            string sEmployeeNo = request.EmployeeNo;
            string sConfirmFlag = "0";
            string sSmsFlag = "1";
            string sIud = "IU";

            string sTitle = string.Empty;
            string sBaseName = string.Empty;
            string sDate1 = string.Empty;
            string sDate2 = string.Empty;

            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    // 본문 내용
                    DbProcedureRequest boardDetail = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_LIST",
                        Division = "detail",
                        Parameters = new Dictionary<string, object>
                        {
                            { "ISSUE_NO", sIssueNo },
                            { "EMPLOYEE_NO", sEmployeeNo },
                            { "TITLE", sTitle },
                            { "BASE_NAME", sBaseName },
                            { "DATE1", sDate1 },
                            { "DATE2", sDate2 },
                            { "COMMENT_NO", sCommentNo }
                        }
                    };

                    // 댓글 리스트
                    DbProcedureRequest reviewRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_02_LIST",
                        Division = "review",
                        Parameters = new Dictionary<string, object>
                        {
                            { "@ISSUE_NO", sIssueNo },
                            { "@COMMENT_NO", sCommentNo }
                        }
                    };

                    // 첨부파일
                    DbProcedureRequest fileRequest = new DbProcedureRequest
                    {
                        ProcedureName = "SP_WEB_FrmBP602_01_01_LIST",
                        Division = "file",
                        Parameters = new Dictionary<string, object>
                        {
                            { "@ISSUE_NO", sIssueNo },
                            { "@COMMENT_NO", sCommentNo }
                        }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { boardDetail, reviewRequest, fileRequest });

                    bool markAsReadResult = false;
                    if (result.Result)
                    {
                        markAsReadResult = await MarkAsReadBoard(helper, sIssueNo, sCommentNo, sEmployeeNo, sConfirmFlag, sSmsFlag, sIud);

                        if (markAsReadResult)
                        {
                            _logger.LogDebug("완료");
                            return JsonHelper.ConvertDbResultToJsonObject(result);
                        }
                        else
                        {
                            throw new Exception("조회 기록 저장 실패");
                        }
                    }
                    else
                    {
                        throw new Exception("게시물 조회 실패");
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "게시판 상세 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }



        /// <summary>
        /// 게시판 이슈(타이틀) 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<object> GetBoardIssueList(BoardIssueRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sTitle = string.Empty;
                    string sStatusGbn = string.Empty;
                    string sBaseName = string.Empty;
                    string sEmployeeNo = request.EmployeeNo ?? string.Empty;

                    // 개인
                    DbProcedureRequest issueRequest = new DbProcedureRequest();
                    issueRequest.ProcedureName = "SP_WEB_FrmBP601_01_LIST";
                    issueRequest.Division = "issue";
                    issueRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "@TITLE", sTitle },
                        { "@STATUS_GBN", sStatusGbn },
                        { "@BASE_NAME", sBaseName },
                        { "@EMPLOYEE_NO", sEmployeeNo }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { issueRequest });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        DataTable? dt = result.ResultList?.FirstOrDefault()?.DataTable;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            dt.Columns.Remove("OPTIME");
                            dt.Columns.Remove("REG_MAN_NAME");
                            dt.Columns.Remove("STATUS_FLAG");
                            dt.Columns.Remove("STATUS_NAME");
                            dt.Columns.Remove("MANAGER_CODE1");
                            dt.Columns.Remove("MANAGER_CODE2");
                            dt.Columns.Remove("MANAGER_CODE3");
                            dt.Columns.Remove("MANAGER_NAME2");
                            dt.Columns.Remove("MANAGER_NAME3");
                            dt.Columns.Remove("BIGO");
                            dt.Columns.Remove("EMPLOYEE_NO");
                            dt.Columns.Remove("LAST_OPTIME");
                            dt.Columns.Remove("BOARD_TYPE");
                            dt.Columns.Remove("BOARD_TYPE_NAME");

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
                _logger.LogError(ex, "게시판 타이틀(이슈) 데이터 조회 중 오류 발생");
                throw new Exception("실패");
            }

        }

        /// <summary>
        /// 게시물 리스트 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> GetBoardList(BoardListRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sIssueNo = request.IssueNo ?? string.Empty;
                    string sEmployeeNo = request.EmployeeNo ?? string.Empty;
                    string sTitle = string.Empty;
                    string sBaseName = string.Empty;
                    string sDate1 = request.SDate ?? string.Empty;
                    string sDate2 = request.EDate ?? string.Empty;
                    string sCommentNo = request.CommentNo ?? string.Empty;

                    DbProcedureRequest boardListRequest = new DbProcedureRequest();
                    boardListRequest.ProcedureName = "SP_WEB_FrmBP602_01_LIST";
                    boardListRequest.Division = "boardList";
                    boardListRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "ISSUE_NO", sIssueNo },
                        { "EMPLOYEE_NO", sEmployeeNo },
                        { "TITLE", sTitle },
                        { "BASE_NAME", sBaseName },
                        { "DATE1", sDate1 },
                        { "DATE2", sDate2 },
                        { "COMMENT_NO", sCommentNo }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { boardListRequest });

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
                _logger.LogError(ex, "게시물 리스트 조회 중 오류 발생");
                throw new Exception("실패");
            }
        }

        /// <summary>
        /// 댓글 작성, 댓글 등록
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> PostAnswer(BoardlAnswerRequest request)
        {
            try
            {
                using (var helper = new SQLServerHelper(_connectionString, _logger))
                {
                    string sIssueNo = request.IssueNo ?? string.Empty;
                    string sCommentNo = request.CommentNo ?? string.Empty;
                    string sReplyNo = string.Empty;
                    string sReplyComment = request.ReplyComment ?? string.Empty;
                    string sOpmanCode = request.EmployeeNo ?? string.Empty;
                    string sIUD = request.IUD.ToUpper() ?? "I";

                    DbProcedureRequest getReplyNo = new DbProcedureRequest();
                    getReplyNo.ProcedureName = "SP_WEB_STORE_MAX_10";
                    getReplyNo.Parameters = new Dictionary<string, object>();

                    var replyNoResult = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { getReplyNo });

                    if (replyNoResult.Result)
                    {
                        sReplyNo = replyNoResult.ResultList?.FirstOrDefault()?.DataTable?.Rows[0]["REPLY_NO"].ToString() ?? string.Empty;
                    }
                    else
                    {
                        throw new Exception("댓글 번호 조회 실패");
                    }

                    DbProcedureRequest commentRequest = new DbProcedureRequest();
                    commentRequest.ProcedureName = "SP_WEB_FrmBP602_01_02_IUD";
                    commentRequest.Division = "processBoardComment";
                    commentRequest.Parameters = new Dictionary<string, object>()
                    {
                        { "ISSUE_NO", sIssueNo },
                        { "COMMENT_NO", sCommentNo },
                        { "REPLY_NO", sReplyNo },
                        { "REPLY_COMMENT", sReplyComment },
                        { "OPMAN_CODE", sOpmanCode },
                        { "IUD", sIUD }
                    };

                    var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { commentRequest });

                    if (result.Result)
                    {
                        _logger.LogDebug("완료");
                        var detailResult = await GetBoardDetail(new BoardDetailRequest
                        {
                            IssueNo = sIssueNo,
                            CommentNo = sCommentNo,
                            EmployeeNo = sOpmanCode
                        });

                        return detailResult;
                    }
                    else
                    {
                        throw new Exception("실패");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "댓글 처리 중 오류 발생");
                throw new Exception("실패");
            }
        }

        private async Task<bool> MarkAsReadBoard(SQLServerHelper helper, string issueNo, string commentNo, string employeeNo, string ConfirmFlag, string SmsFlag, string Iud)
        {
            string sIssueNo = issueNo ?? string.Empty;
            string sCommentNo = commentNo ?? string.Empty;
            string sOpManCode = employeeNo ?? string.Empty;
            string sConfirmFlag = ConfirmFlag ?? "0";
            string sSmsFlag = SmsFlag ?? "1";
            string sIud = Iud ?? "IU";

            DbProcedureRequest request = new DbProcedureRequest
            {
                ProcedureName = "SP_WEB_FrmBP602_01_03_IUD",
                Division = "readBoard",
                Parameters = new Dictionary<string, object>
                {
                    { "ISSUE_NO", sIssueNo },
                    { "COMMENT_NO", sCommentNo },
                    { "OPMAN_CODE", sOpManCode },
                    { "CONFIRM_FLAG", sConfirmFlag },
                    { "SMS_FLAG", sSmsFlag },
                    { "IUD", sIud }
                }
            };

            var result = await helper.CallProcedureAsync(new List<DbProcedureRequest> { request });

            return result.Result;
        }





    }
}

```

### IBoardService.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Board.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface IBoardService
    {
        /// <summary>
        /// 캘린더 전체 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardIssueList(BoardIssueRequest request);

        /// <summary>
        /// 이슈(타이틀)별 게시글 리스트 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardList(BoardListRequest request);

        /// <summary>
        /// 게시물 본문 조회
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetBoardDetail(BoardDetailRequest request);

        /// <summary>
        /// 게시물 댓글 작성
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> PostAnswer(BoardlAnswerRequest request);


    }
}

```

### CalendarAllListRequest.cs

```namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models
{
    /// <summary>
    /// 캘린더 전체 리스트 요청
    /// </summary>
    public class CalendarAllListRequest
    {
        /// <summary>
        /// 시작 날짜
        /// </summary>
        public string SDate { get; set; } = string.Empty;

        /// <summary>
        /// 종료 날짜
        /// </summary>
        public string EDate { get; set; } = string.Empty;

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        ///// <summary>
        ///// 조회자 ID
        ///// </summary>
        //public string ViewEmployeeNo { get; set; } = string.Empty;
    }
}

```

### CalendarController.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval.Models;
using F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar;

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
public class CalendarController : Controller
{
    private readonly ILogger<ApprovalController> _logger;
    private readonly string _connectionString;
    private readonly IEnvService _envService;
    private readonly ICalendarService _calendarService;

    /// <summary>
    /// 캘린더 컨트롤러 생성자
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="envService"></param>
    /// <param name="calendarService"></param>
    public CalendarController(ILogger<ApprovalController> logger, IEnvService envService, ICalendarService calendarService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _calendarService = calendarService;

    }

    /// <summary>
    /// 캘린더 전체 리스트
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     {
    ///      "sDate": "20250101",
    ///      "eDate": "20250206",
    ///      "employeeNo": "19039"
    ///     }
    ///     
    /// </remarks>
    [HttpPost]
    [Route("GetCalendarAllList")]
    public async Task<IActionResult> GetCalendarAllList([FromBody] CalendarAllListRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { Message = "유효하지 않은 요청입니다." });
            }

            var result = await _calendarService.GetCalendarAllList(request);

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

```

### CalendarService.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### ICalendarService.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Calendar.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Calendar
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface ICalendarService
    {
        /// <summary>
        /// 캘린더 전체 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetCalendarAllList(CalendarAllListRequest request);
    }
}

```

### FileController.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using F1Soft.Starmap.Service.Helper;
using F1Soft.Starmap.Service.Services.EnvService;
using F1Soft.Starmap.Service.Services.FtpService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.IO.Compression;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Common;

/// <summary>
/// 기본 컨트롤러
/// </summary>
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class FileController : Controller
{
    private readonly ILogger<FileController> _logger;
    private readonly string _connectionString;
    private readonly IFtpService _ftpService;
    private readonly IEnvService _envService;


   /// <summary>
   /// 
   /// </summary>
   /// <param name="envService"></param>
   /// <param name="logger"></param>
   /// <param name="ftpService"></param>
    public FileController(IEnvService envService, ILogger<FileController> logger, IFtpService ftpService)
    {
        _envService = envService;
        _connectionString = _envService.GetConnectionString();
        _logger = logger;
        _ftpService = ftpService;
    }

    /// <summary>
    /// Ftp에서 첨부파일 받아오기
    /// </summary>
    /// <param name="FileCode">DO2024101500007</param>
    /// <param name="FileName">라인테크시스템_241015_동방푸드마스타_AutoCAD LT 및 Adobe CCT 재 계약.pdf</param>
    /// <param name="GubunCode">01</param>
    /// <param name="FileExt">pdf</param>
    /// <returns></returns>
    [HttpGet]
    [Route("DownloadFile")]
    public IActionResult DownloadFile(string FileCode, string FileName, string GubunCode, string FileExt)
    {
        try
        {
            string fname = FileName;
            string locationGubun = GubunCode ?? "01";
            string fileExt = string.IsNullOrEmpty(FileExt) ? "" : "." + FileExt;
            string filePath = "";
            string year = FileCode.Substring(2, 4);
            string month = FileCode.Substring(6, 2);

            switch (locationGubun)
            {
                case "01": // Docu 폴더
                    filePath = "wf_ftp/Docu/";
                    break;
                case "02": // Library 폴더
                    filePath = "wf_ftp/Library/";
                    break;
                case "03": // SaleDocu 폴더
                    filePath = "wf_ftp/SaleDocu/";
                    break;
                case "04": // BusiProcess 폴더
                    filePath = "wf_ftp/BusiProcess/";
                    break;
                case "05": // 제품,자재 폴더
                    filePath = "wf_ftp/Itempicture/";
                    break;
                case "06":
                    filePath = "wf_ftp/RND/";
                    break;
            }
            filePath += year + month + "/";

            var ftpStream = _ftpService.GetStream(filePath + FileCode);

            // 메모리 스트림에 zlib 압축 해제
            using var memoryStream = new MemoryStream();
            using (var decompressedStream = new ZLibStream(ftpStream, CompressionMode.Decompress))
            {
                decompressedStream.CopyTo(memoryStream);
            }

            // 압축 해제된 데이터를 바이트 배열로 변환
            memoryStream.Position = 0;
            var fileBytes = memoryStream.ToArray();

            // 파일 다운로드
            return File(fileBytes, "application/octet-stream", FileName);
        }
        catch (Exception e)
        {
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("파일 다운로드 실패 : " + e);
            Console.WriteLine("--------------------------------------------------------------------------------");
            return StatusCode(500); // Internal Server Error
        }
    }




    /// <summary>
    /// EA_EXE_ID를 이용해 첨부파일을 다운받습니다.
    /// </summary>
    /// <param name="EaExeID">202410140039</param>
    /// <param name="EabusNo">001</param>
    /// <param name="EmpCode">19039</param>
    /// <returns></returns>
    [HttpGet]
    [Route("DownloadFileByEaExeID")]
    public async Task<IActionResult> DownloadFileByEaExeID(string EaExeID, string EabusNo, string EmpCode)
    {
        try
        {
            using (var helper = new SQLServerHelper(_connectionString, _logger))
            {
                DbProcedureRequest request = new DbProcedureRequest();
                request.ProcedureName = "SP_WEB_FrmEA101_07_LIST";
                request.Parameters = new Dictionary<string, object>()
                    {
                        { "EA_EXE_ID", EaExeID!},
                        { "EABUS_NO", EabusNo! },
                        { "EMP_CODE", EmpCode! },
                    };


                var result = await helper.CallProcedureAsync(new List<DbProcedureRequest>() { request });

                if (result.Result)
                {
                    if (result.ResultList![0].DataTable!.Rows.Count >= 1)
                    {
                        DataRow row = result.ResultList![0].DataTable!.Rows[0];
                        string FileCode = row.Field<string>("DOCU_CODE")!;
                        string FileName = row.Field<string>("DOCU_NAME")!;
                        string GubunCode = "01";
                        string FileExt = row.Field<string>("FILE_EXT")!;
                        return DownloadFile(FileCode, FileName, GubunCode, FileExt);
                    }
                    else
                        return BadRequest();

                }
            }

            return BadRequest();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "실패");
            return BadRequest();
        }


    }

}
```

### EmpListRequest.cs

```using static System.Runtime.InteropServices.JavaScript.JSType;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Emp.Models
{
    /// <summary>
    /// 사원 List 요청 모델
    /// </summary>
    public class EmpListRequest
    {
        ///// <summary>
        ///// 공장 코드
        ///// </summary>
        //public string FactoryCode { get; set; } = string.Empty;

        ///// <summary>
        ///// 플랜트 코드
        ///// </summary>
        //public string PlantCode { get; set; } = string.Empty;

        /// <summary>
        /// 전체 검색 키워드
        /// </summary>
        public string TotalSearch { get; set; } = string.Empty;

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string EmployeeNo { get; set; } = string.Empty;

        ///// <summary>
        ///// 체크 옵션 1
        ///// </summary>
        //public string Check01 { get; set; } = string.Empty;

        ///// <summary>
        ///// 체크 옵션 2
        ///// </summary>
        //public string Check02 { get; set; } = string.Empty;

        ///// <summary>
        ///// 입사일 시작
        ///// </summary>
        //public string JoinDateFrom { get; set; } = string.Empty;

        ///// <summary>
        ///// 입사일 종료
        ///// </summary>
        //public string JoinDateTo { get; set; } = string.Empty;

        ///// <summary>
        ///// 퇴사일 시작
        ///// </summary>
        //public string RetireDateFrom { get; set; } = string.Empty;

        ///// <summary>
        ///// 퇴사일 종료
        ///// </summary>
        //public string RetireDateTo { get; set; } = string.Empty;

        ///// <summary>
        ///// 기준 날짜
        ///// </summary>
        //public string StandardDate { get; set; } = string.Empty;

    }
}

```

### EmpController.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
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

```

### EmpService.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
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

```

### IEmpService.cs

```using F1Soft.Starmap.Service.Controllers.Groupware.Emp.Models;

namespace F1Soft.Starmap.Service.Controllers.Groupware.Board
{
    /// <summary>
    /// 캘린더 서비스 인터페이스
    /// </summary>
    public interface IEmpService
    {

        /// <summary>
        /// 사원 정보 리스트
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetEmpList(EmpListRequest request);

    }
}

```

### SampleController.cs

```//using F1Soft.Starmap.Service.Controllers.Database.Models;
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
```

### SapRFC.cs

```using SapNwRfc;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace F1Soft.Starmap.Service.Controllers.Database.Models
{
    /// <summary>
    /// Database Procedure 요청
    /// </summary>
    public class SapRFCRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public string? RFCName { get; set; }

        /// <summary>
        /// 파라미터 리스트
        /// </summary>
        public Dictionary<string, object>? Parameters { get; set; }
    }


    #region P040
    /// <summary>
    /// SapRFC_P040
    /// </summary>
    public class SapRFC_P040
    {
        /// <summary>
        /// I_IFID
        /// </summary>
        [SapName("I_IFID")]
        public string? I_IFID { get; set; }

        /// <summary>
        /// IT_DATA
        /// </summary>
        [SapName("IT_DATA")]
        public SapRFC_P040Item[]? IT_DATA { get; set; }
    }

    /// <summary>
    /// SapRFC_P040Item
    /// </summary>
    public class SapRFC_P040Item
    {
        /// <summary>
        /// CSYST
        /// </summary>
        [SapName("CSYST")]
        public string? CSYST { get; set; }

        /// <summary>
        /// ZMESIFNO
        /// </summary>
        [SapName("ZMESIFNO")]
        public string? ZMESIFNO { get; set; }
        
    }
    #endregion


    /// <summary>
    /// SapRFCResult 결과
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local", Justification = "Used as generic parameter")]
    public class SapRFCResult
    {
        /// <summary>
        /// ES Result
        /// </summary>
        [SapName("ES_RESULT")]
        public SapRFCResultItem? ES_RESULT { get; set; }
    }

    /// <summary>
    /// SapRFCResultItem 결과 항목
    /// </summary>
    public class SapRFCResultItem
    {
        /// <summary>
        /// Return Code
        /// </summary>
        [SapName("RETCD")]
        public string? RETCD { get; set; }

        /// <summary>
        /// Return Message
        /// </summary>
        [SapName("RETMG")]
        public string? RETMG { get; set; }
    }
}

```

### SapRFCController.cs

```//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using SapNwRfc.Pooling;
//using System.Reflection;

//namespace F1Soft.Starmap.Service.Controllers.Interface.Sap;

///// <summary>
///// SAP RFC를 이용한 인터페이스 API
///// </summary>
//[ApiController]
//[Route("api/[controller]")]
//public class SapRFCController : ControllerBase
//{
//    private readonly ISapPooledConnection _connection;
//    private readonly ILogger<SapRFCController> _logger;

//    /// <summary>
//    /// SapRFCController 생성자
//    /// </summary>
//    /// <param name="configuration"></param>
//    /// <param name="logger"></param>
//    /// <param name="connection"></param>
//    public SapRFCController(IConfiguration configuration, ILogger<SapRFCController> logger, ISapPooledConnection connection)
//    {
//        _connection = connection;
//        _logger = logger;
//    }

//    /// <summary>
//    /// SAP RFC 호출 API
//    /// </summary>
//    /// <param name="SapRFCs"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [Route("CallSapRFC")]
//    [EnableCors("AllowSpecificOrigin")]
//    public IActionResult SapRFC([FromBody] List<SapRFCRequest> SapRFCs)
//    {
//        try
//        {
//            string rfcID = SapRFCs[0].RFCName!;
//            SapRFCResult result = new SapRFCResult() { ES_RESULT = new SapRFCResultItem() { RETCD = "X", RETMG = "XX" } };

//            if (rfcID.Equals("P030"))
//            {
//                List<SapRFC_P040Item> IT_DATA_List = new List<SapRFC_P040Item>();
//                foreach (SapRFCRequest sqlProcedure in SapRFCs)
//                {
//                    var row = new SapRFC_P040Item();
//                    foreach (var param in sqlProcedure.Parameters!)
//                    {
//                        // 1. 대상 모델(row)에서 param.Name과 일치하는 속성 정보 가져오기
//                        PropertyInfo propertyInfo = typeof(SapRFC_P040Item).GetProperty(param.Key)!;

//                        if (propertyInfo != null && propertyInfo.CanWrite)
//                        {
//                            if (propertyInfo.PropertyType == typeof(string))
//                            {
//                                propertyInfo.SetValue(row, param.Value.ToString());
//                            }
//                            else if (propertyInfo.PropertyType == typeof(int))
//                            {
//                                int.TryParse(param.Value.ToString(), out int parseValue);
//                                propertyInfo.SetValue(row, parseValue);
//                            }
//                            else if (propertyInfo.PropertyType == typeof(decimal))
//                            {
//                                decimal.TryParse(param.Value.ToString(), out decimal parseValue);
//                                propertyInfo.SetValue(row, parseValue);
//                            }
//                        }
//                    }
//                    IT_DATA_List.Add(row);
//                }

//                result = _connection.InvokeFunction<SapRFCResult>("ZDPSF_" + rfcID, new SapRFC_P040
//                {
//                    I_IFID = rfcID,
//                    IT_DATA = IT_DATA_List.ToArray()
//                });
//            }
//            else
//            {
//                _logger.LogError("RFC ID 오류"); // 정보 로그 작성
//                return StatusCode(403, "존재하지 않는 RFC ID");
//            }

//            string resultJson = JsonConvert.SerializeObject(result);
//            return Ok(resultJson);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error 발생"); // 정보 로그 작성
//            return StatusCode(500, ex.ToString());
//        }
//    }
//}

```

### SampleAPIController.cs

```//using Azure;
//using F1Soft.Starmap.Service.Controllers.Users.Models;
//using F1Soft.Starmap.Service.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Annotations;

//namespace F1Soft.Starmap.Service.Controllers.Sample
//{
//    /// <summary>
//    /// ���� API
//    /// </summary>
//    [ApiController]
//    [Route("/api/[controller]")]
//    public class SampleAPIController : ControllerBase
//    {
//        private readonly TokenService _tokenService;

//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<SampleAPIController> _logger;

//        /// <summary>
//        /// SampleAPIController ������
//        /// </summary>
//        /// <param name="logger"></param>
//        /// <param name="tokenService"></param>
//        public SampleAPIController(ILogger<SampleAPIController> logger, TokenService tokenService)
//        {
//            _logger = logger;
//            _tokenService = tokenService;
//        }

//        /// <summary>
//        /// ������ �������� API ����
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("GetSampleAPI")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        public IEnumerable<SampleModel> Get()
//        {
//            return Enumerable.Range(1, 5).Select(index => new SampleModel
//            {
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                TemperatureC = Random.Shared.Next(-20, 55),
//                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//    }
//}

```

### SampleModel.cs

```using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace F1Soft.Starmap.Service.Controllers.Sample
{
    /// <summary>
    /// ���� ��
    /// </summary>
    public class SampleModel
    {
        /// <summary>
        /// ��¥
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// �µ� ����
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// �µ� ȭ��
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// ����
        /// </summary>
        public string? Summary { get; set; }
    }
}

```

### Role.cs

```namespace F1Soft.Starmap.Service.Controllers.Users.Enums
{
    /// <summary>
    /// 사용자 역할, 권한
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// 관리자
        /// </summary>
        Admin,
        /// <summary>
        /// 일반 사용자
        /// </summary>
        User
    }
}

```

### AppUser.cs

```using F1Soft.Starmap.Service.Controllers.Users.Enums;
using Microsoft.AspNetCore.Identity;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 인증용 사용자 모델
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// 권한
        /// </summary>
        public Role Role { get; set; }

    }
}

```

### AuthRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 사용자 인증 요청 모델
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// 사용자 ID
        /// </summary>
        [Required]
        public string? UserID { get; set; }

        /// <summary>
        /// 패스워드 (암호화)
        /// </summary>
        [Required]
        public string? UserPassword { get; set; }
    }
}

```

### AuthResponse.cs

```namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 인증 결과 반환
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// JWT 토큰 (API 요청 시 Header에 포함시켜야함)
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// 사원 번호
        /// </summary>
        public string? EmployeeNo { get; set; }

        /// <summary>
        /// 담당자 이름
        /// </summary>
        public string? ChargeName { get; set; }

        /// <summary>
        /// 공장 코드
        /// </summary>
        public string? FactoryCode { get; set; }

        /// <summary>
        /// 등급 (빈 값으로 초기화됨)
        /// </summary>
        public string? Grade { get; set; }

        /// <summary>
        /// 사용자 ID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 부서 코드
        /// </summary>
        public string? DepartmentCode { get; set; }

        /// <summary>
        /// 플랜트 코드
        /// </summary>
        public string? PlantCode { get; set; }

        /// <summary>
        /// 코드 이름 (전체 이름)
        /// </summary>
        public string? CodeNameFull { get; set; }

        /// <summary>
        /// 플랜트 이름
        /// </summary>
        public string? PlantName { get; set; }

        /// <summary>
        /// 부서 이름
        /// </summary>
        public string? DepartmentName { get; set; }

        /// <summary>
        /// 이미지 경로 (예: 사용자 이미지 또는 ID 사진)
        /// </summary>
        public string? Imabmp2 { get; set; }

        /// <summary>
        /// 서명 이미지 경로
        /// </summary>
        public string? SignImage { get; set; }

        /// <summary>
        /// 기본 서명 여부
        /// </summary>
        public string? DefaultSignFlag { get; set; }

        /// <summary>
        /// 휴대폰 번호
        /// </summary>
        public string? HandphoneNo { get; set; }

        /// <summary>
        /// 메일 주소
        /// </summary>
        public string? AddrMail { get; set; }

        /// <summary>
        /// BTMS 주소
        /// </summary>
        public string? AddrBtms { get; set; }

        /// <summary>
        /// 비밀번호 변경 필요 여부 (1: 필요, 0: 불필요)
        /// </summary>
        public string? ChagepwFlag { get; set; }

        /// <summary>
        /// 이메일
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 이메일 비밀번호
        /// </summary>
        public string? EmailPw { get; set; }

        /// <summary>
        /// 공용 이메일 개수
        /// </summary>
        public int PublicEmailCnt { get; set; }

        /// <summary>
        /// 연차 정보 (총 연차, 사용 연차, 잔여 연차)
        /// </summary>
        public string? AnnualCntText { get; set; }


    }
}

```

### AutoLoginRequest.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// JWT Token 요청 모델 (자동로그인 활용)
    /// </summary>
    public class AutoLoginRequest
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        [Required]
        public string? JwtToken { get; set; }
    }
}

```

### AutoLoginResponse.cs

```using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 자동로그인 결과 반환 DTO
    /// </summary>
    public class AutoLoginResponse
    {
        /// <summary>
        /// 토큰 유효 결과
        /// </summary>
        [Required]
        public bool? Result { get; set; }

        /// <summary>
        /// 당일 발급 토큰 여부
        /// </summary>
        [Required]
        public bool? TodayToken { get; set; }

        /// <summary>
        /// 유저 정보
        /// </summary>
        public AuthResponse? authResponse { get; set; }
        
    }
}

```

### RegistrationRequest.cs

```using F1Soft.Starmap.Service.Controllers.Users.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 사용자 등록 모델 (미사용)
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// 이메일
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required]
        public string? Username { get; set; }


        /// <summary>
        /// 비밀번호
        /// </summary>
        [Required]
        public string? Password { get; set; }
        
        /// <summary>
        /// 사용자 권한, 기본 값 : User
        /// </summary>
        public Role Role { get; set; } = Role.User;
    }
}

```

### IDatabaseHelper.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;

namespace F1Soft.Starmap.Service.Helper
{
    /// <summary>
    /// Database helper interface
    /// </summary>
    public interface IDatabaseHelper : IDisposable
    {
        /// <summary>
        /// 프로시져 호출
        /// </summary>
        /// <param name="dbProcs"></param>
        /// <returns></returns>
        Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs);

        /// <summary>
        /// SQL 커맨드 실행
        /// </summary>
        /// <param name="dbSqls"></param>
        /// <returns></returns>
        Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls);
    }
}

```

### JsonHelper.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using Newtonsoft.Json;

namespace F1Soft.Starmap.Service.Helper
{
    /// <summary>
    /// Json Helper
    /// </summary>
    public static class JsonHelper
    {

        /// <summary>
        /// DbResult 형식을 Json Object 형식으로 변환합니다.
        /// </summary>
        /// <param name="dbResult"></param>
        /// <returns></returns>
        public static object ConvertDbResultToJsonObject(object dbResult)
        {
            string jsonContent = JsonConvert.SerializeObject(dbResult);
            object jsonObject = System.Text.Json.JsonSerializer.Deserialize<object>(jsonContent)!;

            return jsonObject;
        }




    }
}

```

### OracleHelper.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Text;

namespace F1Soft.Starmap.Service.Helper;

/// <summary>
/// 데이터베이스 작업을 위한 헬퍼 클래스
/// </summary>
public class OracleServerHelper : IDatabaseHelper
{
    private readonly OracleConnection _connection;
    private readonly ILogger _logger;

    /// <summary>
    /// DatabaseHelper 생성자
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="logger"></param>
    public OracleServerHelper(string connectionString, ILogger logger)
    {
        _connection = new OracleConnection(connectionString);
        _logger = logger;
        _connection.Open();
    }

    /// <summary>
    /// Stored Procedure 비동기 호출
    /// </summary>
    /// <param name="dbProcs"></param>
    /// <returns></returns>
    public async Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var procInfo in dbProcs)
            {
                var procedureName = procInfo.ProcedureName;
                var parameters = procInfo.Parameters;
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = procedureName;

                var command = new OracleCommand(procedureName, _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = transaction
                };

                if (procInfo.Parameters != null)
                {
                    //숫자, 날짜 파라미터 테스트 필요
                    foreach (var param in procInfo.Parameters)
                    {
                        command.Parameters.Add(param.Key, param.Value.ToString());
                    }
                }

                if (procInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (OracleException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (OracleException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }

    /// <summary>
    /// Sql Command Procedure 비동기 호출
    /// </summary>
    /// <param name="dbSqls"></param>
    /// <returns></returns>
    public async Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var sqlInfo in dbSqls)
            {
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = dbSqls.IndexOf(sqlInfo).ToString();


                var command = sqlInfo.SqlCommand as OracleCommand;
                command!.Connection = _connection;
                command!.CommandType = CommandType.Text;
                command!.Transaction = transaction;

                if (sqlInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (OracleException ex)
                    {
                        errorMessages.AppendLine($"Error in {sqlInfo.SqlCommand}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (OracleException ex)
                    {
                        errorMessages.AppendLine($"Error in: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }


    /// <summary>
    /// 리소스 해제
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }
}

```

### PostgreSQLHelper.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using System.Data;
using System.Text;
using Npgsql;

namespace F1Soft.Starmap.Service.Helper;

/// <summary>
/// 데이터베이스 작업을 위한 헬퍼 클래스
/// </summary>
public class PostgreSQLHelper : IDatabaseHelper
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger _logger;

    /// <summary>
    /// DatabaseHelper 생성자
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="logger"></param>
    public PostgreSQLHelper(string connectionString, ILogger logger)
    {
        _connection = new NpgsqlConnection(connectionString);
        _logger = logger;
        _connection.Open();
    }

    /// <summary>
    /// Stored Procedure 비동기 호출
    /// </summary>
    /// <param name="dbProcs"></param>
    /// <returns></returns>
    public async Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var procInfo in dbProcs)
            {
                var procedureName = procInfo.ProcedureName;
                var parameters = procInfo.Parameters;
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = procedureName;

                var command = new NpgsqlCommand(procedureName, _connection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Transaction = transaction
                };

                if (procInfo.Parameters != null)
                {
                    //숫자, 날짜 파라미터 테스트 필요
                    foreach (var param in procInfo.Parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

                if (procInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }

    /// <summary>
    /// Sql Command Procedure 비동기 호출
    /// </summary>
    /// <param name="dbSqls"></param>
    /// <returns></returns>
    public async Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var sqlInfo in dbSqls)
            {
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = dbSqls.IndexOf(sqlInfo).ToString();


                var command = sqlInfo.SqlCommand as NpgsqlCommand;
                command!.Connection = _connection;
                command!.CommandType = CommandType.Text;
                command!.Transaction = transaction;

                if (sqlInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {sqlInfo.SqlCommand}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                        }
                    }
                    catch (NpgsqlException ex)
                    {
                        errorMessages.AppendLine($"Error in: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }


    /// <summary>
    /// 리소스 해제
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }
}

```

### SQLServerHelper.cs

```using F1Soft.Starmap.Service.Controllers.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace F1Soft.Starmap.Service.Helper;

/// <summary>
/// 데이터베이스 작업을 위한 헬퍼 클래스
/// </summary>
public class SQLServerHelper : IDatabaseHelper
{
    private readonly SqlConnection _connection;
    private readonly ILogger _logger;

    /// <summary>
    /// DatabaseHelper 생성자
    /// </summary>
    /// <param name="connectionString"></param>
    /// <param name="logger"></param>
    public SQLServerHelper(string connectionString, ILogger logger)
    {
        _connection = new SqlConnection(connectionString);
        _logger = logger;
        _connection.Open();
    }



    /// <summary>
    /// Stored Procedure 비동기 호출
    /// </summary>
    /// <param name="dbProcs"></param>
    /// <returns></returns>
    public async Task<DbResult> CallProcedureAsync(List<DbProcedureRequest> dbProcs)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var procInfo in dbProcs)
            {
                var procedureName = procInfo.ProcedureName;
                var procedureDivision = procInfo.Division;
                var parameters = procInfo.Parameters;
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = procedureName;
                sqlDataItem.Division = procedureDivision;

                var command = new SqlCommand(procedureName, _connection, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };

                StringBuilder sqlLog = new StringBuilder();
                sqlLog.Append($"EXEC {procedureName} ");

                if (procInfo.Parameters != null)
                {
                    int count = 0;
                    //숫자, 날짜 파라미터 테스트 필요
                    foreach (var param in procInfo.Parameters)
                    {
                        //command.Parameters.AddWithValue(param.Key, param.Value.ToString());
                        var value = param.Value ?? DBNull.Value;  // NULL 처리

                        // VARCHAR 강제 지정
                        if (param.Value is string)
                        {
                            command.Parameters.Add(param.Key, SqlDbType.VarChar, 200).Value = value;
                        }
                        else
                        {
                            command.Parameters.AddWithValue(param.Key, value);
                        }

                        sqlLog.Append($"{(count > 0 ? ", " : "")}'{param.Value}'");
                        count++;
                    }
                }

                // 프로시저 로그 확인
                _logger.LogInformation($"Executing SQL: {sqlLog.ToString()}");

                if (procInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                            sqlDataItem.ReturnValue = dt.Rows.Count;
                        }
                    }
                    catch (SqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {procInfo.ProcedureName}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }

    /// <summary>
    /// Sql Command Procedure 비동기 호출
    /// </summary>
    /// <param name="dbSqls"></param>
    /// <returns></returns>
    public async Task<DbResult> ExecuteSqlCommandAsync(List<DbSqlRequest> dbSqls)
    {
        using (var transaction = _connection.BeginTransaction())
        {
            DbResult result = new DbResult();
            result.Result = false;
            result.ResultList = new List<DbResultItem>();
            StringBuilder errorMessages = new StringBuilder();

            foreach (var sqlInfo in dbSqls)
            {
                DbResultItem sqlDataItem = new DbResultItem();
                sqlDataItem.DataTable = new DataTable();
                sqlDataItem.Name = dbSqls.IndexOf(sqlInfo).ToString();


                var command = sqlInfo.SqlCommand as SqlCommand;
                command!.Connection = _connection;
                command!.CommandType = CommandType.Text;
                command!.Transaction = transaction;

                if (sqlInfo.IsChangeProcedure)
                {
                    try
                    {
                        sqlDataItem.ReturnValue = await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        errorMessages.AppendLine($"Error in {sqlInfo.SqlCommand}: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                else
                {
                    try
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            DataTable dt = new DataTable();

                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                int suffix = 1;
                                string newColumnName = reader.GetName(i);

                                while (dt.Columns.Contains(newColumnName))
                                {
                                    newColumnName = $"{reader.GetName(i)}_{suffix}";
                                    suffix++;
                                }

                                dt.Columns.Add(new DataColumn(newColumnName, reader.GetFieldType(i)));
                            }

                            while (await reader.ReadAsync())
                            {
                                object[] values = new object[reader.FieldCount];
                                reader.GetValues(values);
                                dt.Rows.Add(values);
                            }
                            sqlDataItem.DataTable = dt;
                            sqlDataItem.ReturnValue = dt.Rows.Count;
                        }
                    }
                    catch (SqlException ex)
                    {
                        errorMessages.AppendLine($"Error in: {ex.Message}");
                        transaction.Rollback();
                    }
                }
                result.ResultList.Add(sqlDataItem);
            }

            if (errorMessages.Length > 0)
            {
                result.ErrorMessage = errorMessages.ToString();
                return result;
            }

            result.Result = true;
            transaction.Commit();
            return result;
        }
    }




    /// <summary>
    /// 리소스 해제
    /// </summary>
    public void Dispose()
    {
        _connection.Dispose();
    }
}

```

### DevEnvService.cs

```using System.Configuration;

namespace F1Soft.Starmap.Service.Services.EnvService;

/// <summary>
/// 개발환경 서비스
/// </summary>
public class DevEnvService : IEnvService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public DevEnvService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// GetEnvironmentName으로 설정된 db 정보 리턴하기
    /// </summary>
    /// <returns></returns>
    public string GetConnectionString()
    {
        return _configuration.GetConnectionString($"MsSql{GetEnvironmentName()}Connection") ?? string.Empty;
    }

    //configuration.GetConnectionString("DongbangFoodFtp")
    /// <summary>
    /// appsettings.json에 설정된 환경 이름 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetEnvironmentName()
    {
        return "Dev";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetFtpUrl()
    {
        return _configuration.GetConnectionString($"Ftp{GetEnvironmentName()}Url")!;
    }

    /// <summary>
    /// 이미지 ftp URL 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetImageUrl()
    {
        string ftpImageUrl = _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}UserImageUrl"] ?? string.Empty;

        return ftpImageUrl;
    }

    /// <summary>
    /// 사원 카드 URL 가져오기
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetEmpCardUrl()
    {
        string empCardUrl = _configuration[$"ConnectionStrings:{GetEnvironmentName()}EmpCardUrl"] ?? string.Empty;

        return empCardUrl;
    }





}


```

### IEnvService.cs

```namespace F1Soft.Starmap.Service.Services.EnvService;
/// <summary>
/// 환경 인터페이스
/// </summary>
public interface IEnvService
{
    /// <summary>
    /// 연결 DB 가져오기
    /// </summary>
    /// <returns></returns>
    string GetConnectionString();
    /// <summary>
    /// FTP URL을 가져오기
    /// </summary>
    /// <returns></returns>
    string GetFtpUrl();

    /// <summary>
    /// 현재 환경 이름 가져오기
    /// </summary>
    /// <returns></returns>
    string GetEnvironmentName();

    /// <summary>
    /// 이미지 ftp URL 가져오기
    /// </summary>
    /// <returns></returns>
    string GetImageUrl();
    /// <summary>
    /// 사원 카드 URL 가져오기
    /// </summary>
    /// <returns></returns>
    string GetEmpCardUrl();
}

```

### PrdEnvService.cs

```namespace F1Soft.Starmap.Service.Services.EnvService;
/// <summary>
/// 운영 환경 서비스
/// </summary>
public class PrdEnvService : IEnvService
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public PrdEnvService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetConnectionString()
    {
        return _configuration.GetConnectionString($"MsSql{GetEnvironmentName()}Connection") ?? string.Empty;


    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetEnvironmentName()
    {
        return "Prd";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetFtpUrl()
    {
        return _configuration.GetConnectionString($"Ftp{GetEnvironmentName()}Url")!;
    }

    /// <summary>
    /// 이미지 ftp URL 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetImageUrl()
    {
        string ftpImageUrl = _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}UserImageUrl"] ?? string.Empty;

        return ftpImageUrl;
    }
    /// <summary>
    /// 사원 카드 URL 가져오기
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetEmpCardUrl()
    {
        string empCardUrl = _configuration[$"ConnectionStrings:{GetEnvironmentName()}EmpCardUrl"] ?? string.Empty;

        return empCardUrl;
    }
}

```

### StgEnvService.cs

```namespace F1Soft.Starmap.Service.Services.EnvService;
/// <summary>
/// 운영 전 환경 서비스
/// </summary>
public class StgEnvService : IEnvService
{
    private readonly IConfiguration _configuration;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="configuration"></param>
    public StgEnvService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetConnectionString()
    {
        return _configuration.GetConnectionString($"MsSql{GetEnvironmentName()}Connection") ?? string.Empty;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetEnvironmentName()
    {
        return "Stg";
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string GetFtpUrl()
    {
        return _configuration.GetConnectionString($"Ftp{GetEnvironmentName()}Url")!;
    }
    /// <summary>
    /// 이미지 ftp URL 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetImageUrl()
    {
        string ftpImageUrl = _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}UserImageUrl"] ?? string.Empty;

        return ftpImageUrl;
    }
    /// <summary>
    /// 사원 카드 URL 가져오기
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public string GetEmpCardUrl()
    {
        string empCardUrl = _configuration[$"ConnectionStrings:{GetEnvironmentName()}EmpCardUrl"] ?? string.Empty;

        return empCardUrl;
    }
}
```

### FtpService.cs

```using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.IO;
using System.Net;

namespace F1Soft.Starmap.Service.Services.FtpService;

/// <summary>
/// Ftp Client Service
/// </summary>
public class FtpService : IFtpService
{
    private readonly string _ftpUrl;
    private readonly IEnvService _envService;


    /// <summary>
    /// Ftp Service
    /// </summary>
    /// <param name="envService"></param>
    public FtpService(IEnvService envService)
    {
        _envService = envService;

        _ftpUrl = _envService.GetFtpUrl();
    }

    // Disable the warning.
#pragma warning disable SYSLIB0014
    /// <summary>
    /// Ftp의 파일을 Stream형태로 가져옵니다.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public Stream GetStream(string filePath)
    {
        Uri ftpUri = new Uri(_ftpUrl + filePath);
        // FTP 서버에서 파일 다운로드
        var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        ftpRequest.Credentials = new NetworkCredential();
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        var ftpStream = ftpResponse.GetResponseStream();


        return ftpStream;
    }


}

```

### IFtpService.cs

```namespace F1Soft.Starmap.Service.Services.FtpService
{
    /// <summary>
    /// 
    /// </summary>
    public interface IFtpService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream GetStream(string filePath);
    }
}

```

### SFtpService.cs

```using F1Soft.Starmap.Service.Services.EnvService;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.IO;
using System.Net;

namespace F1Soft.Starmap.Service.Services.FtpService;

/// <summary>
/// Ftp Client Service
/// </summary>
public class SFtpService : IFtpService
{
    private readonly string _ftpUrl;
    private readonly IEnvService _envService;


    /// <summary>
    /// Ftp Service
    /// </summary>
    /// <param name="envService"></param>
    public SFtpService(IEnvService envService)
    {
        _envService = envService;

        _ftpUrl = _envService.GetFtpUrl();
    }

    // Disable the warning.
#pragma warning disable SYSLIB0014
    /// <summary>
    /// Ftp의 파일을 Stream형태로 가져옵니다.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public Stream GetStream(string filePath)
    {
        Uri ftpUri = new Uri(_ftpUrl + filePath);
        // FTP 서버에서 파일 다운로드
        var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        ftpRequest.Credentials = new NetworkCredential();
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        var ftpStream = ftpResponse.GetResponseStream();


        return ftpStream;
    }


}

```

### TokenService.cs

```namespace F1Soft.Starmap.Service.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using F1Soft.Starmap.Service.Controllers.Users.Models;
using F1Soft.Starmap.Service.Controllers.Users.Enums;
using System.Security.Cryptography;

/// <summary>
/// JWT 토큰 서비스
/// </summary>
public class TokenService
{
    /// <summary>
    /// 유효 기간
    /// </summary>
    private const int ExpirationDay = 90;
    private readonly ILogger<TokenService> _logger;

    /// <summary>
    /// TokenService 생성자
    /// </summary>
    /// <param name="logger"></param>
    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// JWT 토큰 생성
    /// </summary>
    /// <param name="authResponse"></param>
    /// <returns></returns>
    public string CreateToken(AuthResponse authResponse)
    {
        var expiration = DateTime.UtcNow.AddDays(ExpirationDay);
        var token = CreateJwtToken(
            CreateClaims(authResponse),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();

        _logger.LogInformation("JWT Token created");

        return tokenHandler.WriteToken(token);
    }

    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private List<Claim> CreateClaims(AuthResponse user)
    {
        var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

        try
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwtSub!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId!),
                new Claim(ClaimTypes.Name, user.UserId!),
                new Claim(ClaimTypes.Email, user.Email!)
                //new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            return claims;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// appsettings.json에서 토큰 키를 받아와 SHA256으로 암호화 키 생성
    /// </summary>
    /// <returns></returns>
    private SigningCredentials CreateSigningCredentials()
    {
        var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }

    /// <summary>
    /// JWT 토큰 복호화
    /// </summary>
    /// <param name="token">JWT 토큰</param>
    /// <returns></returns>
    public ClaimsPrincipal DecodeToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
            ValidAudience = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // 토큰 복호화 및 유효성 검사
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

        return principal;

    }

    /// <summary>
    /// Claims를 AppUser로 변환
    /// </summary>
    /// <param name="principal"></param>
    /// <returns></returns>
    public AuthResponse GetAppUserFromClaimsPrincipal(ClaimsPrincipal principal)
    {
        var user = new AuthResponse
        {
            UserId = principal.FindFirstValue(ClaimTypes.Name)!,
        };
        return user;
    }

}

```

### Program.cs

```using F1Soft.Starmap.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Reflection;
using F1Soft.Starmap.Service.Controllers.Auth.Models;
using F1Soft.Starmap.Service.Services.EnvService;
using F1Soft.Starmap.Service.Services.FtpService;
using F1Soft.Starmap.Service.Controllers.Groupware.Approval;
using F1Soft.Starmap.Service.Controllers.Groupware.Calendar;
using F1Soft.Starmap.Service.Controllers.Groupware.Board;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string todayDate = DateTime.Now.ToString("yyMMdd"); // ���� ��¥�� YYMMDD �������� ��ȯ
    //options.SwaggerDoc("v1", new OpenApiInfo { Title = $"testStarmap API Service Stg {todayDate}-1", Version = "v1" });

#if DEBUG
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Dev {todayDate}-1", Version = "v1.0.0" });
#elif STAGING
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Stg {todayDate}-1", Version = "v1.0.0" });
#elif RELEASE
    options.SwaggerDoc("v1", new OpenApiInfo { Title = $"Starmap API Service Prd {todayDate}-1", Version = "v1.0.0" });
#endif


    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    // XML �ּ� ������ �����ϵ��� ����
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});


// Register our TokenService dependency
#if DEBUG
builder.Services.AddScoped<IEnvService, DevEnvService>();
#elif STAGING
builder.Services.AddScoped<IEnvService, StgEnvService>();
#elif RELEASE
builder.Services.AddScoped<IEnvService, PrdEnvService>();
#endif

// ������ ���� �߰�
builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddScoped<UserManager, UserManager>();
builder.Services.AddScoped<IFtpService, FtpService>();
builder.Services.AddScoped<IApprovalService, ApprovalService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IBoardService, BoardService>();
builder.Services.AddScoped<IEmpService, EmpService>();




builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection("JwtTokenSettings")["ValidIssuer"],
            ValidAudience = builder.Configuration.GetSection("JwtTokenSettings")["ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtTokenSettings")["SymmetricSecurityKey"]!))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

```

## Project: F1Soft.Starmap.Core

### Project File: F1Soft.Starmap.Core.csproj

```<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <Folder Include="Models\Auth\" />
  </ItemGroup>

</Project>

```

### IApprovalService.cs

```namespace F1Soft.Starmap.Core.Services.DomainService;
{
    /// <summary>
    /// 결재문서 서비스 인터페이스
    /// </summary>
    public interface IApprovalService
    {
        /// <summary>
        /// 미결함, 보관함, 기안함, 결재함
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalAllList(ApprovalRequest request);

        /// <summary>
        /// 본문 상세 조회 인터페이스
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> GetApprovalDetailList(ApprovalDetailRequest request);

        /// <summary>
        /// 문서 열람시간 체크
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> CheckOpenTime(ApprovalDetailRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> ConfirmApproval(SubmitApprovalRequest request);

        /// <summary>
        /// 댓글 작성
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<object> PostAnswer(ApprovalAnswerRequest request);

    }
}

```

### IEnvService.cs

```namespace F1Soft.Starmap.Core.Services.EnvService;
/// <summary>
/// 환경 인터페이스
/// </summary>
public interface IEnvService
{
    /// <summary>
    /// 연결 DB 가져오기
    /// </summary>
    /// <returns></returns>
    string GetConnectionString();
    /// <summary>
    /// FTP URL을 가져오기
    /// </summary>
    /// <returns></returns>
    string GetFtpUrl();

    /// <summary>
    /// 현재 환경 이름 가져오기
    /// </summary>
    /// <returns></returns>
    string GetEnvironmentName();
}

```

### Class1.cs

```namespace F1Soft.Starmap.Core
{
    public class Class1
    {

    }
}

```

## Project: 기타 파일

### JsonConvert.cs

```#region License
// Copyright (c) 2007 James Newton-King
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.IO;
using System.Globalization;
#if HAVE_BIG_INTEGER
using System.Numerics;
#endif
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using System.Xml;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Diagnostics.CodeAnalysis;
#if HAVE_XLINQ
using System.Xml.Linq;
#endif

namespace Newtonsoft.Json
{
    /// <summary>
    /// Provides methods for converting between .NET types and JSON types.
    /// </summary>
    /// <example>
    ///   <code lang="cs" source="..\Src\Newtonsoft.Json.Tests\Documentation\SerializationTests.cs" region="SerializeObject" title="Serializing and Deserializing JSON with JsonConvert" />
    /// </example>
    public static class JsonConvert
    {
        /// <summary>
        /// Gets or sets a function that creates default <see cref="JsonSerializerSettings"/>.
        /// Default settings are automatically used by serialization methods on <see cref="JsonConvert"/>,
        /// and <see cref="JToken.ToObject{T}()"/> and <see cref="JToken.FromObject(object)"/> on <see cref="JToken"/>.
        /// To serialize without using any default settings create a <see cref="JsonSerializer"/> with
        /// <see cref="JsonSerializer.Create()"/>.
        /// </summary>
        public static Func<JsonSerializerSettings>? DefaultSettings { get; set; }

        /// <summary>
        /// Represents JavaScript's boolean value <c>true</c> as a string. This field is read-only.
        /// </summary>
        public static readonly string True = "true";

        /// <summary>
        /// Represents JavaScript's boolean value <c>false</c> as a string. This field is read-only.
        /// </summary>
        public static readonly string False = "false";

        /// <summary>
        /// Represents JavaScript's <c>null</c> as a string. This field is read-only.
        /// </summary>
        public static readonly string Null = "null";

        /// <summary>
        /// Represents JavaScript's <c>undefined</c> as a string. This field is read-only.
        /// </summary>
        public static readonly string Undefined = "undefined";

        /// <summary>
        /// Represents JavaScript's positive infinity as a string. This field is read-only.
        /// </summary>
        public static readonly string PositiveInfinity = "Infinity";

        /// <summary>
        /// Represents JavaScript's negative infinity as a string. This field is read-only.
        /// </summary>
        public static readonly string NegativeInfinity = "-Infinity";

        /// <summary>
        /// Represents JavaScript's <c>NaN</c> as a string. This field is read-only.
        /// </summary>
        public static readonly string NaN = "NaN";

        /// <summary>
        /// Converts the <see cref="DateTime"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="DateTime"/>.</returns>
        public static string ToString(DateTime value)
        {
            return ToString(value, DateFormatHandling.IsoDateFormat, DateTimeZoneHandling.RoundtripKind);
        }

        /// <summary>
        /// Converts the <see cref="DateTime"/> to its JSON string representation using the <see cref="DateFormatHandling"/> specified.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">The format the date will be converted to.</param>
        /// <param name="timeZoneHandling">The time zone handling when the date is converted to a string.</param>
        /// <returns>A JSON string representation of the <see cref="DateTime"/>.</returns>
        public static string ToString(DateTime value, DateFormatHandling format, DateTimeZoneHandling timeZoneHandling)
        {
            DateTime updatedDateTime = DateTimeUtils.EnsureDateTime(value, timeZoneHandling);

            using (StringWriter writer = StringUtils.CreateStringWriter(64))
            {
                writer.Write('"');
                DateTimeUtils.WriteDateTimeString(writer, updatedDateTime, format, null, CultureInfo.InvariantCulture);
                writer.Write('"');
                return writer.ToString();
            }
        }

#if HAVE_DATE_TIME_OFFSET
        /// <summary>
        /// Converts the <see cref="DateTimeOffset"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="DateTimeOffset"/>.</returns>
        public static string ToString(DateTimeOffset value)
        {
            return ToString(value, DateFormatHandling.IsoDateFormat);
        }

        /// <summary>
        /// Converts the <see cref="DateTimeOffset"/> to its JSON string representation using the <see cref="DateFormatHandling"/> specified.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="format">The format the date will be converted to.</param>
        /// <returns>A JSON string representation of the <see cref="DateTimeOffset"/>.</returns>
        public static string ToString(DateTimeOffset value, DateFormatHandling format)
        {
            using (StringWriter writer = StringUtils.CreateStringWriter(64))
            {
                writer.Write('"');
                DateTimeUtils.WriteDateTimeOffsetString(writer, value, format, null, CultureInfo.InvariantCulture);
                writer.Write('"');
                return writer.ToString();
            }
        }
#endif

        /// <summary>
        /// Converts the <see cref="Boolean"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Boolean"/>.</returns>
        public static string ToString(bool value)
        {
            return (value) ? True : False;
        }

        /// <summary>
        /// Converts the <see cref="Char"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Char"/>.</returns>
        public static string ToString(char value)
        {
            return ToString(char.ToString(value));
        }

        /// <summary>
        /// Converts the <see cref="Enum"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Enum"/>.</returns>
        public static string ToString(Enum value)
        {
            return value.ToString("D");
        }

        /// <summary>
        /// Converts the <see cref="Int32"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int32"/>.</returns>
        public static string ToString(int value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="Int16"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int16"/>.</returns>
        public static string ToString(short value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="UInt16"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt16"/>.</returns>
        [CLSCompliant(false)]
        public static string ToString(ushort value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="UInt32"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt32"/>.</returns>
        [CLSCompliant(false)]
        public static string ToString(uint value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="Int64"/>  to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Int64"/>.</returns>
        public static string ToString(long value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

#if HAVE_BIG_INTEGER
        private static string ToStringInternal(BigInteger value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }
#endif

        /// <summary>
        /// Converts the <see cref="UInt64"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="UInt64"/>.</returns>
        [CLSCompliant(false)]
        public static string ToString(ulong value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="Single"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Single"/>.</returns>
        public static string ToString(float value)
        {
            return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        internal static string ToString(float value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
        {
            return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
        }

        private static string EnsureFloatFormat(double value, string text, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
        {
            if (floatFormatHandling == FloatFormatHandling.Symbol || !(double.IsInfinity(value) || double.IsNaN(value)))
            {
                return text;
            }

            if (floatFormatHandling == FloatFormatHandling.DefaultValue)
            {
                return (!nullable) ? "0.0" : Null;
            }

            return quoteChar + text + quoteChar;
        }

        /// <summary>
        /// Converts the <see cref="Double"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Double"/>.</returns>
        public static string ToString(double value)
        {
            return EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture));
        }

        internal static string ToString(double value, FloatFormatHandling floatFormatHandling, char quoteChar, bool nullable)
        {
            return EnsureFloatFormat(value, EnsureDecimalPlace(value, value.ToString("R", CultureInfo.InvariantCulture)), floatFormatHandling, quoteChar, nullable);
        }

        private static string EnsureDecimalPlace(double value, string text)
        {
            if (double.IsNaN(value) || double.IsInfinity(value) || StringUtils.IndexOf(text, '.') != -1 || StringUtils.IndexOf(text, 'E') != -1 || StringUtils.IndexOf(text, 'e') != -1)
            {
                return text;
            }

            return text + ".0";
        }

        private static string EnsureDecimalPlace(string text)
        {
            if (StringUtils.IndexOf(text, '.') != -1)
            {
                return text;
            }

            return text + ".0";
        }

        /// <summary>
        /// Converts the <see cref="Byte"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Byte"/>.</returns>
        public static string ToString(byte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="SByte"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="SByte"/>.</returns>
        [CLSCompliant(false)]
        public static string ToString(sbyte value)
        {
            return value.ToString(null, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts the <see cref="Decimal"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Decimal"/>.</returns>
        public static string ToString(decimal value)
        {
            return EnsureDecimalPlace(value.ToString(null, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Converts the <see cref="Guid"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Guid"/>.</returns>
        public static string ToString(Guid value)
        {
            return ToString(value, '"');
        }

        internal static string ToString(Guid value, char quoteChar)
        {
            string text;
            string qc;
#if HAVE_CHAR_TO_STRING_WITH_CULTURE
            text = value.ToString("D", CultureInfo.InvariantCulture);
            qc = quoteChar.ToString(CultureInfo.InvariantCulture);
#else
            text = value.ToString("D");
            qc = quoteChar.ToString();
#endif

            return qc + text + qc;
        }

        /// <summary>
        /// Converts the <see cref="TimeSpan"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="TimeSpan"/>.</returns>
        public static string ToString(TimeSpan value)
        {
            return ToString(value, '"');
        }

        internal static string ToString(TimeSpan value, char quoteChar)
        {
            return ToString(value.ToString(), quoteChar);
        }

        /// <summary>
        /// Converts the <see cref="Uri"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Uri"/>.</returns>
        public static string ToString(Uri? value)
        {
            if (value == null)
            {
                return Null;
            }

            return ToString(value, '"');
        }

        internal static string ToString(Uri value, char quoteChar)
        {
            return ToString(value.OriginalString, quoteChar);
        }

        /// <summary>
        /// Converts the <see cref="String"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="String"/>.</returns>
        public static string ToString(string? value)
        {
            return ToString(value, '"');
        }

        /// <summary>
        /// Converts the <see cref="String"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="delimiter">The string delimiter character.</param>
        /// <returns>A JSON string representation of the <see cref="String"/>.</returns>
        public static string ToString(string? value, char delimiter)
        {
            return ToString(value, delimiter, StringEscapeHandling.Default);
        }

        /// <summary>
        /// Converts the <see cref="String"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="delimiter">The string delimiter character.</param>
        /// <param name="stringEscapeHandling">The string escape handling.</param>
        /// <returns>A JSON string representation of the <see cref="String"/>.</returns>
        public static string ToString(string? value, char delimiter, StringEscapeHandling stringEscapeHandling)
        {
            if (delimiter != '"' && delimiter != '\'')
            {
                throw new ArgumentException("Delimiter must be a single or double quote.", nameof(delimiter));
            }

            return JavaScriptUtils.ToEscapedJavaScriptString(value, delimiter, true, stringEscapeHandling);
        }

        /// <summary>
        /// Converts the <see cref="Object"/> to its JSON string representation.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>A JSON string representation of the <see cref="Object"/>.</returns>
        public static string ToString(object? value)
        {
            if (value == null)
            {
                return Null;
            }

            PrimitiveTypeCode typeCode = ConvertUtils.GetTypeCode(value.GetType());

            switch (typeCode)
            {
                case PrimitiveTypeCode.String:
                    return ToString((string)value);
                case PrimitiveTypeCode.Char:
                    return ToString((char)value);
                case PrimitiveTypeCode.Boolean:
                    return ToString((bool)value);
                case PrimitiveTypeCode.SByte:
                    return ToString((sbyte)value);
                case PrimitiveTypeCode.Int16:
                    return ToString((short)value);
                case PrimitiveTypeCode.UInt16:
                    return ToString((ushort)value);
                case PrimitiveTypeCode.Int32:
                    return ToString((int)value);
                case PrimitiveTypeCode.Byte:
                    return ToString((byte)value);
                case PrimitiveTypeCode.UInt32:
                    return ToString((uint)value);
                case PrimitiveTypeCode.Int64:
                    return ToString((long)value);
                case PrimitiveTypeCode.UInt64:
                    return ToString((ulong)value);
                case PrimitiveTypeCode.Single:
                    return ToString((float)value);
                case PrimitiveTypeCode.Double:
                    return ToString((double)value);
                case PrimitiveTypeCode.DateTime:
                    return ToString((DateTime)value);
                case PrimitiveTypeCode.Decimal:
                    return ToString((decimal)value);
#if HAVE_DB_NULL_TYPE_CODE
                case PrimitiveTypeCode.DBNull:
                    return Null;
#endif
#if HAVE_DATE_TIME_OFFSET
                case PrimitiveTypeCode.DateTimeOffset:
                    return ToString((DateTimeOffset)value);
#endif
                case PrimitiveTypeCode.Guid:
                    return ToString((Guid)value);
                case PrimitiveTypeCode.Uri:
                    return ToString((Uri)value);
                case PrimitiveTypeCode.TimeSpan:
                    return ToString((TimeSpan)value);
#if HAVE_BIG_INTEGER
                case PrimitiveTypeCode.BigInteger:
                    return ToStringInternal((BigInteger)value);
#endif
            }

            throw new ArgumentException("Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith(CultureInfo.InvariantCulture, value.GetType()));
        }

        #region Serialize
        /// <summary>
        /// Serializes the specified object to a JSON string.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value)
        {
            return SerializeObject(value, null, (JsonSerializerSettings?)null);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, Formatting formatting)
        {
            return SerializeObject(value, formatting, (JsonSerializerSettings?)null);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using a collection of <see cref="JsonConverter"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, params JsonConverter[] converters)
        {
            JsonSerializerSettings? settings = (converters != null && converters.Length > 0)
                ? new JsonSerializerSettings { Converters = converters }
                : null;

            return SerializeObject(value, null, settings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and a collection of <see cref="JsonConverter"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="converters">A collection of converters used while serializing.</param>
        /// <returns>A JSON string representation of the object.</returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, Formatting formatting, params JsonConverter[] converters)
        {
            JsonSerializerSettings? settings = (converters != null && converters.Length > 0)
                ? new JsonSerializerSettings { Converters = converters }
                : null;

            return SerializeObject(value, null, formatting, settings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, JsonSerializerSettings? settings)
        {
            return SerializeObject(value, null, settings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using a type, formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="type">
        /// The type of the value being serialized.
        /// This parameter is used when <see cref="JsonSerializer.TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.
        /// </param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, Type? type, JsonSerializerSettings? settings)
        {
            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);

            return SerializeObjectInternal(value, type, jsonSerializer);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, Formatting formatting, JsonSerializerSettings? settings)
        {
            return SerializeObject(value, null, formatting, settings);
        }

        /// <summary>
        /// Serializes the specified object to a JSON string using a type, formatting and <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The object to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="settings">The <see cref="JsonSerializerSettings"/> used to serialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.</param>
        /// <param name="type">
        /// The type of the value being serialized.
        /// This parameter is used when <see cref="JsonSerializer.TypeNameHandling"/> is <see cref="TypeNameHandling.Auto"/> to write out the type name if the type of the value does not match.
        /// Specifying the type is optional.
        /// </param>
        /// <returns>
        /// A JSON string representation of the object.
        /// </returns>
        [DebuggerStepThrough]
        public static string SerializeObject(object? value, Type? type, Formatting formatting, JsonSerializerSettings? settings)
        {
            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);
            jsonSerializer.Formatting = formatting;

            return SerializeObjectInternal(value, type, jsonSerializer);
        }

        private static string SerializeObjectInternal(object? value, Type? type, JsonSerializer jsonSerializer)
        {
            StringBuilder sb = new StringBuilder(256);
            StringWriter sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = jsonSerializer.Formatting;

                jsonSerializer.Serialize(jsonWriter, value, type);
            }

            return sw.ToString();
        }
        #endregion

        #region Deserialize
        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static object? DeserializeObject(string value)
        {
            return DeserializeObject(value, null, (JsonSerializerSettings?)null);
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static object? DeserializeObject(string value, JsonSerializerSettings settings)
        {
            return DeserializeObject(value, null, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The <see cref="Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static object? DeserializeObject(string value, Type type)
        {
            return DeserializeObject(value, type, (JsonSerializerSettings?)null);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static T? DeserializeObject<T>(string value)
        {
            return DeserializeObject<T>(value, (JsonSerializerSettings?)null);
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified
        /// traditionally and must be inferred from the anonymous type passed
        /// as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        [DebuggerStepThrough]
        public static T? DeserializeAnonymousType<T>(string value, T anonymousTypeObject)
        {
            return DeserializeObject<T>(value);
        }

        /// <summary>
        /// Deserializes the JSON to the given anonymous type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The anonymous type to deserialize to. This can't be specified
        /// traditionally and must be inferred from the anonymous type passed
        /// as a parameter.
        /// </typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="anonymousTypeObject">The anonymous type object.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized anonymous type from the JSON string.</returns>
        [DebuggerStepThrough]
        public static T? DeserializeAnonymousType<T>(string value, T anonymousTypeObject, JsonSerializerSettings settings)
        {
            return DeserializeObject<T>(value, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using a collection of <see cref="JsonConverter"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static T? DeserializeObject<T>(string value, params JsonConverter[] converters)
        {
            return (T?)DeserializeObject(value, typeof(T), converters);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="value">The object to deserialize.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static T? DeserializeObject<T>(string value, JsonSerializerSettings? settings)
        {
            return (T?)DeserializeObject(value, typeof(T), settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using a collection of <see cref="JsonConverter"/>.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The type of the object to deserialize.</param>
        /// <param name="converters">Converters to use while deserializing.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        [DebuggerStepThrough]
        public static object? DeserializeObject(string value, Type type, params JsonConverter[] converters)
        {
            JsonSerializerSettings? settings = (converters != null && converters.Length > 0)
                ? new JsonSerializerSettings { Converters = converters }
                : null;

            return DeserializeObject(value, type, settings);
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The JSON to deserialize.</param>
        /// <param name="type">The type of the object to deserialize to.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static object? DeserializeObject(string value, Type? type, JsonSerializerSettings? settings)
        {
            ValidationUtils.ArgumentNotNull(value, nameof(value));

            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);

            // by default DeserializeObject should check for additional content
            if (!jsonSerializer.IsCheckAdditionalContentSet())
            {
                jsonSerializer.CheckAdditionalContent = true;
            }

            using (JsonTextReader reader = new JsonTextReader(new StringReader(value)))
            {
                return jsonSerializer.Deserialize(reader, type);
            }
        }
        #endregion

        #region Populate
        /// <summary>
        /// Populates the object with values from the JSON string.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        [DebuggerStepThrough]
        public static void PopulateObject(string value, object target)
        {
            PopulateObject(value, target, null);
        }

        /// <summary>
        /// Populates the object with values from the JSON string using <see cref="JsonSerializerSettings"/>.
        /// </summary>
        /// <param name="value">The JSON to populate values from.</param>
        /// <param name="target">The target object to populate values onto.</param>
        /// <param name="settings">
        /// The <see cref="JsonSerializerSettings"/> used to deserialize the object.
        /// If this is <c>null</c>, default serialization settings will be used.
        /// </param>
        public static void PopulateObject(string value, object target, JsonSerializerSettings? settings)
        {
            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(settings);

            using (JsonReader jsonReader = new JsonTextReader(new StringReader(value)))
            {
                jsonSerializer.Populate(jsonReader, target);

                if (settings != null && settings.CheckAdditionalContent)
                {
                    while (jsonReader.Read())
                    {
                        if (jsonReader.TokenType != JsonToken.Comment)
                        {
                            throw JsonSerializationException.Create(jsonReader, "Additional text found in JSON string after finishing deserializing object.");
                        }
                    }
                }
            }
        }
        #endregion

        #region Xml
#if HAVE_XML_DOCUMENT
        /// <summary>
        /// Serializes the <see cref="XmlNode"/> to a JSON string.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <returns>A JSON string of the <see cref="XmlNode"/>.</returns>
        public static string SerializeXmlNode(XmlNode? node)
        {
            return SerializeXmlNode(node, Formatting.None);
        }

        /// <summary>
        /// Serializes the <see cref="XmlNode"/> to a JSON string using formatting.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>A JSON string of the <see cref="XmlNode"/>.</returns>
        public static string SerializeXmlNode(XmlNode? node, Formatting formatting)
        {
            XmlNodeConverter converter = new XmlNodeConverter();

            return SerializeObject(node, formatting, converter);
        }

        /// <summary>
        /// Serializes the <see cref="XmlNode"/> to a JSON string using formatting and omits the root object if <paramref name="omitRootObject"/> is <c>true</c>.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="omitRootObject">Omits writing the root object.</param>
        /// <returns>A JSON string of the <see cref="XmlNode"/>.</returns>
        public static string SerializeXmlNode(XmlNode? node, Formatting formatting, bool omitRootObject)
        {
            XmlNodeConverter converter = new XmlNodeConverter { OmitRootObject = omitRootObject };

            return SerializeObject(node, formatting, converter);
        }

        /// <summary>
        /// Deserializes the <see cref="XmlNode"/> from a JSON string.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <returns>The deserialized <see cref="XmlNode"/>.</returns>
        public static XmlDocument? DeserializeXmlNode(string value)
        {
            return DeserializeXmlNode(value, null);
        }

        /// <summary>
        /// Deserializes the <see cref="XmlNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <returns>The deserialized <see cref="XmlNode"/>.</returns>
        public static XmlDocument? DeserializeXmlNode(string value, string? deserializeRootElementName)
        {
            return DeserializeXmlNode(value, deserializeRootElementName, false);
        }

        /// <summary>
        /// Deserializes the <see cref="XmlNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>
        /// and writes a Json.NET array attribute for collections.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A value to indicate whether to write the Json.NET array attribute.
        /// This attribute helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <returns>The deserialized <see cref="XmlNode"/>.</returns>
        public static XmlDocument? DeserializeXmlNode(string value, string? deserializeRootElementName, bool writeArrayAttribute)
        {
            return DeserializeXmlNode(value, deserializeRootElementName, writeArrayAttribute, false);
        }

        /// <summary>
        /// Deserializes the <see cref="XmlNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>,
        /// writes a Json.NET array attribute for collections, and encodes special characters.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A value to indicate whether to write the Json.NET array attribute.
        /// This attribute helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <param name="encodeSpecialCharacters">
        /// A value to indicate whether to encode special characters when converting JSON to XML.
        /// If <c>true</c>, special characters like ':', '@', '?', '#' and '$' in JSON property names aren't used to specify
        /// XML namespaces, attributes or processing directives. Instead special characters are encoded and written
        /// as part of the XML element name.
        /// </param>
        /// <returns>The deserialized <see cref="XmlNode"/>.</returns>
        public static XmlDocument? DeserializeXmlNode(string value, string? deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
        {
            XmlNodeConverter converter = new XmlNodeConverter();
            converter.DeserializeRootElementName = deserializeRootElementName;
            converter.WriteArrayAttribute = writeArrayAttribute;
            converter.EncodeSpecialCharacters = encodeSpecialCharacters;

            return (XmlDocument?)DeserializeObject(value, typeof(XmlDocument), converter);
        }
#endif

#if HAVE_XLINQ
        /// <summary>
        /// Serializes the <see cref="XNode"/> to a JSON string.
        /// </summary>
        /// <param name="node">The node to convert to JSON.</param>
        /// <returns>A JSON string of the <see cref="XNode"/>.</returns>
        public static string SerializeXNode(XObject? node)
        {
            return SerializeXNode(node, Formatting.None);
        }

        /// <summary>
        /// Serializes the <see cref="XNode"/> to a JSON string using formatting.
        /// </summary>
        /// <param name="node">The node to convert to JSON.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <returns>A JSON string of the <see cref="XNode"/>.</returns>
        public static string SerializeXNode(XObject? node, Formatting formatting)
        {
            return SerializeXNode(node, formatting, false);
        }

        /// <summary>
        /// Serializes the <see cref="XNode"/> to a JSON string using formatting and omits the root object if <paramref name="omitRootObject"/> is <c>true</c>.
        /// </summary>
        /// <param name="node">The node to serialize.</param>
        /// <param name="formatting">Indicates how the output should be formatted.</param>
        /// <param name="omitRootObject">Omits writing the root object.</param>
        /// <returns>A JSON string of the <see cref="XNode"/>.</returns>
        public static string SerializeXNode(XObject? node, Formatting formatting, bool omitRootObject)
        {
            XmlNodeConverter converter = new XmlNodeConverter { OmitRootObject = omitRootObject };

            return SerializeObject(node, formatting, converter);
        }

        /// <summary>
        /// Deserializes the <see cref="XNode"/> from a JSON string.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <returns>The deserialized <see cref="XNode"/>.</returns>
        public static XDocument? DeserializeXNode(string value)
        {
            return DeserializeXNode(value, null);
        }

        /// <summary>
        /// Deserializes the <see cref="XNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <returns>The deserialized <see cref="XNode"/>.</returns>
        public static XDocument? DeserializeXNode(string value, string? deserializeRootElementName)
        {
            return DeserializeXNode(value, deserializeRootElementName, false);
        }

        /// <summary>
        /// Deserializes the <see cref="XNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>
        /// and writes a Json.NET array attribute for collections.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A value to indicate whether to write the Json.NET array attribute.
        /// This attribute helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <returns>The deserialized <see cref="XNode"/>.</returns>
        public static XDocument? DeserializeXNode(string value, string? deserializeRootElementName, bool writeArrayAttribute)
        {
            return DeserializeXNode(value, deserializeRootElementName, writeArrayAttribute, false);
        }

        /// <summary>
        /// Deserializes the <see cref="XNode"/> from a JSON string nested in a root element specified by <paramref name="deserializeRootElementName"/>,
        /// writes a Json.NET array attribute for collections, and encodes special characters.
        /// </summary>
        /// <param name="value">The JSON string.</param>
        /// <param name="deserializeRootElementName">The name of the root element to append when deserializing.</param>
        /// <param name="writeArrayAttribute">
        /// A value to indicate whether to write the Json.NET array attribute.
        /// This attribute helps preserve arrays when converting the written XML back to JSON.
        /// </param>
        /// <param name="encodeSpecialCharacters">
        /// A value to indicate whether to encode special characters when converting JSON to XML.
        /// If <c>true</c>, special characters like ':', '@', '?', '#' and '$' in JSON property names aren't used to specify
        /// XML namespaces, attributes or processing directives. Instead special characters are encoded and written
        /// as part of the XML element name.
        /// </param>
        /// <returns>The deserialized <see cref="XNode"/>.</returns>
        public static XDocument? DeserializeXNode(string value, string? deserializeRootElementName, bool writeArrayAttribute, bool encodeSpecialCharacters)
        {
            XmlNodeConverter converter = new XmlNodeConverter();
            converter.DeserializeRootElementName = deserializeRootElementName;
            converter.WriteArrayAttribute = writeArrayAttribute;
            converter.EncodeSpecialCharacters = encodeSpecialCharacters;

            return (XDocument?)DeserializeObject(value, typeof(XDocument), converter);
        }
#endif
        #endregion
    }
}

```

