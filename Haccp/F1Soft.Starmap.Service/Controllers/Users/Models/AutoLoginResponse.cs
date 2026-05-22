using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 자동로그인 결과 반환 DTO
    /// </summary>
    public class AutoLoginResponse
    {
        /// <summary>
        /// 토큰 유효 결과
        /// </summary>
        [Required]
        public bool? Result { get; set; }

        /// <summary>
        /// 당일 발급 토큰 여부
        /// </summary>
        [Required]
        public bool? TodayToken { get; set; }

        /// <summary>
        /// 유저 정보
        /// </summary>
        public AuthResponse? authResponse { get; set; }
        
    }
}
