using System.ComponentModel.DataAnnotations;

namespace ITI_Attendance.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "*")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        public int Capcity { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new HashSet<Student>();
        public virtual ICollection<Program> Programs { get; set; } = new HashSet<Program>();
    }
}
