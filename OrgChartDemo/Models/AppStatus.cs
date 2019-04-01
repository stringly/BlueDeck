using System.ComponentModel.DataAnnotations;

namespace OrgChartDemo.Models
{
    public class AppStatus
    {
        [Key]
        public int AppStatusId {get;set;}
        public string StatusName {get;set;}
        
    }
}
