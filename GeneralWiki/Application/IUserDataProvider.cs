using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IUserDataProvider
{
    //Post:登录
    public Task<string> LoginAsync(string email, string password);

    //Post:注册
    public Task<string> SignupAsync(string name, string email, string password);

    //Delete:注销账号
    public Task<string> LogoutAsync(string id);

    //Get:Id查找用户
    public Task<User> IdSelectUserAsync(string id);
    
    //Get:Name查找用户
    public Task<IQueryable<User>> NameSelectUsersAsync(string name);

    //Post:修改自己的用户名
    public Task<string> NameModifyAsync(string newName, string id);

}
