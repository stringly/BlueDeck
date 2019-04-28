using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlueDeck.Models
{
    public class AppStatus
    {
        [Key]
        [Display(Name = "Status Id")]
        public int? AppStatusId {get;set;}
        [Display(Name = "Status Name")]
        [Required]
        public string StatusName {get;set;}

        public virtual IEnumerable<Member> Members { get; set; }
        
    }
}
