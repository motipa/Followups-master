using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerFollow.Models;
using Followups.Models;
using Followups.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Followups.Controllers
{
    public class UserController : Controller
    {
        const string SessionName = "_Name";
        private readonly ILogger<HomeController> _logger;
        private readonly FollowUpDbContext _followupsContext;
        public UserController(ILogger<HomeController> logger, FollowUpDbContext followupsContext)
        {
            _logger = logger;
            _followupsContext = followupsContext;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Title = "User";
            UserResultViewModel _userResultViewModel = new UserResultViewModel();
            _userResultViewModel.SearchDate = new SearchModel();
            _userResultViewModel.UserResultviewCustomer = new List<CustomerFollowUp>();            
            return View(_userResultViewModel);
        }
        [HttpPost]

        public IActionResult Search(UserResultViewModel Search)
        {
            UserResultViewModel _userResultViewModel = new UserResultViewModel();
            _userResultViewModel.UserResultviewCustomer = _followupsContext.CustomerFollowUp.Where(x => x.DateOfContact >= Search.SearchDate.FromDate && x.DateOfContact >= Search.SearchDate.ToDate).ToList();
            TempData["DetailsView"] = _userResultViewModel;
            return RedirectToAction("Index", "User");
        }
    }
}
