using Stump.Server.WorldServer.Game;
using System.Net;
using System.Web.Http;

namespace Stump.Server.WorldServer.WebAPI.Controllers
{
    [CustomAuthorize]
    [Route("Character/{characterId:int}")]
    public class CharacterController : ApiController
    {
        public IHttpActionResult Get(int characterId)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

            return Json(character);
        }

        [HttpPut]
        [Route("Character/{characterId:int}/AddTokens/{amount:int}")]
        public IHttpActionResult AddTokens(int characterId, int amount)
        {
            var character = World.Instance.GetCharacter(characterId);

            if (character == null)
                return StatusCode(HttpStatusCode.BadRequest);

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

            return Json(character);
        }

        public IHttpActionResult Put(int characterId) => StatusCode(HttpStatusCode.MethodNotAllowed);
        public IHttpActionResult Post(int characterId, string value) => StatusCode(HttpStatusCode.MethodNotAllowed);
        public IHttpActionResult Delete(int characterId) => StatusCode(HttpStatusCode.MethodNotAllowed);
    }
}
