namespace F1Soft.Starmap.Service.Services.FtpService
{
    /// <summary>
    /// FTP 서비스 인터페이스
    /// </summary>
    public interface IFtpService
    {
        /// <summary>
        /// FTP에서 파일을 Stream 형태로 가져옵니다.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream GetStream(string filePath);

        /// <summary>
        /// FTP에 파일을 업로드합니다.
        /// </summary>
        /// <param name="fileName">저장할 파일명 (예: materialCode.png)</param>
        /// <param name="stream">업로드할 파일 스트림</param>
        Task UploadAsync(string fileName, Stream stream);
    }
}
