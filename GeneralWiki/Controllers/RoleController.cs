﻿using GeneralWiki.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<string>> ApplyAdminAsync()
        {
            var staff = User.FindFirstValue(ClaimTypes.Role);
            if (staff is not "author")
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
        [Authorize]
        public async Task<ActionResult<string>> SetAdminAsync(string applyToken)
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
    }
}
