using F1Soft.Starmap.Service.Services.EnvService;
using System.Net;

namespace F1Soft.Starmap.Service.Services.FtpService;

/// <summary>
/// FTP Client Service
/// </summary>
public class FtpService : IFtpService
{
    private readonly string _ftpUrl;
    private readonly string _ftpUser;
    private readonly string _ftpPassword;

    /// <summary>
    /// FTP Service
    /// </summary>
    /// <param name="envService"></param>
    public FtpService(IEnvService envService)
    {
        _ftpUrl = envService.GetFtpUrl();
        _ftpUser = envService.GetFtpUser();
        _ftpPassword = envService.GetFtpPassword();
    }

#pragma warning disable SYSLIB0014
    /// <summary>
    /// FTP의 파일을 Stream 형태로 가져옵니다.
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public Stream GetStream(string filePath)
    {
        Uri ftpUri = new Uri(_ftpUrl + filePath);
        var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
        ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
        ftpRequest.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
        var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
        return ftpResponse.GetResponseStream();
    }

    /// <summary>
    /// FTP에 파일을 업로드합니다.
    /// </summary>
    /// <param name="fileName">저장할 파일명 (예: materialCode.png)</param>
    /// <param name="stream">업로드할 파일 스트림</param>
    public async Task UploadAsync(string fileName, Stream stream)
    {
        Uri ftpUri = new Uri(_ftpUrl + fileName);
        var ftpRequest = (FtpWebRequest)WebRequest.Create(ftpUri);
        ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
        ftpRequest.Credentials = new NetworkCredential(_ftpUser, _ftpPassword);
        ftpRequest.UseBinary = true;

        using var requestStream = await ftpRequest.GetRequestStreamAsync();
        await stream.CopyToAsync(requestStream);

        using var response = (FtpWebResponse)await ftpRequest.GetResponseAsync();
    }
#pragma warning restore SYSLIB0014
}
