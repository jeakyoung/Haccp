//using Azure;
//using F1Soft.Starmap.Service.Controllers.Users.Models;
//using F1Soft.Starmap.Service.Services;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.Annotations;

//namespace F1Soft.Starmap.Service.Controllers.Sample
//{
//    /// <summary>
//    /// »ùÇÃ API
//    /// </summary>
//    [ApiController]
//    [Route("/api/[controller]")]
//    public class SampleAPIController : ControllerBase
//    {
//        private readonly TokenService _tokenService;

//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<SampleAPIController> _logger;

//        /// <summary>
//        /// SampleAPIController »ý¼ºÀÚ
//        /// </summary>
//        /// <param name="logger"></param>
//        /// <param name="tokenService"></param>
//        public SampleAPIController(ILogger<SampleAPIController> logger, TokenService tokenService)
//        {
//            _logger = logger;
//            _tokenService = tokenService;
//        }

//        /// <summary>
//        /// ³¯¾¾¸¦ °¡Á®¿À´Â API »ùÇÃ
//        /// </summary>
//        /// <returns></returns>
//        [HttpGet]
//        [Route("GetSampleAPI")]
//        [ProducesResponseType(200)]
//        [ProducesResponseType(400)]
//        public IEnumerable<SampleModel> Get()
//        {
//            return Enumerable.Range(1, 5).Select(index => new SampleModel
//            {
//                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//                TemperatureC = Random.Shared.Next(-20, 55),
//                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
//            })
//            .ToArray();
//        }
//    }
//}
