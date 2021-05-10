using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoyalCusts.Controllers.MemberControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddMemberController : ControllerBase
    {

        private readonly Common.DbContext _context;
        public AddMemberController(Common.DbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Model.Member member)
        {
            using (var db = _context.Context)
            {
                var memberCollection = db.GetCollection<Model.Member>("Members");
                if (memberCollection.FindAll().Any(x => x.Name == member.Name))
                {
                    return Conflict("Member already exists");
                }

                memberCollection.Insert(member);
                return Ok(member);
            }
        }
    }
}
