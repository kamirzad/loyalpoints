using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoyalCusts.Model
{
    public class Member
    {
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<Account> Accounts { get; set; } = new List<Account>();
    }
}
