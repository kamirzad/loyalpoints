using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoyalCusts.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddPointsToAccountController : ControllerBase
    {
        private readonly Common.DbContext _context;
        public AddPointsToAccountController(Common.DbContext context)
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
                if (member != null)
                {
                    var account = member.Accounts.FirstOrDefault(x => x.Name == newAccountParams.AccountName);
                    if (account != null)
                    {
                        account.Balance += newAccountParams.PointsToAdd;
                        memberCollection.Update(member);
                        return Ok($"{newAccountParams.PointsToAdd} point(s) added to {newAccountParams.MemberName}'s '{newAccountParams.AccountName}' account.\nCurrent {newAccountParams.AccountName} balance: {account.Balance}");
                    }
                    return NotFound("Account not found");

                }
                return NotFound("Member not found");
            }
        }


        public class Input
        {
            public string MemberName { get; set; }
            public string AccountName { get; set; }
            public int PointsToAdd { get; set; }
        }
    }
}
