using System;
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

namespace Followups.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FollowUpDbContext _followupsContext;
        const string SessionName = "_Name";
        const string SessionAge = "_Age";
        public HomeController(ILogger<HomeController> logger, FollowUpDbContext followupsContext)
        {
            _logger = logger;
            _followupsContext = followupsContext;
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
            _custResult.ResultCountry.Insert(0, new Countries { Id = 0, Name = "--Select--" });
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
            
            _custResult.ResultSalesPerson = (from c in _followupsContext.Employee select c).ToList();
            _custResult.ResultSalesPerson.Insert(0, new Employee { Id = 0, Name = "--Select--" });
            // _customer.FirstOrDefault().SalesEmployee = cl;
            ViewBag.SalesPerson = _custResult.ResultSalesPerson;
            //Coutry Details
            _custResult.ResultCountry = new List<Countries>();
            _custResult.ResultCountry = (from d in _followupsContext.Countries select d).ToList();
            _custResult.ResultCountry.Insert(0, new Countries { Id = 0, Name = "--Select--" });           
            ViewBag.Country = _custResult.ResultCountry;

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
            else
            {
                if (customers.ResultCustomer != null)
                {
                    foreach (var item in customers.ResultCustomer)
                    {
                        CustomerFollowUp CustResult = new CustomerFollowUp();
                        CustResult.CountryId = int.Parse(item.Country);
                        CustResult.CustomerInterest = item.CustInterest;
                        CustResult.Phone = item.Phone;
                        CustResult.DateOfContact = item.ContactDate;
                        CustResult.Idstatus = item.IdStatus;
                        CustResult.SalesPersonId = int.Parse(item.SalesPerson);
                        _followupsContext.Add(CustResult);
                        _followupsContext.SaveChanges();
                    }
                }
                if (customers.Customer != null)
                {                    
                    var details = customers.Customer;
                    string d = "1/1/0001 12:00:00 AM";
                    DateTime Defaultdate = Convert.ToDateTime(d);
                    if (customers.Customer.ContactDate != Defaultdate)
                    {
                        CustomerFollowUp CustResult = new CustomerFollowUp();
                        CustResult.Phone = details.Phone;
                        CustResult.CountryId = int.Parse(details.Country);
                        CustResult.CustomerInterest = details.CustInterest;

                        CustResult.DateOfContact = details.ContactDate;
                        CustResult.Idstatus = details.IdStatus;
                        CustResult.SalesPersonId = int.Parse(details.SalesPerson);
                        _followupsContext.Add(CustResult);
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
            List<Customer>  _Customer = new List<Customer>();
          
            var filename = $"{Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + fname;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        _Customer.Add(new Customer()
                        {
                            Phone = reader.GetValue(0).ToString()
                        });
                    }
                }
                _Customer.RemoveAt(0);
            }
            return _Customer;
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
    }
}
