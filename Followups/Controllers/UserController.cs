using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerFollow.Models;
using Followups.Models;
using Followups.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Followups.Controllers
{
    public class UserController : Controller
    {
        const string SessionName = "_Name";
        const string SessionId = "_Id";
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
            _userResultViewModel.CountryResult = new List<Countries>();
            _userResultViewModel.ResultSalesPerson = new List<Employee>();

            return View(_userResultViewModel);
        }
        [HttpPost]
        public IActionResult Index(UserResultViewModel UserReult)
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Title = "User";
            UserResultViewModel _userResultViewModels = new UserResultViewModel();
            _userResultViewModels.SearchDate = new SearchModel();
            //Get Customer Details
            _userResultViewModels.UserResultviewCustomer = new List<CustomerFollowUp>();
            int SalesPersonId = Convert.ToInt32(HttpContext.Session.GetString(SessionId));
            //int SalesPersonId = 2;
            string Curr_date = System.DateTime.Now.ToShortDateString();
            DateTime Cur_date = Convert.ToDateTime(Curr_date);
            if (UserReult.SearchDate != null)
            {
                _userResultViewModels.UserResultviewCustomer = _followupsContext.CustomerFollowUp.Where(x => x.DateOfContact >= UserReult.SearchDate.FromDate && x.DateOfContact <= UserReult.SearchDate.ToDate || x.CreateDate>= Cur_date && x.SalesPersonId==SalesPersonId).ToList();
                _userResultViewModels.CountryResult = new List<Countries>();
                _userResultViewModels.CountryResult = (from d in _followupsContext.Countries select d).ToList();
                _userResultViewModels.CountryResult.Insert(0, new Countries { Id = 0, Name = "--Select--" });
                ViewBag.Country = _userResultViewModels.CountryResult;
                //Bind Sales Person
                _userResultViewModels.ResultSalesPerson = new List<Employee>();
                _userResultViewModels.ResultSalesPerson = (from c in _followupsContext.Employee select c).ToList();
                _userResultViewModels.ResultSalesPerson.Insert(0, new Employee { Id = 0, Name = "--Select--" });
                ViewBag.SalesPerson = _userResultViewModels.ResultSalesPerson;
            }
            else
            {
                if (UserReult.UserResultviewCustomer != null)
                {
                    foreach (var item in UserReult.UserResultviewCustomer)
                    {
                        CustomerFollowUp objCust = new CustomerFollowUp();
                        objCust = _followupsContext.CustomerFollowUp.Find(item.FollowId);
                        objCust.CountryId = item.CountryId;
                        objCust.CustomerInterest = item.CustomerInterest;
                        objCust.DateOfContact = item.DateOfContact;
                        objCust.Idstatus = item.Idstatus;
                        objCust.Phone = objCust.Phone;
                        objCust.SalesPersonId = objCust.SalesPersonId;
                        string d = DateTime.Now.ToShortDateString();
                        DateTime cur_date = Convert.ToDateTime(d);
                        objCust.ModifiedDate = cur_date;
                        _followupsContext.Entry(objCust).State = EntityState.Modified;
                        _followupsContext.SaveChanges();
                    }
                }
            }
            //Bind Country

            return View(_userResultViewModels);
        }

    }
}
