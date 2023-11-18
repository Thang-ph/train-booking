using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRN211_Project_Group_4.Models;
using System.Diagnostics;

namespace PRN211_Project_Group_4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PRN211Context _context;


        public HomeController(ILogger<HomeController> logger, PRN211Context context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            int? accountId = TempData["AccountId"] as int?;
            if(accountId.HasValue)
            {
                ViewBag.AccountId = accountId.Value;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Account acc, Exception exception)
        {
            try
            {
                Account a = _context.Accounts.FirstOrDefault(x => x.Username == acc.Username );
                if (a == null)
                {
                    throw exception;
                }
                TempData["AccountID"] = a.AccountId;
                a.checkRole(a.Role);
                a.Login();
            }
            catch (Exception e)
            {
                ViewBag.Error = "Login Fail";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Account acc = new Account();
            acc.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Account acc, string Confirm)
        {
            try
            {
               Account check = _context.Accounts.FirstOrDefault(x => x.Username == acc.Username && x.Password == acc.Password);
     
                if(string.IsNullOrEmpty(acc.Username))
                {
                    ViewBag.Mes = "UserName cant be null";
                    return View();
                }

                if(string.IsNullOrEmpty(acc.Password))
                {
                    ViewBag.Mes = "Invalid Password";
                    return View();
                }


                if (check == null)
                {
                    if(acc.Password == Confirm)
                    {
                        _context.Accounts.Add(acc);
                        _context.SaveChanges();
                        acc.checkRole(acc.Role);
                        acc.Login();
                        return RedirectToAction("Login", "Home");
                    }
                    else
                    {
                        ViewBag.Mes = "Confirmation wrong";
                        return View();
                    }
                }
               else
                {
                    ViewBag.Mes = "This user name already exist";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Mes = "?";
                return View();
            }
        }
    }
}