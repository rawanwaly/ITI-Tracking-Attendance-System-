using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_Attendance.Models
{
    public class Hr
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        public int? Age { get; set; }    
        [RegularExpression(@"[a-zA-Z0-9_]+@[a-zA-Z]+.[a-zA-Z]{2,4}")]
        [Remote("CheckEmailExist", "Hr", AdditionalFields = "Name,Age")]
        public string Email { get; set; }
        public virtual ICollection<Student> VerifiedStudents { get; set; } = new HashSet<Student>();

        [ForeignKey("UserLoginId")]
        public int UserLoginId { get; set; }
        public virtual UserLogin UserLogin { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        [NotMapped]
        public string CPassword { get; set; }


    }
}
