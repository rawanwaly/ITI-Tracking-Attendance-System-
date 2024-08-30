using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_Attendance.Models
{
    public class StudentAttendance
    {
        [Key]
        public int Id { get; set; }
        public bool Saturday { get; set; } = false;
        public bool Sunday { get; set; }=false;
        public bool Monday { get; set; } = false;
        public bool Tuesday { get; set; } =false;
        public bool Wednesday { get; set; } = false;
        public bool  Thursday { get; set; } = false ;
        public bool Friday { get; set; } = false;
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [NotMapped]
        public string SelectedDay { get; set; }

    }
}
