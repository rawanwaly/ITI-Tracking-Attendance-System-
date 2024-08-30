using Demo1.Service;
using ITI_Attendance.Models;
using ITI_Attendance.Services;
using ITI_Attendance.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static Demo1.Service.DepartmentService;

namespace Demo1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        IDepartmentService departmentService;
        IProgramService programService;
        IStudentProgramIntakeService studentProgramIntakeService;
        public DepartmentController(IDepartmentService _departmentService , IProgramService _programService , IStudentProgramIntakeService _studentProgramIntakeService)
        {
            departmentService = _departmentService;   
            programService = _programService;
            studentProgramIntakeService = _studentProgramIntakeService;
        }
        public IActionResult Index()
        {
            List<Department> model = departmentService.GetAll();
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            Department model = departmentService.GetById(id.Value);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [HttpPost]
        public IActionResult Create(Department dept)
        { 
            if (ModelState.IsValid)
            {
                var existingDept = departmentService.GetDeptByName(dept.Name);

                if (existingDept == null)
                {
                    departmentService.Add(dept);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Dept_Name", "The department name is already in use.");
                }
            }
            return View(dept);
        
        }
        public IActionResult Create()
        {
            return View();
        }
     
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            Department dept = departmentService.GetById(id.Value);
            if (dept == null)
                return NotFound();
            return View(dept);
        }
        [HttpPost]
        public IActionResult Edit(Department dept, int id)
        {
            if (ModelState.IsValid)
            {
                var existingDept = departmentService.GetDeptByName(dept.Name);

                if (existingDept == null || existingDept.Id == id)
                {
                    departmentService.Edit(dept, id);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Dept_Name", "The department name is already in use.");
                }
            }
            return View(dept);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            departmentService.Delete(id.Value);

            return RedirectToAction("Index");
        }
        public IActionResult ShowCourses(int id)
        {
            var model = departmentService.ShowCrs(id);
            return View(model);
        }
        public IActionResult ManageCourses(int id)
        {
            var AllCrs = programService.GetAll();
            var Dept = departmentService.GetDeptCours(id);
            var CourseNotInDept = AllCrs.Except(Dept.Programs);
            ViewBag.CoursesInDept = Dept.Programs;
            ViewBag.CoursesNotInDept = CourseNotInDept;
            return View();
        }
        [HttpPost]
        public IActionResult ManageCourses(int id, List<int>CourseToRemove,List<int> CourseToAdd)
        {

            var dept = departmentService.GetDeptCours(id);
            foreach (int i in CourseToRemove)
            {
                var crs = programService.GetById(i);
                dept.Programs.Remove(crs);
            }
            foreach (int i in CourseToAdd)
            {
                var crs = programService.GetById(i);
                dept.Programs.Add(crs);
            }
            departmentService.EditDeptCours(dept);
            return RedirectToAction("ShowCourses", "department", new { id = id});
        }
        //public IActionResult AddDegree(int dept_Id, int crs_Id)
        //{
        //    var dept = departmentService.GetStdDept(dept_Id);
        //    var crs = programService.GetById(crs_Id);
        //    var degrees = studentProgramIntakeService.GetDegree(crs_Id);

        //    var model = new AddDegreeViewModel
        //    {
        //        Department = dept,
        //        Program = crs,
        //        Degrees = degrees
        //    };
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult AddDegree(int dept_Id, int crs_Id, Dictionary<int, int> deg)
        {
            foreach (var i in deg)
            {
                var res = studentProgramIntakeService.GetStudentCourse(crs_Id, i.Key);
                if (res == null)
                {
                    studentProgramIntakeService.Add(new StudentProgramIntake()
                    {
                        ProgramId = crs_Id,
                        StudentId = i.Key,
                        Degree = i.Value
                    });
                }
                else
                {
                    res.Degree = i.Value;
                    studentProgramIntakeService.Update(res);

                }
            }

            return RedirectToAction("ShowDegrees", new { dept_Id = dept_Id, crs_Id = crs_Id });
        }
        public IActionResult AddDegree(int dept_Id, int crs_Id)
        {
            var dept = departmentService.GetStdDept(dept_Id);
            var crs = programService.GetById(crs_Id);
            var studentProgramIntakes = studentProgramIntakeService.GetDegree(crs_Id,dept_Id);

            var degrees = studentProgramIntakeService.GetDegree(crs_Id,dept_Id);

            var model = new AddDegreeViewModel
            {
                Department = dept,
                Program = crs,
                Degrees = degrees
            };

            return View(model);
        }
        public IActionResult ShowDegrees(int dept_Id, int crs_Id)
        {
            var degrees = studentProgramIntakeService.GetDegree(crs_Id, dept_Id);
            var course = programService.GetById(crs_Id);
            var department = departmentService.GetById(dept_Id);
            if (course == null || department == null)
                return NotFound();

            ViewBag.Course = course;
            ViewBag.Department = department;

            return View(degrees);
        }

    }
}
