namespace F1Soft.Starmap.Service.Services.EnvService;
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
