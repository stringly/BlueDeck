using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    public class OrgChartController : Controller
    {
        private IComponentRepository repository;

        public OrgChartController(IComponentRepository repo) {
            repository = repo;
        }

        [HttpGet]
        public JsonResult GetComponents() 
        {
            return Json(repository.ChartableComponents.ToArray());
        }

        public IActionResult Index() => View();

        
        
    }
}