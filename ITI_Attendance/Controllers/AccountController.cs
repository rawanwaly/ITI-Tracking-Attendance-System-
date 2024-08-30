using ITI_Attendance.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ITI_Attendance.Services;
using System.Threading.Tasks;

namespace ITI_Attendance.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices userService;

        public AccountController(IUserServices _userService) => userService = _userService;
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = userService.GetUser(username, password);
                if (user != null)
                {
                    var role = userService.GetUserRole(user.Id);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(claimsPrincipal);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                }
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserLogin model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = userService.GetUser(model.Username, model.Password);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "A user with the same username and password already exists.");
                    return View(model);
                }
                userService.RegisterUser(model);
                return RedirectToAction("Login");
            }

            return View(model);
        }
    }
}
