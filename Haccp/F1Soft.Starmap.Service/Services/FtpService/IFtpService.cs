namespace F1Soft.Starmap.Service.Services.FtpService
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
