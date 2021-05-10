using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoyalCusts.Controllers.AccountControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedeemPointsController : ControllerBase
    {
        private readonly Common.DbContext _context;
        public RedeemPointsController(Common.DbContext context)
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
                    var account = member.Accounts.FirstOrDefault(x => x.Name == newAccountParams.AccountName && x.Active == true);
                    if (account != null)
                    {
                        if(account.Balance < newAccountParams.PointsToRedeem)
                        {
                            return Conflict("Points are not sufficient!");
                        }
                        account.Balance -= newAccountParams.PointsToRedeem;
                        memberCollection.Update(member);
                        return Ok($"{newAccountParams.PointsToRedeem} point(s) redeemed from {newAccountParams.MemberName}'s '{newAccountParams.AccountName}' account.\nCurrent {newAccountParams.AccountName} balance: {account.Balance}");
                    }
                    return NotFound("Account is either inactive or not found");

                }
                return NotFound("Member not found");
            }
        }


        public class Input
        {
            public string MemberName { get; set; }
            public string AccountName { get; set; }
            public int PointsToRedeem { get; set; }
        }
    }
}
