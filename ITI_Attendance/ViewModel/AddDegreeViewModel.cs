using ITI_Attendance.Models;

namespace ITI_Attendance.ViewModel
{
    public class AddDegreeViewModel
    {
      
            public Department Department { get; set; }
            public ITI_Attendance.Models.Program Program { get; set; }
            public List<StudentProgramIntake> Degrees { get; set; }
    }
}
