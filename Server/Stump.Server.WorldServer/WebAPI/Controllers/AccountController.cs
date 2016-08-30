using Stump.Server.WorldServer.Game;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Account/{accountId:int}")]
    public class AccountController : ApiController
    {
        public IHttpActionResult Get(int accountId)
        {
            var account = World.Instance.GetConnectedAccount(accountId);

            if (account == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Json(account);
        }

        [HttpGet]
        [Route("Account/{accountId:int}/Test")]
        public IHttpActionResult Test(int accountId)
        {
            return Json("OK");
        }

        public IHttpActionResult Put(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Post(int accountId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Delete(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);
    }
}