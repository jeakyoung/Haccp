using F1Soft.Starmap.Service.Controllers.Database.Models;
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