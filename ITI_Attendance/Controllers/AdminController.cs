using ITI_Attendance.Models;
using ITI_Attendance.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ITI_Attendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
       
       private readonly IUserServices userService;

       public AdminController(IUserServices _userService)
       {
           userService = _userService;
       }

       public IActionResult RegisterHr()
       {
           return View();
       }

       [HttpPost]
        public IActionResult RegisterHr(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = userService.GetUser(model.Username, model.Password);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "A user with the same username already exists.");
                    return View(model);
                }

                userService.RegisterHr(model);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

    }
}
