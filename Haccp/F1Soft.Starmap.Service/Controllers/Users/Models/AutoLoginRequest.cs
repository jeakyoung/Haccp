using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// JWT Token 요청 모델 (자동로그인 활용)
    /// </summary>
    public class AutoLoginRequest
    {
        /// <summary>
        /// JWT Token
        /// </summary>
        [Required]
        public string? JwtToken { get; set; }
    }
}
