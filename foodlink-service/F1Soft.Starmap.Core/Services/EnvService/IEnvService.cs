namespace F1Soft.Starmap.Core.Services.EnvService;
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
