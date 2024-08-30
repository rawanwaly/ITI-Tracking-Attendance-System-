using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_Attendance.Models
{
    public class UserRole
    {

        [ForeignKey("UserLogin")]
        public int UserLoginId { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
