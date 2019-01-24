using Microsoft.AspNetCore.Mvc.Rendering;
using OrgChartDemo.Models.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.ViewModels
{
    public class RosterManagerViewModel
    {
        [Display(Name = "Choose a Component")]
        public int SelectedComponentId {get;set;}
        public List<ComponentSelectListItem> Components {get; set;}
    }
}
