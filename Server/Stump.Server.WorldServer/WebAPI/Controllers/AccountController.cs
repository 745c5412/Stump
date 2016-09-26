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
                return NotFound();

            return Json(account);
        }

        [HttpPut]
        [Route("Account/{accountId:int}/AddTokens/{amount:int}")]
        public IHttpActionResult AddTokens(int accountId, int amount)
        {
            var account = World.Instance.GetConnectedAccount(accountId);

            if (account == null)
                return NotFound();

            if (!account.ConnectedCharacter.HasValue)
                return NotFound();

            var character = World.Instance.GetCharacter(account.ConnectedCharacter.Value);

            if (character == null)
                return NotFound();

            var tokens = character.Inventory.Tokens;

            if (tokens != null)
            {
                tokens.Stack += (uint)amount;
                character.Inventory.RefreshItem(tokens);
            }
            else
            {
                character.Inventory.CreateTokenItem(amount);
            }

            character.SendServerMessage($"Vous venez de recevoir votre achat de {amount} Ogrines !");

            return Ok();
        }

        public IHttpActionResult Put(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Post(int accountId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);

        public IHttpActionResult Delete(int accountId) => StatusCode(HttpStatusCode.MethodNotAllowed);
    }
}