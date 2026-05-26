using F1Soft.Starmap.Service.Services.EnvService;
using F1Soft.Starmap.Service.Services.FtpService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace F1Soft.Starmap.Service.Controllers.DataVoucher.MaterialImage;

/// <summary>
/// 데이터 바우처 원료입고 이미지 관리
/// </summary>
[ApiController]
[Route("api/[controller]")]
#if !DEBUG
[Authorize]
#endif
[ProducesResponseType(200)]
[ProducesResponseType(401)]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IFtpService _ftpService;
    private readonly string _httpsBaseUrl;

    /// <summary>
    ///
    /// </summary>
    public ImageController(IEnvService envService, ILogger<ImageController> logger, IFtpService ftpService)
    {
        _logger = logger;
        _ftpService = ftpService;
        _httpsBaseUrl = envService.GetMaterialImageHttpsBaseUrl();
    }

    /// <summary>
    /// 이미지를 FTP에 업로드합니다.
    /// </summary>
    /// <param name="fileName">저장할 파일명 (확장자 포함, 예: image.png)</param>
    /// <param name="image">업로드할 이미지 파일</param>
    /// <returns>업로드된 이미지의 URL</returns>
    [HttpPost]
    [Route("UploadImage")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage([FromQuery] string fileName, IFormFile image)
    {
        try
        {
            if (image == null || image.Length == 0)
                return BadRequest(new { message = "이미지 파일이 없습니다." });

            if (!image.ContentType.StartsWith("image/"))
                return BadRequest(new { message = "이미지 파일만 업로드 가능합니다." });

            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest(new { message = "파일명을 입력해주세요." });

            using var stream = image.OpenReadStream();
            await _ftpService.UploadAsync(fileName, stream);

            string imageUrl = _httpsBaseUrl + fileName;

            _logger.LogInformation("이미지 업로드 완료: {FileName}", fileName);

            return Ok(new
            {
                message = "이미지 업로드 성공",
                fileName = fileName,
                imageUrl = imageUrl
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "이미지 업로드 실패: {FileName}", fileName);
            return StatusCode(500, new { message = "이미지 업로드 중 오류가 발생했습니다." });
        }
    }

    /// <summary>
    /// 이미지의 HTTPS URL을 반환합니다.
    /// </summary>
    /// <param name="fileName">파일명 (확장자 포함, 예: image.png)</param>
    /// <returns>이미지 렌더링용 HTTPS URL</returns>
    [HttpGet]
    [Route("GetImageUrl")]
    public IActionResult GetImageUrl(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return BadRequest(new { message = "파일명을 입력해주세요." });

        string imageUrl = _httpsBaseUrl + fileName;

        return Ok(new
        {
            fileName = fileName,
            imageUrl = imageUrl
        });
    }

    /// <summary>
    /// 이미지를 다운로드합니다. (FTP에서 직접 스트리밍)
    /// </summary>
    /// <param name="fileName">파일명 (확장자 포함, 예: image.png)</param>
    /// <returns>PNG 이미지 파일</returns>
    [HttpGet]
    [Route("DownloadImage")]
    public IActionResult DownloadImage(string fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest(new { message = "파일명을 입력해주세요." });

            var stream = _ftpService.GetStream(fileName);

            return File(stream, "image/png", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "이미지 다운로드 실패: {FileName}", fileName);
            return StatusCode(500, new { message = "이미지 다운로드 중 오류가 발생했습니다." });
        }
    }
}
