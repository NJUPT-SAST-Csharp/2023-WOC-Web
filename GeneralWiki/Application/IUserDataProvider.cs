using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IUserDataProvider
{

    public Task<ActionResult<string>> LoginAsync(string email, string password);

    //Post:注册
    public Task<ActionResult<string>> SignupAsync(string name, string email, string password);

    //Post:退出登录
    public Task<ActionResult<string>> QuitAsync();

    //Delete:注销账号
    public Task<ActionResult<string>> LogoutAsync();

    //Get:Id查找用户
    public Task<ActionResult<User>> IdSelectUserAsync(int id);
    
    //Get:Name查找用户
    public Task<ActionResult<IQueryable<User>>> NameSelectUsersAsync(string name);

    //Post:修改自己的用户名
    public Task<ActionResult<string>> NameModifyAsync(string newName);
    //Post:设置管理员
    public Task<ActionResult<string>> SetAdminAsync(int id);

    //Post:设置创作者
    public Task<ActionResult<string>> SetAuthorAsync(int id);
    
    
}
//async和http动词不放在接口中