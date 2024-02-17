using GeneralWiki.Application;
using GeneralWiki.Models;
using GeneralWiki.Service;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]

public class UserController(UserDataProvider userDataProvider) : ControllerBase, IUserDataProvider
{
    //1.登录
    [HttpPost]
    public async Task<ActionResult<string>> LoginAsync(string email, string password)
    {
        try
        {
            return Ok(await userDataProvider.LoginAsync(email, password));
        }catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //2.注册
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

    //3.退出登录
    [HttpPost]
    public async Task<ActionResult<string>> QuitAsync()
    {
        try
        {
            return Ok(await userDataProvider.QuitAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //4.注销账号
    [HttpDelete]
    public async Task<ActionResult<string>> LogoutAsync()
    {
        try
        {
            return Ok(await userDataProvider.LogoutAsync());
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //5.利用id查找用户
    [HttpGet]
    public async Task<ActionResult<User>> IdSelectUserAsync(int id)
    {
        try
        {
            return Ok(await userDataProvider.SelectUsersAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //6.通过用户名查找用户
    [HttpGet]
    public async Task<ActionResult<IQueryable<User>>> NameSelectUsersAsync(string name)
    {
        try
        {
            return Ok(await userDataProvider.SelectUsersAsync(name));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //7.修改用户名
    [HttpPost]
    public async Task<ActionResult<string>> NameModifyAsync(string newName)
    {
        try
        {
            return Ok(await userDataProvider.NameModifyAsync(newName));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //8.设置为管理员
    [HttpPost]
    public async Task<ActionResult<string>> SetAdminAsync(int id)
    {
        if (Startup.user.Role is not Role.adminstrator)
        {
            return Unauthorized("Only administrators have permission to delete entries");
        }

        try 
        {
            return Ok(await userDataProvider.SetAdminAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //9.设置为创作者
    [HttpPost]
    public async Task<ActionResult<string>> SetAuthorAsync(int id)
    {
        if (Startup.user.Role is not Role.adminstrator)
        {
            return Unauthorized("Only administrators have permission to delete entries");
        }

        try
        {
            return Ok(await userDataProvider.SetAuthorAsync(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}