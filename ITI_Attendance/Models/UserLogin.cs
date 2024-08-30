using System.ComponentModel.DataAnnotations;

namespace ITI_Attendance.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public virtual Hr Hr { get; set; }
        public virtual Student Student { get; set; }

    }
}
