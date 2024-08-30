using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_Attendance.Models
{
    public class Student
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; }
        public int? Age { get; set; }
        public string ImageName { get; set; }
        [RegularExpression(@"[a-zA-Z0-9_]+@[a-zA-Z]+.[a-zA-Z]{2,4}")]
        [Remote("CheckEmailExist", "Student", AdditionalFields = "Name,Age")]
        public string Email { get; set; }
        public bool IsVerified { get; set; }
        [ForeignKey("Department")]
        public int? DeptNum { get; set; }
        public virtual Department Department { get; set; }
        public virtual ICollection<Program> Programs { get; set; } = new HashSet<Program>();
        [ForeignKey("Hr")]
        public int? HrId { get; set; }
        public virtual Hr Hr { get; set; }
        public virtual ICollection<StudentProgramIntake> Intakes { get; set; } = new HashSet<StudentProgramIntake>();
        [ForeignKey("UserLoginId")]
        public int? UserLoginId { get; set; } 
        public virtual UserLogin UserLogin { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password")]
        [NotMapped]
        public string CPassword { get; set; }
        public virtual StudentAttendance StudentAttendece { get; set; }



    }
}
