using ITI_Attendance.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ITI_Attendance.Services
{
    public class StudentProgramIntakeService : IStudentProgramIntakeService
    {
        private readonly ITIDbContext db;
        public StudentProgramIntakeService(ITIDbContext _db) => db = _db;
        public void Add(StudentProgramIntake studentCourse)
        {
            db.Intakes.Add(studentCourse);
            db.SaveChanges();
        }
        public List<StudentProgramIntake> GetDegree(int crs_Id)=> db.Intakes.Where(sc => sc.ProgramId == crs_Id).ToList();
        public List<StudentProgramIntake> GetDegree(int crs_Id, int dept_Id)=> db.Intakes
                        .Where(sc => sc.ProgramId == crs_Id && sc.Student.Department.Id == dept_Id)
                        .Include(sc => sc.Student)
                        .Include(sc => sc.Program)
                        .ToList();

        public StudentProgramIntake GetStudentCourse(int crsId, int stId) => db.Intakes.FirstOrDefault(c => c.ProgramId == crsId && c.StudentId == stId);

        public void Update(StudentProgramIntake studentCourse)
        {
            db.Intakes.Update(studentCourse);
            db.SaveChanges();
        }
    }
}
