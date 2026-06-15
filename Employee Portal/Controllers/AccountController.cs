using Employee_Portal.Data;
using Employee_Portal.Services;
using Employee_Portal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Employee_Portal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public AccountController(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // If already authenticated, redirect to home
            if (Request.Cookies.ContainsKey("jwt_token"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _context.Admins
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Username == model.Username);

            if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Invalid username or password.");
                return View(model);
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(admin);

            // Store JWT in HttpOnly cookie (secure, not accessible from JS)
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(7)
                    : DateTimeOffset.UtcNow.AddHours(1)
            };

            Response.Cookies.Append("jwt_token", token, cookieOptions);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt_token");
            return RedirectToAction("Login", "Account");
        }
    }
}