using ITI_Attendance.Models;
using static Demo1.Service.StudentService;

namespace Demo1.Service
{
    public interface IStudentService
    {
        public List<Student> GetAll();
        public void Add(Student std);
        public void Edit(Student std,int id);
        public void Delete(int id);
        public Student GetById(int id);
        public Student ExistingStdEmail(Student std,int id);
        public Student CheckEmail(string Email);
        public Student GetByName(string username);
        public List<Student> GetUnverifiedStudentsByHr(int hrId);
        public Student GetStudentById(int id);
        public Student GetStudentById(int studentId, int hrId);
        public void UpdateStudent(Student student);

    }
}
