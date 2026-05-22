using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace F1Soft.Starmap.Service.Controllers.Sample
{
    /// <summary>
    /// »ùÇÃ ¸ðµ¨
    /// </summary>
    public class SampleModel
    {
        /// <summary>
        /// ³¯Â¥
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// ¿Âµµ ¼·¾¾
        /// </summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// ¿Âµµ È­¾¾
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>
        /// ¼³¸í
        /// </summary>
        public string? Summary { get; set; }
    }
}
