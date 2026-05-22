using SapNwRfc;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace F1Soft.Starmap.Service.Controllers.Database.Models
{
    /// <summary>
    /// Database Procedure 요청
    /// </summary>
    public class SapRFCRequest
    {
        /// <summary>
        /// 프로시져 명
        /// </summary>
        public string? RFCName { get; set; }

        /// <summary>
        /// 파라미터 리스트
        /// </summary>
        public Dictionary<string, object>? Parameters { get; set; }
    }


    #region P040
    /// <summary>
    /// SapRFC_P040
    /// </summary>
    public class SapRFC_P040
    {
        /// <summary>
        /// I_IFID
        /// </summary>
        [SapName("I_IFID")]
        public string? I_IFID { get; set; }

        /// <summary>
        /// IT_DATA
        /// </summary>
        [SapName("IT_DATA")]
        public SapRFC_P040Item[]? IT_DATA { get; set; }
    }

    /// <summary>
    /// SapRFC_P040Item
    /// </summary>
    public class SapRFC_P040Item
    {
        /// <summary>
        /// CSYST
        /// </summary>
        [SapName("CSYST")]
        public string? CSYST { get; set; }

        /// <summary>
        /// ZMESIFNO
        /// </summary>
        [SapName("ZMESIFNO")]
        public string? ZMESIFNO { get; set; }
        
    }
    #endregion


    /// <summary>
    /// SapRFCResult 결과
    /// </summary>
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Local", Justification = "Used as generic parameter")]
    public class SapRFCResult
    {
        /// <summary>
        /// ES Result
        /// </summary>
        [SapName("ES_RESULT")]
        public SapRFCResultItem? ES_RESULT { get; set; }
    }

    /// <summary>
    /// SapRFCResultItem 결과 항목
    /// </summary>
    public class SapRFCResultItem
    {
        /// <summary>
        /// Return Code
        /// </summary>
        [SapName("RETCD")]
        public string? RETCD { get; set; }

        /// <summary>
        /// Return Message
        /// </summary>
        [SapName("RETMG")]
        public string? RETMG { get; set; }
    }
}
