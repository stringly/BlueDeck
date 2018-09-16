using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OrgChartDemo.Models.Types {
    public class MemberRank {
        [Key]
        public int RankId { get; set; }
        public string RankFullName { get; set; }
        public string RankShort { get; set; }
        public string PayGrade { get; set; }
    }
}
