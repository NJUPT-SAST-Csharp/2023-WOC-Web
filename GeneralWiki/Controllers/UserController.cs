using GeneralWiki.Application;
using GeneralWiki.Models;
using GeneralWiki.Service;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class UserController(IUserDataProvider userDataProvider) : ControllerBase
{
    //Post:登录
    [HttpPost]
    public async Task<ActionResult<string>> LoginAsync(string email, string password)
    {
        try
        {
            return Ok(await userDataProvider.LoginAsync(email, password));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Post:注册
    [HttpPost]
    public async Task<ActionResult<string>> SignupAsync(string name, string email, string password)
    {
        try
        {
            return Ok(await userDataProvider.SignupAsync(name, email, password));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Post:退出登录
    [HttpPost]
    public async Task<ActionResult<string>> QuitAsync()
    {
        var staff = Startup.user.Role;
        if (staff is Role.tourist) return Unauthorized("Only administrators have permission to quit");

        try
        {
            return Ok(await userDataProvider.QuitAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Delete:注销账号
    [HttpDelete]
    public async Task<ActionResult<string>> LogoutAsync()
    {
        var staff = Startup.user.Role;
        if (staff is Role.tourist) return Unauthorized("Only administrators have permission to log out");
        try
        {
            return Ok(await userDataProvider.LogoutAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //Get:Id查找用户
    [HttpGet]
    public async Task<ActionResult<User>> IdSelectUserAsync(int id)
    {
        try
        {
            return Ok(await userDataProvider.IdSelectUserAsync(id));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    //Get:Name查找用户
    [HttpGet]
    public async Task<ActionResult<IQueryable<User>>> NameSelectUsersAsync(string name)
    {
        try
        {
            return Ok(await userDataProvider.NameSelectUsersAsync(name));
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    //Post:修改自己的用户名
    [HttpPost]
    public async Task<ActionResult<string>> NameModifyAsync(string newName)
    {
        var staff = Startup.user.Role;
        if (staff is Role.tourist) return Unauthorized("Only administrators have permission to modify name");

        try
        {
            return Ok(await userDataProvider.NameModifyAsync(newName));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}