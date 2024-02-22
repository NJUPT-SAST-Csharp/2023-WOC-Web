using GeneralWiki.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GeneralWiki.Service.DtoService;

namespace GeneralWiki.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController:ControllerBase
    {
        private readonly IEntryDataProvider _entryDataProviderService;
        private readonly IRoleServiceProvider _roleServiceProvider;

       public RoleController(IEntryDataProvider entryDataProviderService, IRoleServiceProvider roleServiceProvider)
        {
            _entryDataProviderService = entryDataProviderService;
            _roleServiceProvider = roleServiceProvider;
        }
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
                return Ok(await _roleServiceProvider.ApplyAdminAsync());
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
                return Ok(await _roleServiceProvider.SetAdminAsync(applyToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // 限制只有作者角色的用户可以访问
        [Authorize(Roles = "Author")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EntryDto entry)
        {
            try
            {
                var createdEntry = await _entryDataProviderService.PostEntry(entry);//返回一个 EntryDto 对象
                return Ok(createdEntry);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        // 限制只有管理员角色的用户可以访问
        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEntry(string title)
        {
            try
            {
                var deleteResult = await _entryDataProviderService.DeleteEntry(title);
                if (!string.IsNullOrEmpty(deleteResult))
                {
                    return Ok($"Article with title '{title}' deleted successfully.");
                }
                else
                {
                    return NotFound($"Article with title '{title}' not found.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
