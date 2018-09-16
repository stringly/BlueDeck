﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using OrgChartDemo.Models.Types;

namespace OrgChartDemo.Models {
    public class Member {
        [Key]
        public int MemberId { get; set; }
        public MemberRank Rank { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string IdNumber { get; set; }
        public string Email {get; set; }
        public string GetTitleName() => $"{this.Rank.RankShort}. {this.FirstName} {this.LastName} #{this.IdNumber}";        
        public string GetLastNameFirstName() => $"{this.LastName}, {this.FirstName}";
    }
}

