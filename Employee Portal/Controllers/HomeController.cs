using Employee_Portal.Data;
using Employee_Portal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Employee_Portal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalEmployees = await _context.Employees.CountAsync();
            ViewBag.TotalDepartments = await _context.Departments.CountAsync();
            ViewBag.MaleCount = await _context.Employees.CountAsync(e => e.Gender == "Male");
            ViewBag.FemaleCount = await _context.Employees.CountAsync(e => e.Gender == "Female");

            var recentEmployees = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Designation)
                .OrderByDescending(e => e.HireDate)
                .Take(5)
                .AsNoTracking()
                .ToListAsync();

            ViewBag.RecentEmployees = recentEmployees;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}