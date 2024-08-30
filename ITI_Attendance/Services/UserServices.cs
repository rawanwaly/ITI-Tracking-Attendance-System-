using Demo1.Service;
using ITI_Attendance.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ITI_Attendance.Services
{
    public class UserServices : IUserServices
    {
        private readonly ITIDbContext db;

        public UserServices(ITIDbContext _db) => db = _db;
        
       // public UserLogin GetUser(string username, string password) => db.UserLogins.FirstOrDefault(u => u.Username == username && u.Password == password);
        
        public string GetUserRole(int userId)
        {
            var roles = db.UserRoles
                          .Where(ur => ur.UserLoginId == userId)
                          .Select(ur => ur.Role.RoleName)
                          .ToList();

            return string.Join(", ", roles);
        }
        public void RegisterUser(UserLogin user)
        {
            db.UserLogins.Add(user);
            db.SaveChanges();

            string roleName = "Student";
            int roleId = 3;
            if (user.Username.EndsWith("AD", System.StringComparison.OrdinalIgnoreCase)) {
                roleName = "Admin";
                roleId = 1;
            }

            AssignUserRole(user.Id, roleName,roleId);

            if (roleName == "Student")
            {
                AddStudent(user);
            }
        }

        public UserLogin GetUser(string username, string password)
        {
            var user = db.UserLogins
                            .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var role = db.UserRoles
                            .Where(ur => ur.UserLoginId == user.Id)
                            .Select(ur => ur.Role.RoleName)
                            .FirstOrDefault();

                if (role == "Student")
                {
                    AddStudent(user);
                }
            }

            return user;
        }

        private void AddStudent(UserLogin user)
        {
            var studentService = new StudentService(db);  
            var existingStudent = db.Students.FirstOrDefault(s => s.Name == user.Username);
            if (existingStudent == null)
            {
                Student student = new Student
                {
                    Name = user.Username,
                    Email = $"{user.Username}@iti.gov",
                    IsVerified = false,
                    Age = null,
                    DeptNum = null,  
                    HrId = null,   
                    //ImageName = $"{user.Username}.jpg"
                };
                studentService.Add(student);
            }
        }

        public void AssignUserRole(int userId, string roleName, int roleId)
        {
            var role = db.Roles.FirstOrDefault(r => r.Id == roleId && r.RoleName == roleName);
            if (role == null)
            {
                role = new Role { Id = roleId, RoleName = roleName };
                db.Roles.Add(role);
                db.SaveChanges();
            }

            var userRole = new UserRole
            {
                UserLoginId = userId,
                RoleId = role.Id
            };
            db.UserRoles.Add(userRole);
            db.SaveChanges();
        }
        //public void RegisterHr(UserLogin hrUser)
        //{
        //    db.UserLogins.Add(hrUser);
        //    db.SaveChanges();

        //    AssignUserRole(hrUser.Id, "HR", 2);
        //}
        public void Add(UserLogin userLogin)
        {
            // Hash the password before saving
            db.UserLogins.Add(userLogin);
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            UserLogin userLogin = db.UserLogins.SingleOrDefault(r => r.Id == id);
            db.UserLogins.Remove(userLogin);
            db.SaveChanges();
        }
        public UserLogin GetById(int Id)
        {
            return db.UserLogins.SingleOrDefault(d => d.Id == Id);    
        }

        public void Add(UserRole userRole)
        {
            // Add the UserRole entity to the DbSet
            db.UserRoles.Add(userRole);
            // Save changes to the database
            db.SaveChanges();
        }
        public void RegisterHr(UserLogin hrUser)
        {
            db.UserLogins.Add(hrUser);
            db.SaveChanges();

            AssignUserRole(hrUser.Id, "HR", 2);

            Hr newHr = new Hr
            {
                Name = hrUser.Username,
                Email = hrUser.Username + "@iti.gov",
                UserLoginId = hrUser.Id
            };
            db.Hrs.Add(newHr);
            db.SaveChanges();
        }

        public void Add(string username, string password)
        {
            UserLogin user = new UserLogin() { Username = username, Password = password };
            db.UserLogins.Add(user);
            db.SaveChanges();

        }
    }
}
