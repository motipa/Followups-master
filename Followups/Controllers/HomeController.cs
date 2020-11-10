﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Followups.Models;
using Microsoft.AspNetCore.Http;
using CustomerFollow.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using ExcelDataReader;
using Followups.Models.DB;
using AutoMapper;

namespace Followups.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;
        private readonly FollowUpDbContext _followupsContext;
        const string SessionName = "_Name";
        const string SessionAge = "_Age";
        public HomeController(ILogger<HomeController> logger, FollowUpDbContext followupsContext, IMapper mapper)
        {
            _logger = logger;
            _followupsContext = followupsContext;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Title = "Admin";
            CustomerResultViewModel _custResult = new CustomerResultViewModel();
            _custResult.ResultCustomer = new List<Customer>();
            _custResult.ResultSalesPerson = new List<Employee>();
            _custResult.Customer = new Customer();
            _custResult.ResultSalesPerson = (from c in _followupsContext.Employee select c).ToList();
            _custResult.ResultSalesPerson.Insert(0, new Employee { Id = 0, Name = "--Select--" });
            ViewBag.SalesPerson = _custResult.ResultSalesPerson;
            //Coutry Details
            _custResult.ResultCountry = new List<Countries>();
            _custResult.ResultCountry = (from d in _followupsContext.Countries select d).ToList();
            _custResult.ResultCountry.Insert(0, new Countries { PhoneCode = 0, Name = "--Select--" });
            ViewBag.Country = _custResult.ResultCountry;
            return View(_custResult);

        }
        [HttpPost]
        public IActionResult Index(CustomerResultViewModel customers, IFormFile file, [FromServices] IHostingEnvironment hostingEnvironment)
        {
            CustomerResultViewModel _custResult = new CustomerResultViewModel();
            _custResult.ResultCustomer = new List<Customer>();
            _custResult.ResultSalesPerson = new List<Employee>();
            _custResult.Customer = new Customer();
            //Sales Persons Details
            _custResult.ResultSalesPerson = (from c in _followupsContext.Employee select c).ToList();
            _custResult.ResultSalesPerson.Insert(0, new Employee { Id = 0, Name = "--Select--" });
            ViewBag.SalesPerson = _custResult.ResultSalesPerson;
            //Coutry Details
            _custResult.ResultCountry = new List<Countries>();
            _custResult.ResultCountry = (from d in _followupsContext.Countries select d).ToList();
            _custResult.ResultCountry.Insert(0, new Countries { PhoneCode = 0, Name = "--Select--" });
            ViewBag.Country = _custResult.ResultCountry;
            //Upload Files
            if (file != null)
            {
                string filename = $"{hostingEnvironment.WebRootPath}\\files\\{ file.FileName}";
                using (FileStream fileStream = System.IO.File.Create(filename))
                {
                    file.CopyTo(fileStream);
                    fileStream.Flush();
                }
                _custResult.ResultCustomer = this.GetCustDetails(file.FileName);
                return View(_custResult);
            }
            //Save Details
            else
            {
                if (customers.ResultCustomer != null)
                {
                    using (var context = new FollowUpDbContext())
                    {
                        foreach (var item in customers.ResultCustomer)
                        {
                            CustomerFollowUp CustResult = _mapper.Map<CustomerFollowUp>(item);
                            context.CustomerFollowUp.Add(CustResult);
                            context.SaveChanges();
                        }
                    }
                   
                }
                //Save Details Mannually
                if (customers.Customer != null)
                {
                    var details = customers.Customer;
                    string d = "1/1/0001 12:00:00 AM";
                    DateTime Defaultdate = Convert.ToDateTime(d);
                    if (customers.Customer.DateOfContact != Defaultdate)
                    {
                        CustomerFollowUp CustResult = _mapper.Map<CustomerFollowUp>(customers.Customer);
                        _followupsContext.CustomerFollowUp.Add(CustResult);
                        _followupsContext.SaveChanges();                       
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                return RedirectToAction("Index");
            }
        }
        private List<Customer> GetCustDetails(string fname)
        {
            //CustomerResultViewModel _custResult = new CustomerResultViewModel();
            List<Customer> _Customer = new List<Customer>();

            var filename = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fname;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    
                    while (reader.Read())
                    {
                        string Phone = reader.GetValue(0).ToString();
                        if (Phone != "Phone")
                        {
                            int Code = Convert.ToInt32(Phone.Substring(0, 2));
                            _Customer.Add(new Customer()
                            {
                                Phone = reader.GetValue(0).ToString(),
                                CountryId = Code

                            }); ;
                        }
                    }
                }
                //_Customer.RemoveAt(0);
            }
            return _Customer;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(UserModel userModel)
        {
            using (var context = new FollowUpDbContext())
            {

                Employee employee = _mapper.Map<Employee>(userModel.Employee);
                User user = _mapper.Map<User>(userModel.User);

                context.Employee.Add(employee);
                context.SaveChanges();
                user.Empid = employee.Id;
                user.Username = employee.Email;
                user.IsActive = true;
                user.IsDelete = false;
                user.Type = userModel.User.type.ToString();
                context.User.Add(user);
                int a = context.SaveChanges();
                if (a > 0)
                {
                    userModel = null;
                }
            }
            ViewBag.Name = HttpContext.Session.GetString(SessionName);
            ViewBag.Title = "User";
            return View();

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
