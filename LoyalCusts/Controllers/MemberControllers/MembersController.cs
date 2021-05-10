using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;


namespace LoyalCusts.Controllers.MemberControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {

        private readonly Common.DbContext _context;
        public MembersController(Common.DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("All")]
        public IEnumerable<Model.Member> Get()
        {
            using (var db = _context.Context)
            {
                var memberCollection = db.GetCollection<Model.Member>("Members");
                return memberCollection.FindAll().ToList();
            }
        }

        [HttpGet]
        [Route("filter")]
        public IEnumerable<Model.Member> Get([FromBody] FilterParam filterParam)
        {
            using (var db = _context.Context)
            {
                var memberCollection = db.GetCollection<Model.Member>("Members");
                var filteredMembers = memberCollection.FindAll().ToList();
                
                //TODO: refactor filter logic and use bsonexpressions instead

                if (filterParam.IsActive != null)
                {
                    filteredMembers = filteredMembers.Where(m => m.Accounts.Any(a => a.Active == (bool)filterParam.IsActive)).ToList();
                }
                if (filterParam.Balance != null)
                {
                    switch (filterParam.Operator)
                    {
                        case ">":
                            filteredMembers = filteredMembers.Where(m => m.Accounts.Any(a => a.Balance > filterParam.Balance)).ToList();
                            break;
                        case "<":
                            filteredMembers = filteredMembers.Where(m => m.Accounts.Any(a => a.Balance < filterParam.Balance)).ToList();
                            break;
                        default:
                            filteredMembers = filteredMembers.Where(m => m.Accounts.Any(a => a.Balance == filterParam.Balance)).ToList();
                            break;

                    }
                }

                return filteredMembers;
            }
        }


        public class FilterParam
        {
            public bool? IsActive { get; set; }
            public int? Balance { get; set; }
            public string Operator { get; set; } = "=";
        }
    }
}
