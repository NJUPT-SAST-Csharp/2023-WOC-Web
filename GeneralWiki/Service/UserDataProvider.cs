using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Service;

public class UserDataProvider(WikiContext cts): IUserDataProvider
{
    //1.登录用邮箱，匹配密码是否相等
    public async Task<string> LoginAsync(string email, string password)
    {
        var users = cts.Users.Where(u => u.Email == email);

        if (users.Count() is 0) throw new Exception("The email has not signed up");

        if (!users.Any(u => u.Email == email && u.Password == password))
        {
            throw new Exception("Password error");
        }
        Startup.user = users.Single();
        await Task.Delay(1000);

       
        return "Login success";
    }

    //2.注册一个新的账号
    //首先要查找邮箱是否已经注册，是就要提示无法注册
    //还要查用户名是否已经存在，存在无法注册
    public bool EmailExistOrDefault(string email) => cts.Users.Any(u => u.Email == email);
    public bool NameExistOrDefault(string name) => cts.Users.Any(u => u.Name == name);
    public async Task<string> SignupAsync(string name, string email, string password)
    {
        if (EmailExistOrDefault(email)) throw new Exception("The email has signed up");

        if (NameExistOrDefault(name)) throw new Exception("The name cannot access,because it is existing");

        Startup.user.Name = name;
        Startup.user.Email = email;
        Startup.user.Password = password;
        Startup.user.Role = Role.consumer; //注册登录之后这个Id账号权限从游客变成普通用户

        cts.Users.Add(Startup.user);
        await cts.SaveChangesAsync();
        await Task.Delay(1000);

        return "Signup success";
    }

    //3.注销账号,游客状态无法注销
    public async Task<string> LogoutAsync()
    {
        if (Startup.user.Role is Role.tourist)
        {
            throw new Exception("You have not logged in");
        }

        cts.Users.Remove(Startup.user);
        await cts.SaveChangesAsync();

        Startup.user.Name = string.Empty;
        Startup.user.Email = string.Empty;
        Startup.user.Password = string.Empty;
        Startup.user.Role = Role.tourist;

        await Task.Delay(1000);

        return "Logout success";
    }

    //4.退出登录
    public async Task<string> QuitAsync()
    {
        if (Startup.user.Role is Role.tourist)
        {
            throw new Exception("You have not logged in");
        }

        Startup.user.Name = string.Empty;
        Startup.user.Email = string.Empty;
        Startup.user.Password = string.Empty;
        Startup.user.Role = Role.tourist;

        await Task.Delay(1000);

        return "Quit success";
    }

    //5.通过id查找用户
    public async Task<User> IdSelectUserAsync(int id)
    {
        User? user = cts.Users.SingleOrDefault(x => x.Id == id);
        if (user is null) throw new Exception();
        return await Task.FromResult(user);
    }

    //6.利用用户名查找用户，并返回多个包含查找字符串的数据，并进行排序
    public async Task<IQueryable<User>> NameSelectUsersAsync(string name)
    {
        var users = cts.Users.Where(x => x.Name != null && x.Name.Contains(name)).OrderBy(x => x.Name);
        if (users.Count() is 0) throw new Exception();

        return await Task.FromResult(users);
    }

    //7.用户改名
    public async Task<string> NameModifyAsync(string newName)
    {
        if (Startup.user.Role is Role.tourist)
        {
            throw new Exception("Please log in");
        }

        if (cts.Users.Any(u => u.Name == newName))
        {
            throw new Exception("The name cannot access, because it is existing");
        }

        User? userToUpdate = cts.Users.SingleOrDefault(u => u.Id == Startup.user.Id);

        if (userToUpdate is null)
        {
            // 处理用户不存在的情况
            throw new Exception("The user is not existing");
        }

        // 修改用户属性
        userToUpdate.Name = newName;
        Startup.user.Name = newName;

        // 保存更改
        await cts.SaveChangesAsync();
        await Task.Delay(1000);

        return "Modify success";

    }

    public async Task<string> SetAdminAsync(int id)
    {
        User? user = cts.Users.Single(u => u.Id == id);
        if (user is null) throw new Exception("The id is not existing");

        user.Role = Role.adminstrator;
        await Task.Delay(1000);
        return "Already become adminstrator";
    }

    public async Task<string> SetAuthorAsync(int id)
    {
        User? user = cts.Users.Single(u => u.Id == id);
        if (user is null) throw new Exception("The id is not existing");

        user.Role = Role.author;
        await Task.Delay(1000);
        return "Already become author";
    }

}