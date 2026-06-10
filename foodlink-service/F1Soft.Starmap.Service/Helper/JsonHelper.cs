using F1Soft.Starmap.Service.Controllers.Database.Models;
using Newtonsoft.Json;

namespace F1Soft.Starmap.Service.Helper
{
    /// <summary>
    /// Json Helper
    /// </summary>
    public static class JsonHelper
    {

        /// <summary>
        /// DbResult 형식을 Json Object 형식으로 변환합니다.
        /// </summary>
        /// <param name="dbResult"></param>
        /// <returns></returns>
        public static object ConvertDbResultToJsonObject(object dbResult)
        {
            string jsonContent = JsonConvert.SerializeObject(dbResult);
            object jsonObject = System.Text.Json.JsonSerializer.Deserialize<object>(jsonContent)!;

            return jsonObject;
        }




    }
}
