using GeneralWiki.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeneralWiki.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController(IRoleServiceProvider roleService):ControllerBase
    {
        //申请管理员
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<string>> ApplyAdmin()
        {
            var staff = User.FindFirstValue(ClaimTypes.Role);
            if (staff is not "author")
            {
                Unauthorized("Only authors have permission to apply admin");
            }

            try
            {
                return Ok(await roleService.ApplyAdminAsync(User.FindFirstValue(JwtRegisteredClaimNames.Sub)));
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        //设置为管理员
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<string>> SetAdmin(string applyToken)
        {
            var staff = User.FindFirstValue(ClaimTypes.Role);
            if (staff is not "adminstrator")
            {
                Unauthorized("Only adminstrator have permission to set admin");
            }

            try
            {
                return Ok(await roleService.SetAdminAsync(applyToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //取消管理员
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<string>> CancelAdmin(string quitToken)
        {
            var staff = User.FindFirstValue(ClaimTypes.Role);
            if (staff is not "adminstrator")
            {
                Unauthorized("Only adminstrator have permission to cancel admin");
            }

            try
            {
                return Ok(await roleService.SetAdminAsync(quitToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
