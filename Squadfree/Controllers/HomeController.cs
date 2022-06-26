using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Squadfree.Data;
using Squadfree.Models;

namespace Squadfree.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ApplicationDbContext _context { get; set; }

        public HomeController(ApplicationDbContext context,ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_context.Testimonials);
        }

        
    }
}
