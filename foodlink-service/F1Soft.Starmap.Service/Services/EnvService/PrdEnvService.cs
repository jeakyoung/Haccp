namespace F1Soft.Starmap.Service.Services.EnvService;
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
    public string GetEmpCardUrl()
    {
        string empCardUrl = _configuration[$"ConnectionStrings:{GetEnvironmentName()}EmpCardUrl"] ?? string.Empty;

        return empCardUrl;
    }

    /// <summary>
    /// FTP 계정 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetFtpUser()
    {
        return _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}User"] ?? string.Empty;
    }

    /// <summary>
    /// FTP 비밀번호 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetFtpPassword()
    {
        return _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}Password"] ?? string.Empty;
    }

    /// <summary>
    /// 원료 이미지 HTTPS 베이스 URL 가져오기
    /// </summary>
    /// <returns></returns>
    public string GetMaterialImageHttpsBaseUrl()
    {
        return _configuration[$"ConnectionStrings:Ftp{GetEnvironmentName()}MaterialImageHttpsUrl"] ?? string.Empty;
    }
}
