using System;
using System.Web.Http;
using System.Threading.Tasks;
using BLL.DTO;
using BLL.Contracts;
using Microsoft.AspNet.Identity;

namespace WebApi.Controllers
{
    [Authorize(Roles = "moderator,admin")]
    [RoutePrefix("api/moder")]    
    public class ModeratorController : ApiController
    {
        private readonly IBLLUnitOfWork _uow;

        public ModeratorController(IBLLUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        [Route("block/refractory/{refId}")]
        public async Task<IHttpActionResult> BlockRefractory([FromUri]int refId)
        {

            DTORefractory refractory = await _uow.RefractoryService.GetRefractoryByRefId(refId);
            if (refractory == null)
                return NotFound();

            if (refractory.IsBlocked)
                return BadRequest("Refractory already blocked.");

            var userName = User.Identity.GetUserName();

            if (userName == null || !User.IsInRole("admin"))
                return this.Unauthorized();
            if (userName == null || !User.IsInRole("moderator"))
                return this.Unauthorized();

            await _uow.ModeratorService.BlockRefractory(refId, userName);

            return Ok("Refractory blocked");
        }

        [HttpGet]
        [Route("unblock/refractory/{refId}")]
        public async Task<IHttpActionResult> UnblockRefractory([FromUri]int refId)
        {
            DTORefractory refractory = await _uow.RefractoryService.GetRefractoryByRefId(refId);
            if (refractory == null)
                return NotFound();

            if (!refractory.IsBlocked)
                return BadRequest("Refractory not blocked.");

            var userId = this.User.Identity.GetUserId();
            if (userId == null || !User.IsInRole("admin"))
                return this.Unauthorized();


            await _uow.ModeratorService.UnblockRefractory(refId);
            return Ok("Refractory unblocked");
        }
    }
}