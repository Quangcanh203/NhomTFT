using Microsoft.AspNetCore.Identity;

namespace DoAn.Models
{
    public class AddUserModel : IdentityUser
    {
        public string Occupation { get; set; }
        public string[] RoleId { get; set; } // Sửa từ RoleId thành RoleIds để hỗ trợ nhiều vai trò
    }
}
