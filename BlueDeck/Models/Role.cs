using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlueDeck.Models.Enums;

namespace BlueDeck.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public int RoleTypeId { get; set; }
        public RoleType RoleType { get; set; }
        public int MemberId { get; set; }
        public virtual Member Member { get; set;}
        
    }
}
