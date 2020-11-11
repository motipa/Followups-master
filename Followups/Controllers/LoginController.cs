using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            string Password = MD5Hash(userViewModel.password);
            var user =await _dbcontext.User.Where(x => x.Username == userViewModel.username && x.Password== Password).ToListAsync();
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
        //Password Encrypt
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            //get hash result after compute it  
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }
    }
}
