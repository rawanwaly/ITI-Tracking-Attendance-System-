using Demo1.Service;
using ITI_Attendance.Models;
using ITI_Attendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Security.Claims;
using static Demo1.Service.StudentService;

namespace ITI_Attendance.Controllers
{
    [Authorize]
    public class HrController : Controller
    {
        private readonly IHrService hrService;
        private readonly IUserServices userService;
        private readonly IStudentService studentService;
        public HrController(IHrService _hrService, IUserServices userServices, IStudentService _studentService)
        {
            hrService = _hrService;
            userService = userServices;
            studentService = _studentService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            List<Hr> hrs = hrService.GetAll();
            return View(hrs);
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            Hr hr = hrService.GetById(id);
            return View(hr);
        }
        [Authorize(Roles = "Hr")]
        public IActionResult Profile()
        {
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized();
            var username = usernameClaim.Value;

            var hr = hrService.GetByName(username);

            if (hr == null)
                return NotFound();
            return View(hr);
        }
        [Authorize(Roles = "Hr")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            Hr hr = hrService.GetById(id.Value);
            if (hr == null)
                return NotFound();

            return View(hr);
        }
        [HttpPost]
        public IActionResult Edit(Hr hr, int id)
        {
            hr.Id = id;
            var existingStudent = hrService.ExistingStdEmail(hr, id);

            if (existingStudent != null)
            {
                ModelState.AddModelError("Email", "The email address is already in use");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    hrService.Edit(hr, id);
                    return RedirectToAction("Profile");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes.");
                }
            }

            return View(hr);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit2(int? id)
        {
            if (id == null)
                return BadRequest();
            Hr hr = hrService.GetById(id.Value);
            if (hr == null)
                return NotFound();

            return View(hr);
        }
        [HttpPost]
        public IActionResult Edit2(Hr hr, int id)
        {
            hr.Id = id;
            var existingStudent = hrService.ExistingStdEmail(hr, id);

            if (existingStudent != null)
            {
                ModelState.AddModelError("Email", "The email address is already in use");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    hrService.Edit(hr, id);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes.");
                }
            }
            return View(hr);
        }
        public IActionResult CheckEmailExist(string Email, string name, int age)
        {
            var res = hrService.CheckEmail(Email);
            if (res != null)
                return Json($"Email Not Vaild you can uses {name}{age}@iti.com");
            return Json(true);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            Hr hr = hrService.GetById(id.Value);
            if (hr == null)
                return NotFound();

            return View(hr);
        }

        [HttpPost]
        public IActionResult Delete(int id, int userId)
        {
            Hr hr = hrService.GetById(id);
            userId = hr.UserLoginId;
            if (hr == null)
                return NotFound();
            hrService.Delete(id);
            userService.Delete(userId);

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var hrs = hrService.GetAll();
            ViewBag.Hrs = new SelectList(hrs, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(Hr hr)
        {
            if (!ModelState.IsValid)
            {
                var userLogin = new UserLogin
                {
                    Username = hr.Name,
                    Password = hr.Password
                };
                userService.Add(userLogin);

                hr.UserLoginId = userLogin.Id;

                hrService.Add(hr);

                var userRole = new UserRole
                {
                    UserLoginId = userLogin.Id,
                    RoleId = 2
                };
                userService.Add(userRole);

                return RedirectToAction("Index");
            }

            return View(hr);
        }
        [Authorize(Roles = "Hr")]
        public IActionResult VerifyStudents(int hrId)
        {
            var unverifiedStudents = studentService.GetUnverifiedStudentsByHr(hrId);
            return View(unverifiedStudents);
        }

        [HttpPost]
        [Authorize(Roles = "Hr")]
        public IActionResult VerifyStudents(int studentId, int hrId)
        {
            var student = studentService.GetStudentById(studentId);
            hrId = student.HrId.Value;
            if (student != null)
            {
                // Debugging output
                Console.WriteLine($"Student Id: {student.Id}, HrId from Student: {student.HrId}, Provided HrId: {hrId}");

                if (student.HrId.HasValue && student.HrId.Value == hrId)
                {
                    student.IsVerified = true;
                    studentService.UpdateStudent(student);
                    return RedirectToAction("VerifyStudents", new { hrId = hrId });
                }
                else
                {
                    ModelState.AddModelError("", "HR ID does not match.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Student not found.");
            }

            return RedirectToAction("VerifyStudents", new { hrId = hrId });
        }
        [Authorize(Roles = "Hr")]
        public IActionResult UploadStudents(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = hrService.GetById(id.Value);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadStudents(int id, IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file.");
                return View();
            }

            try
            {
                using (var stream = new MemoryStream())
                {
                    await excelFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                        {
                            var student = new Student
                            {
                                Name = worksheet.Cells[row, 1].Text,
                                Email = worksheet.Cells[row, 2].Text,
                                Age = int.TryParse(worksheet.Cells[row, 3].Text, out var Age) ? (int?)Age : null,
                                Password = worksheet.Cells[row, 4].Text,
                                ImageName = worksheet.Cells[row, 5].Text,
                                DeptNum = int.TryParse(worksheet.Cells[row, 6].Text, out var DeptNum) ? DeptNum : 0,
                                HrId = int.TryParse(worksheet.Cells[row, 7].Text, out var HrId) ? HrId : 0,
                                IsVerified = bool.TryParse(worksheet.Cells[row, 8].Text, out var IsVerified) ? IsVerified : false
                            };

                            // Add student to the database
                            // var x = studentService.GetByUserNPass(student.Name,student.Password);
                            student.CPassword = student.Password;
                            studentService.Add(student);
                            userService.Add(student.Name, student.Password);
                        }
                    }
                }

                return RedirectToAction("Profile", new { id = id }); // Redirect to a success page or display a message
            }
            catch (Exception ex)
            {
                // Handle exception and log it
                ModelState.AddModelError("", $"An error occurred while processing the file: {ex.Message}");
                return View();
            }

        }
    }
}
