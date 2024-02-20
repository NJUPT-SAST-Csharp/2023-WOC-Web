using GeneralWiki.Application;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController(IRoleServiceProvider roleService):ControllerBase
    {
        //申请管理员
        [HttpPost]
        public async Task<ActionResult<string>> ApplyAdminAsync()
        {
            if (Startup.user.Role is not Models.Role.author)
            {
                Unauthorized("Only authors have permission to apply admin");
            }

            try
            {
                return Ok(await roleService.ApplyAdminAsync());
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        //设置为管理员
        [HttpPost]
        public async Task<ActionResult<string>> SetAdminAsync(string applyToken)
        {
            if (Startup.user.Role is not Models.Role.adminstrator)
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
    }
}
