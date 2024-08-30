using Demo1.Service;
using ITI_Attendance.Models;
using ITI_Attendance.Services;
using ITI_Attendance.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ITI_Attendance.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        IDepartmentService departmentService;
        IStudentService studentService;
        IHrService hrService;
        IUserServices userService;
        IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<PdfGenerator> _logger;
        //private readonly PdfGenerator _pdfGenerator;
        public StudentController(IStudentService _studentService, IDepartmentService _departmentService , IHrService _hrservice, IUserServices _userService, IWebHostEnvironment _webHostEnvironment, ILogger<PdfGenerator> logger)
        {
            studentService = _studentService;
            departmentService = _departmentService;
            hrService = _hrservice;
            userService = _userService;
            webHostEnvironment = _webHostEnvironment;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        { 
            List<Student> model = studentService.GetAll();
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return BadRequest();
            Student model = studentService.GetById(id.Value);
            if (model == null)
                return NotFound();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var depts = departmentService.GetAll();
            var hrs = hrService.GetAll();
            ViewBag.Departments = new SelectList(depts, "Id", "Name");
            ViewBag.Hrs = new SelectList(hrs, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Student student, IFormFile img)
        {
            if (!ModelState.IsValid)
            {
                //if (student.ImageName != null)
                //{
                //    student.ImageName = $"{student.Id}" + '.' + img.FileName.Split('.').Last();
                //    using (FileStream st = new FileStream($"wwwroot/Image/{student.ImageName}", FileMode.Create))
                //    {
                //        await img.CopyToAsync(st);
                //    }
                //}
                if (img != null )
                {
                    student.ImageName = $"{student.Id}.{img.FileName.Split('.').Last()}";
                    var filePath = Path.Combine("wwwroot", "Image", student.ImageName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                }
                
                var userLogin = new UserLogin
                {
                    Username = student.Name,
                    Password = student.Password 
                };
                userService.Add(userLogin); 

                student.UserLoginId = userLogin.Id;
                studentService.Add(student);

                var userRole = new UserRole
                {
                    UserLoginId = userLogin.Id,
                    RoleId = 3 
                };
                userService.Add(userRole); 

                return RedirectToAction("Index");
            }

            var deptsList = departmentService.GetAll();
            var hrsList = hrService.GetAll();
            ViewBag.Departments = new SelectList(deptsList, "Id", "Name");
            ViewBag.Hrs = new SelectList(hrsList, "Id", "Name");

            return View(student);
        }
        [Authorize(Roles = "Student")]
        public IActionResult Profile()
        {
            var usernameClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim == null)
                return Unauthorized();
            var username = usernameClaim.Value;

            var std = studentService.GetByName(username);
            if (std == null)
                return NotFound();
            return View(std);
        }
        [Authorize(Roles = "Student")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            Student std = studentService.GetById(id.Value);
            if (std == null)
                return NotFound();

            return View(std);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Student std, int id, IFormFile img)
        {
            id = std.Id;
            var existingStudent = studentService.ExistingStdEmail(std, id);
            if (img != null)
            {
                std.ImageName = $"{std.Id}" + '.' + img.FileName.Split('.').Last();
                using (FileStream st = new FileStream($"wwwroot/Image/{std.ImageName}", FileMode.Create))
                {
                    await img.CopyToAsync(st);
                }
            }
            if (existingStudent != null)
            {
                ModelState.AddModelError("Email", "The email address is already in use");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    studentService.Edit(std, id);
                    return RedirectToAction("Profile");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes.");
                }
            }
          
            return View(std);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit2(int? id)
        {
            if (id == null)
                return BadRequest();
            Student std = studentService.GetById(id.Value);
            if (std == null)
                return NotFound();
            var depts = departmentService.GetAll();
            var hrs = hrService.GetAll();
            ViewBag.Departments = new SelectList(depts, "Id", "Name");
            ViewBag.Hrs = new SelectList(hrs, "Id", "Name");
            return View(std);
        }
        [HttpPost]
        public IActionResult Edit2(Student std, int id)
        {
            std.Id = id;
            var existingStudent = studentService.ExistingStdEmail(std, id);

            if (existingStudent != null)
            {
                ModelState.AddModelError("Email", "The email address is already in use");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    studentService.Edit(std, id);
                    return RedirectToAction("index");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving changes.");
                }
            }
            var deptsList = departmentService.GetAll();
            var hrsList = hrService.GetAll();
            ViewBag.Departments = new SelectList(deptsList, "Id", "Name");
            ViewBag.Hrs = new SelectList(hrsList, "Id", "Name");
            return View(std);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return BadRequest();
            Student user = studentService.GetById(id.Value);
            if (user == null)
                return NotFound();

            return View(user);
        }
        [HttpPost]
        public IActionResult Delete(int id,int userLogin)
        {
           Student std = studentService.GetById(id);
            userLogin = std.UserLoginId.Value;
            if (std == null)
                return NotFound();
            studentService.Delete(id);
            userService.Delete(userLogin);

            return RedirectToAction("Index");
        }
        public IActionResult CheckEmailExist(string Email,string name , int age)
        {
            var res = studentService.CheckEmail(Email);
            if (res != null)
                return Json($"Email Not Vaild you can uses {name}{age}@iti.com");
            return Json(true);
        }
        public IActionResult GenerateStudentPdf(int id)
        {
            try
            {
                var pdfGenerator = new PdfGenerator(studentService, webHostEnvironment, _logger);
                pdfGenerator.GenerateStudentCardPdf(id);

                var filePath = Path.Combine(webHostEnvironment.WebRootPath, $"Student_{id}_Card.pdf");

                if (System.IO.File.Exists(filePath))
                {
                    return File(System.IO.File.ReadAllBytes(filePath), "application/pdf", $"Student_{id}_Card.pdf");
                }
                else
                {
                    _logger.LogWarning("The PDF file could not be found.");
                    return NotFound("The PDF file could not be found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating the PDF.");
                return BadRequest("An error occurred while generating the PDF. Please try again later.");
            }
        }

    }
}
