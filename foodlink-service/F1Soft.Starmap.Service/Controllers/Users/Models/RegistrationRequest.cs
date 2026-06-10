using F1Soft.Starmap.Service.Controllers.Users.Enums;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 사용자 등록 모델 (미사용)
    /// </summary>
    public class RegistrationRequest
    {
        /// <summary>
        /// 이메일
        /// </summary>
        [Required]
        public string? Email { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required]
        public string? Username { get; set; }


        /// <summary>
        /// 비밀번호
        /// </summary>
        [Required]
        public string? Password { get; set; }
        
        /// <summary>
        /// 사용자 권한, 기본 값 : User
        /// </summary>
        public Role Role { get; set; } = Role.User;
    }
}
