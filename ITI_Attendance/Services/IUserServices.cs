using ITI_Attendance.Models;
using Newtonsoft.Json.Bson;

namespace ITI_Attendance.Services
{
    public interface IUserServices
    {
        public void RegisterUser(UserLogin user);
        public UserLogin GetUser(string username, string password);
        public string GetUserRole(int userId);
        public void RegisterHr(UserLogin adminUser);
        public void AssignUserRole(int userId, string roleName, int roleId);
        public void Add(UserLogin userLogin);
        public void Delete(int id);
        public UserLogin GetById(int Id);
        public void Add(UserRole userRole);
        public void Add(string username, string password);


    }
}
