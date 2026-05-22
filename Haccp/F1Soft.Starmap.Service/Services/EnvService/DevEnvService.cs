using System.Configuration;

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

