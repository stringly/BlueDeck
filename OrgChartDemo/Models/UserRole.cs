using System.ComponentModel.DataAnnotations;


namespace OrgChartDemo.Models
{
    public class UserRole
    {
        [Key]
        public int RoleId { get; set; }
        public UserRoleType RoleType { get; set; }
    }
}
