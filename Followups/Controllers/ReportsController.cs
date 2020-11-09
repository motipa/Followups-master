using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Followups.Models.DB;
using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

      
        [HttpPost]
        public IActionResult AjaxMethod()
        {
            var customers = (from customer in this.Context.FollowupView
                             select customer).ToList();

            return Json(JsonConvert.SerializeObject(customers));
          
        }
    }
}
