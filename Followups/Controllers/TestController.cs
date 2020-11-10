using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Followups.Models;
using Followups.Models.DB;
using Microsoft.AspNetCore.Mvc;

namespace Followups.Controllers
{
    public class TestController : Controller
    {
        private readonly FollowUpDbContext _dbcontext;
        private readonly IMapper _mapper;
        public TestController(FollowUpDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var result = _dbcontext.Employee.ToList();
        
            return View(result);
        }
    }
}
