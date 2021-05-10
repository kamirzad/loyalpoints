using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoyalCusts.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAccountController : ControllerBase
    {
        private readonly Common.DbContext _context;
        public AddAccountController(Common.DbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Input newAccountParams)
        {
            using (var db = _context.Context)
            {
                var memberCollection = db.GetCollection<Model.Member>("Members");

                var member = memberCollection.FindOne(x => x.Name.ToLower() == newAccountParams.MemberName.ToLower());

                if(member != null)
                {
                    if(member.Accounts.Any(m => m.Name == newAccountParams.Account.Name))
                    {
                        return Conflict("Specified member already has this loyalty account!");
                    }
                    member.Accounts.Add(newAccountParams.Account);
                    memberCollection.Update(member);
                    return Ok();
                }
                return NotFound("Member not found");
            }
        }


        public class Input
        {
            public string MemberName { get; set; }
            public Model.Account Account { get; set; }
        }
    }
}
