using ITI_Attendance.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using static Demo1.Service.DepartmentService;

namespace Demo1.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ITIDbContext db;
        public DepartmentService(ITIDbContext _db) => db = _db;
        
        public List<Department> GetAll() => db.Departments.Where(d => d.Active == true).ToList();
        public Department GetById(int id) => db.Departments.SingleOrDefault(d => d.Id == id);
        public void Delete(int id)
        {
            Department dept = db.Departments.SingleOrDefault(d => d.Id == id);
            dept.Active = false;
            db.SaveChanges();
        }
        public void Add(Department dept)
        {
            db.Departments.Add(dept);
                dept.Active = true;
                db.SaveChanges();
           
        }
        public void Edit(Department dept, int id)
        {
            var existingDept = db.Departments.SingleOrDefault(d => d.Id == id);

            if (existingDept != null)
            {
                existingDept.Name = dept.Name;
                existingDept.Capcity = dept.Capcity;

                db.Departments.Update(existingDept);
                db.SaveChanges();
            }
        }
        public Department ShowCrs(int id) => db.Departments.Include(a => a.Programs).FirstOrDefault(a => a.Id == id);
        public Department GetDeptCours(int id) => db.Departments.Include(a => a.Programs).FirstOrDefault(d => d.Id == id);
        public Department GetDeptByName(string name)=> db.Departments.SingleOrDefault(d=>d.Name == name);
        public Department GetStdDept(int dept_Id) => db.Departments.Include(d => d.Students).FirstOrDefault(a => a.Id == dept_Id);
        public void EditDeptCours(Department dept)
        {
            db.Departments.Update(dept);
            db.SaveChanges();
        }
    }
}
