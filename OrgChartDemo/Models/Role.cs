using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace OrgChartDemo.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public RoleType RoleType { get; set; }
    }
}
