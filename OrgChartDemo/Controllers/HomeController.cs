using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrgChartDemo.Models;
using Newtonsoft.Json;

namespace OrgChartDemo.Controllers
{
    /// <summary>
    /// Default Controller, generates orgchart landing page. (Needs to be reassigned)
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class HomeController : Controller
    {
        private IComponentRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="repo">An <see cref="IComponentRepository"/></param>
        public HomeController(IComponentRepository repo) {
            repository = repo;
        }

        /// <summary>
        /// Gets the components. (async, JSON result from the GetOrgChart JQuery Library
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetComponents() 
        {            
            return Json(repository.GetOrgChartComponentsWithoutMembers());
        }

        /// <summary>
        /// /localhost/ root matches here
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View(); 
    }
}