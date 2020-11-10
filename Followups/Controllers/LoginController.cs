using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Followups.Models;
using Followups.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Followups.Controllers
{
    public class LoginController : Controller
    {
        private readonly FollowUpDbContext _dbcontext;
        const string SessionName = "_Name";
        const string SessionId = "_Id";
        const string SessionAge = "_Age";
        public LoginController(FollowUpDbContext followupsContext)
        {
            _dbcontext = followupsContext;
        }
        public IActionResult Index()
        {
          
            return View(new UserViewModel());
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]

        public async Task<IActionResult> IndexAsync(UserViewModel userViewModel)
        {

            var user =await _dbcontext.User.Where(x => x.Username == userViewModel.username && x.Password==userViewModel.password).ToListAsync();

            if(user.Count<=0)
            {
                ViewBag.Message = "Invalid login";
                return View(userViewModel);
            }

            if(userViewModel.type.ToString()=="admin" && user.FirstOrDefault().Type.Trim()=="admin")
            {
                HttpContext.Session.SetString(SessionName, user.FirstOrDefault().Username);
                HttpContext.Session.SetInt32(SessionId, user.FirstOrDefault().Id);
                return RedirectToAction("Index","Home");
            }
            if (userViewModel.type.ToString() == "user" && user.FirstOrDefault().Type.Trim() == "user")
            {
                HttpContext.Session.SetString(SessionName, user.FirstOrDefault().Username);                
                HttpContext.Session.SetString(SessionId, user.FirstOrDefault().Empid.ToString());
                //TempData["PersonId"] = user.FirstOrDefault().Empid.ToString();                
                return RedirectToAction("Index","User");
            }
            ViewBag.Message = "Invalid login";

            return View(userViewModel);
        }
    }
}
