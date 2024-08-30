using Demo1.Service;
using ITI_Attendance.Models;
using ITI_Attendance.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ITI_Attendance.Controllers
{
    public class StudentAttandanceController : Controller
    {    
        IStudentAttendeceService _studentAttendeceService;
        IStudentService studentService;

        public StudentAttandanceController(IStudentAttendeceService studentAttendeceService, IStudentService _studentService)
        {
            _studentAttendeceService = studentAttendeceService;
            studentService = _studentService;
        }

        public IActionResult Index(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var model = studentService.GetById(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            ViewBag.std = model;
            return View();
        }

        [HttpPost]
        public IActionResult Index(StudentAttendance model)
        {
            if (ModelState.IsValid)
            {
                model.Saturday = false;
                model.Sunday = false;
                model.Monday = false;
                model.Tuesday = false;
                model.Wednesday = false;
                model.Thursday = false;
                model.Friday = false;

                switch (model.SelectedDay)
                {
                    case "Saturday":
                        model.Saturday = true;
                        break;
                    case "Sunday":
                        model.Sunday = true;
                        break;
                    case "Monday":
                        model.Monday = true;
                        break;
                    case "Tuesday":
                        model.Tuesday = true;
                        break;
                    case "Wednesday":
                        model.Wednesday = true;
                        break;
                    case "Thursday":
                        model.Thursday = true;
                        break;
                    case "Friday":
                        model.Friday = true;
                        break;
                }

                _studentAttendeceService.Update(model);

                return RedirectToAction("Profile", "student", new { id = model.Id });
            }

            return View(model);
        }

    }
}
