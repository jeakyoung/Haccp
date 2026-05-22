//using F1Soft.Starmap.Service.Controllers.Database.Models;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using SapNwRfc.Pooling;
//using System.Reflection;

//namespace F1Soft.Starmap.Service.Controllers.Interface.Sap;

///// <summary>
///// SAP RFC를 이용한 인터페이스 API
///// </summary>
//[ApiController]
//[Route("api/[controller]")]
//public class SapRFCController : ControllerBase
//{
//    private readonly ISapPooledConnection _connection;
//    private readonly ILogger<SapRFCController> _logger;

//    /// <summary>
//    /// SapRFCController 생성자
//    /// </summary>
//    /// <param name="configuration"></param>
//    /// <param name="logger"></param>
//    /// <param name="connection"></param>
//    public SapRFCController(IConfiguration configuration, ILogger<SapRFCController> logger, ISapPooledConnection connection)
//    {
//        _connection = connection;
//        _logger = logger;
//    }

//    /// <summary>
//    /// SAP RFC 호출 API
//    /// </summary>
//    /// <param name="SapRFCs"></param>
//    /// <returns></returns>
//    [HttpPost]
//    [Route("CallSapRFC")]
//    [EnableCors("AllowSpecificOrigin")]
//    public IActionResult SapRFC([FromBody] List<SapRFCRequest> SapRFCs)
//    {
//        try
//        {
//            string rfcID = SapRFCs[0].RFCName!;
//            SapRFCResult result = new SapRFCResult() { ES_RESULT = new SapRFCResultItem() { RETCD = "X", RETMG = "XX" } };

//            if (rfcID.Equals("P030"))
//            {
//                List<SapRFC_P040Item> IT_DATA_List = new List<SapRFC_P040Item>();
//                foreach (SapRFCRequest sqlProcedure in SapRFCs)
//                {
//                    var row = new SapRFC_P040Item();
//                    foreach (var param in sqlProcedure.Parameters!)
//                    {
//                        // 1. 대상 모델(row)에서 param.Name과 일치하는 속성 정보 가져오기
//                        PropertyInfo propertyInfo = typeof(SapRFC_P040Item).GetProperty(param.Key)!;

//                        if (propertyInfo != null && propertyInfo.CanWrite)
//                        {
//                            if (propertyInfo.PropertyType == typeof(string))
//                            {
//                                propertyInfo.SetValue(row, param.Value.ToString());
//                            }
//                            else if (propertyInfo.PropertyType == typeof(int))
//                            {
//                                int.TryParse(param.Value.ToString(), out int parseValue);
//                                propertyInfo.SetValue(row, parseValue);
//                            }
//                            else if (propertyInfo.PropertyType == typeof(decimal))
//                            {
//                                decimal.TryParse(param.Value.ToString(), out decimal parseValue);
//                                propertyInfo.SetValue(row, parseValue);
//                            }
//                        }
//                    }
//                    IT_DATA_List.Add(row);
//                }

//                result = _connection.InvokeFunction<SapRFCResult>("ZDPSF_" + rfcID, new SapRFC_P040
//                {
//                    I_IFID = rfcID,
//                    IT_DATA = IT_DATA_List.ToArray()
//                });
//            }
//            else
//            {
//                _logger.LogError("RFC ID 오류"); // 정보 로그 작성
//                return StatusCode(403, "존재하지 않는 RFC ID");
//            }

//            string resultJson = JsonConvert.SerializeObject(result);
//            return Ok(resultJson);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Error 발생"); // 정보 로그 작성
//            return StatusCode(500, ex.ToString());
//        }
//    }
//}
