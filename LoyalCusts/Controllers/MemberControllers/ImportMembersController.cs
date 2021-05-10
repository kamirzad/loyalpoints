using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoyalCusts.Controllers.MemberControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportMembersController : ControllerBase
    {
        private readonly Common.DbContext _context;
        public ImportMembersController(Common.DbContext context)
        {
            _context = context;
        }


        [HttpPost]
        public ActionResult Post([FromBody] List<Model.Member> members)
        {
            using (var db = _context.Context)
            {
                var memberCollection = db.GetCollection<Model.Member>("Members");
                var result = new Output();
                members.ForEach(newmember =>
                {
                    if (memberCollection.FindAll().Any(x => x.Name == newmember.Name))
                    {
                        result.ExistingRecords.Add(newmember.Name);
                    }
                    else
                    {
                        memberCollection.Insert(newmember);
                        result.NewRecords.Add(newmember.Name);
                    }
                });
               
                return Ok($"{result.NewRecords.Count} members(s) added.\n{result.ExistingRecords.Count} member(s) already exists therefore skipped! ");
            }
        }


        public class Output
        {
            public List<string> NewRecords { get; set; } = new List<string>();
            public List<string> ExistingRecords { get; set; } = new List<string>();

        }
    }
}
