using ITI_Attendance.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo1.Service
{
    public class StudentService:IStudentService
    {
       private readonly ITIDbContext db;
        public StudentService(ITIDbContext _db) => db = _db;
        
        public List<Student> GetAll() => db.Students.Include(d=>d.Department).Include(s=>s.Hr).ToList();
        
        public Student GetById(int id) => db.Students.Include(d=>d.Department).Include(s => s.Hr)
            .Include(s => s.Intakes).ThenInclude(i => i.Program)
            .FirstOrDefault(d=>d.Id == id);

        public void Delete(int id)
        {
            Student student = db.Students.Include(d => d.Department).Include(s => s.Hr)
            .Include(s => s.Intakes).ThenInclude(i => i.Program).
            SingleOrDefault(d=>d.Id == id);
            db.Students.Remove(student);
            db.SaveChanges();
        }
        public void Add(Student std)
        {
            db.Students.Add(std);
            db.SaveChanges();
        }

        public void Edit(Student std, int id)
        {
            var existingStudent = db.Students.Local.FirstOrDefault(s => s.Id == id);
            if (existingStudent != null)
            {
                db.Entry(existingStudent).State = EntityState.Detached;
            }

            var studentToUpdate = db.Students.Find(id);
            if (studentToUpdate != null)
            {
                studentToUpdate.Name = std.Name;
                studentToUpdate.Email = std.Email;
                studentToUpdate.Age = std.Age;
                studentToUpdate.Password = std.Password;
                studentToUpdate.ImageName = std.ImageName;

                db.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Student not found.");
            }
        }

        public Student ExistingStdEmail(Student std, int id)
        {
            return db.Students
                .Where(s => s.Email == std.Email && s.Id != id) // Ensure that the Id property matches your Student class
                .FirstOrDefault();
        }
        public Student CheckEmail(string Email) => db.Students.FirstOrDefault(s => s.Email == Email);
        public Student GetByName(string username)
        {
            return db.Students
                .Include(s => s.Department)
                .Include(s => s.Intakes)
                .ThenInclude(i => i.Program)
                .FirstOrDefault(s => s.Name == username);  
        }
        public List<Student> GetUnverifiedStudents()
        {
            return db.Students.Where(s => !s.IsVerified).ToList();
        }
        //public List<Student> GetUnverifiedStudentsByHr(int hrId)
        //{
        //    return db.Students.Where(s => !s.IsVerified && s.HrId == hrId).ToList();
        //}
        public List<Student> GetUnverifiedStudentsByHr(int hrId)
        {
            var unverifiedStudents = db.Students
                .Include(s => s.Department)  // Include related Department data
                .Include(s => s.Hr)          // Include related Hr data
                .Where(s => s.HrId == hrId && !s.IsVerified)
                .ToList();

            // Handling potential null values within the retrieved students
            foreach (var student in unverifiedStudents)
            {
                student.Department = student.Department ?? new Department { Name = "N/A" };
                student.Hr = student.Hr ?? new Hr { Name = "N/A" };
            }

            return unverifiedStudents;
        }

        public Student GetStudentById(int id)
        {
            var student = db.Students
                .Include(s => s.Hr)           // Include the Hr navigation property
                .Include(s => s.Department)   // Include the Department navigation property
                .FirstOrDefault(s => s.Id == id);

            return student;
        }
        public Student GetStudentById(int studentId, int hrId)
        {
            // Retrieve the student with the specified studentId
            var student = db.Students
                .Include(s => s.Hr) // Ensure the related Hr entity is loaded
                .Include (s => s.Department)
                .FirstOrDefault(s => s.Id == studentId && s.HrId == hrId);

            return student;
        }



        public void UpdateStudent(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student), "Student cannot be null");
            }

            // Load the existing student from the database to ensure it's tracked by the DbContext
            var existingStudent = db.Students
                                    .Include(s => s.Hr)          // Include the Hr relationship
                                    .Include(s => s.UserLogin)   // Include the UserLogin relationship
                                    .FirstOrDefault(s => s.Id == student.Id);

            if (existingStudent == null)
            {
                throw new InvalidOperationException("Student not found");
            }

            // Update only the fields that need updating
            existingStudent.Name = student.Name ?? existingStudent.Name;
            existingStudent.Age = student.Age ?? existingStudent.Age;
            existingStudent.Email = student.Email ?? existingStudent.Email;
            existingStudent.ImageName = student.ImageName ?? existingStudent.ImageName;
            existingStudent.IsVerified = student.IsVerified;
            existingStudent.DeptNum = student.DeptNum ?? existingStudent.DeptNum;
            existingStudent.HrId = student.HrId ?? existingStudent.HrId;
            existingStudent.UserLoginId = student.UserLoginId;

            // Update the entity in the DbContext
            db.Students.Update(existingStudent);

            // Save the changes to the database
            db.SaveChanges();
        }




    }
}
