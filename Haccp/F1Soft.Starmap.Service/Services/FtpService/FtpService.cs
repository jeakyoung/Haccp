using F1Soft.Starmap.Service.Services.EnvService;
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
