using ITI_Attendance.Models;

namespace ITI_Attendance.Services
{
    public interface IStudentProgramIntakeService
    {
        public List<StudentProgramIntake> GetDegree(int crs_Id);
        public List<StudentProgramIntake> GetDegree(int crs_Id, int dept_Id);
        public StudentProgramIntake GetStudentCourse(int crsId, int stId);
        public void Add(StudentProgramIntake studentCourse);
        public void Update(StudentProgramIntake studentCourse);
    }
}
