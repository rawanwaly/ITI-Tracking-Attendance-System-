
using ITI_Attendance.Models;

namespace Demo1.Service
{
    public interface IDepartmentService
    {
        public List<Department> GetAll();
        public Department GetDeptCours(int id);
        public void Add(Department department);
        Department GetDeptByName(string name);
        public void Edit(Department department,int id);
        public void Delete(int id);
        public Department GetById(int id);
        public Department ShowCrs(int id);
        public Department GetStdDept(int dept_Id);
        public void EditDeptCours(Department dept);



    }
}
