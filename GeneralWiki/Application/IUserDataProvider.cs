using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IUserDataProvider
{

    public Task<string> LoginAsync(string email, string password);

    //Post:注册
    public Task<string> SignupAsync(string name, string email, string password);

    //Post:退出登录
    public Task<string> QuitAsync();

    //Delete:注销账号
    public Task<string> LogoutAsync();

    //Get:Id查找用户
    public Task<User> IdSelectUserAsync(int id);
    
    //Get:Name查找用户
    public Task<IQueryable<User>> NameSelectUsersAsync(string name);

    //Post:修改自己的用户名
    public Task<string> NameModifyAsync(string newName);

    //Post:设置管理员
    public Task<string> SetAdminAsync(int id);

    //Post:设置创作者
    public Task<string> SetAuthorAsync(int id);
    
  
}
