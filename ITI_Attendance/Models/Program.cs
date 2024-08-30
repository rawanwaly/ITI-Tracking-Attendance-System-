using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ITI_Attendance.Models
{
    public class Program
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<StudentProgramIntake> Intakes { get; set; } = new HashSet<StudentProgramIntake>();
        public virtual ICollection<Department> Departments { get; set; } = new HashSet<Department>();

    }

}
