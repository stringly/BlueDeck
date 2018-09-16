using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models {
    public class Position {
        [Key]
        public int PostionId {get;set;}
        public Component ParentComponent {get;set;}
        public string Name {get; set;}
        public bool IsUnique {get;set;} = false;
        public string JobTitle {get;set;}
        public List<Member> Members {get;set;}
    }
}
