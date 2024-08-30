using ITI_Attendance.Models;

namespace ITI_Attendance.Services
{
    public interface IStudentAttendeceService
    {
        public StudentAttendance GetById(int id);
        public void Add(StudentAttendance st);
        public void Update(StudentAttendance st);
    }
}
