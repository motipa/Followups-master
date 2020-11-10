using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Followups.Models;
using Followups.Models.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Followups.Controllers
{
    public class ReportsController : Controller
    {
        private FollowUpDbContext Context { get; }

      public  ReportsController(FollowUpDbContext context)
        {
           this.Context = context;
        }
        public IActionResult Index()
        {
            return View(new DateSearchModel());
        }

      
        [HttpPost]
        public IActionResult AjaxMethod(DateSearchModel dateSearc)
        {
            var fromdate = Convert.ToDateTime(dateSearc.fromDate);
            var todate = Convert.ToDateTime(dateSearc.toDate);
            var customers = this.Context.FollowupView.Where(x => x.DateOfContact >= fromdate && x.DateOfContact <= todate).ToList();

            return Json(JsonConvert.SerializeObject(customers));
          
        }
    }
}
