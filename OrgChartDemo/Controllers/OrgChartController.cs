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
    /// Serves Views and data that renders in <a href="http://www.getorgchart.com/Documentation#separationMixedHierarchyNodes" >GetOrgChart</a> Plugin
    /// </summary>
    /// <seealso cref="T:Microsoft.AspNetCore.Mvc.Controller" />
    public class OrgChartController : Controller
    {
        private IComponentRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:OrgChartDemo.Controllers.OrgChartController"/> class.
        /// </summary>
        /// <param name="repo">An <see cref="T:OrgChartDemo.Models.IComponentRepository"/> injected dependency</param>
        public OrgChartController(IComponentRepository repo) {
            repository = repo;
        }

        /// <summary>
        /// Gets a list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/>s 
        /// </summary>
        /// <returns>a JSON-formatted list of <see cref="T:OrgChartDemo.Models.ChartableComponent"/>s</returns>
        [HttpGet]
        public JsonResult GetComponents() 
        {
            return Json(repository.GetOrgChartComponentsWithoutMembers());
        }

        /// <summary>
        /// GET /OrgChart/
        /// </summary>
        /// <returns>An <see cref="T:IActionResult"/></returns>
        public IActionResult Index() => View();       
        
    }
}