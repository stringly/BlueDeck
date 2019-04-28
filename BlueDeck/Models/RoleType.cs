using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models
{
    public class RoleType
    {
        [Key]
        public int RoleTypeId { get; set; }
        public string RoleTypeName { get; set; }
    }
}
