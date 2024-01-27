using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Application;

public interface IUserDataProvider
{
    //Post:注册
    public Task<ActionResult<User>> Register(User user);
    
    //Post:设置管理员
    public Task<ActionResult> SetAdmin(int id);
    
    //Post:登录
    public Task<ActionResult<User>> Login(User user);
}
//async和http动词不放在接口中