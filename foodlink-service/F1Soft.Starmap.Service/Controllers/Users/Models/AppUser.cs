using F1Soft.Starmap.Service.Controllers.Users.Enums;
using Microsoft.AspNetCore.Identity;

namespace F1Soft.Starmap.Service.Controllers.Users.Models
{
    /// <summary>
    /// 인증용 사용자 모델
    /// </summary>
    public class AppUser : IdentityUser
    {
        /// <summary>
        /// 권한
        /// </summary>
        public Role Role { get; set; }

    }
}
