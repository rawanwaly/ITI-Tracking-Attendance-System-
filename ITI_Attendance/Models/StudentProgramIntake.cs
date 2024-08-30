using System.ComponentModel.DataAnnotations.Schema;

namespace ITI_Attendance.Models
{
    public class StudentProgramIntake
    {
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
        [ForeignKey("Program")]
        public int ProgramId { get; set; }
        public virtual Program Program { get; set; }
        public DateTime Intake { get; set; }
        public int? Degree { get; set; }


    }


}
