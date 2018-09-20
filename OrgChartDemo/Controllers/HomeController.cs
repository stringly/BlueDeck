using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    public class HomeController : Controller
    {
        private IComponentRepository repository;

        public HomeController(IComponentRepository repo) {
            repository = repo;
        }

        [HttpGet]
        public JsonResult GetComponents() 
        {            
            return Json(repository.GetOrgChartComponentsWithoutMembers());
        }

        public IActionResult Index() => View();

        
        
    }
}