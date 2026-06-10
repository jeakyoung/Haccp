using System.ComponentModel.DataAnnotations;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 사용자 인증 요청 모델
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// 사용자 ID
        /// </summary>
        [Required]
        public string? UserID { get; set; }

        /// <summary>
        /// 패스워드 (암호화)
        /// </summary>
        [Required]
        public string? UserPassword { get; set; }
    }
}
