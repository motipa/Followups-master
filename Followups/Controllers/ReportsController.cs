﻿using System;
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
        public async Task<IActionResult> ReportResultAsync(string d1,string d2)
        {
            DateTime d3 = Convert.ToDateTime(d1);
            DateTime d4 = Convert.ToDateTime(d2);
            var customers =await this.Context.FollowupView.Where(x => x.DateOfContact >= d3 && x.DateOfContact <= d4).ToListAsync();
            return Json(JsonConvert.SerializeObject(customers));          
        }
    }
}
