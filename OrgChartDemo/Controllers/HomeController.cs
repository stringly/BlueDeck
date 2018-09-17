using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using OrgChartDemo.Models.ViewModels;
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
            return Json(repository.ChartableComponents.ToArray());
        }



        public IActionResult Index() => View(new OrgChartOptionsViewModel
        {
            Components = repository.Components.OrderBy(p => p.ComponentId).ToList(),
            Members = repository.Members.OrderBy(p => p.IdNumber).ToList(),
        });
        
    }
}