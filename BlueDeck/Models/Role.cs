using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BlueDeck.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public RoleType RoleType { get; set; }
    }
}
