using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Service;

public class UserDataProvider(WikiContext cts): IUserDataProvider
{

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = cts.Users.SingleOrDefault(u => u.Email == email);

        if (user is null) throw new Exception("The email has not signed up");

        if (user.Password != password)
        {
            throw new Exception("Password error");
        }
        await Task.Delay(1000);

        var token = JwtService.GenerateToken(user, new JwtSetting());

        return token;
    }

    public bool EmailExistOrDefault(string email) => cts.Users.Any(u => u.Email == email);
    public bool NameExistOrDefault(string name) => cts.Users.Any(u => u.Name == name);
    public async Task<string> SignupAsync(string name, string email, string password)
    {
        if (EmailExistOrDefault(email)) throw new Exception("The email has signed up");

        if (NameExistOrDefault(name)) throw new Exception("The name cannot access,because it is existing");

        var user = new User()
        {
            Name = name,
            Email = email,
            Password = password,
            Role = Role.author
        };
        cts.Users.Add(user);
        await cts.SaveChangesAsync();

        return "Signup success";
    }

    public async Task<string> LogoutAsync(string id)
    {

        User user = cts.Users.Single(u => u.Id.ToString() == id);
        cts.Users.Remove(user);
        await cts.SaveChangesAsync();

        var token = JwtService.GenerateToken(user, new JwtSetting());

        return token;
    }


    public async Task<User> IdSelectUserAsync(string id)
    {
        User? user = cts.Users.SingleOrDefault(x => x.Id.ToString() == id);
        if (user is null) throw new Exception("There is no result");
        return await Task.FromResult(user);
    }

    public async Task<IEnumerable<User>> NameSelectUsersAsync(string name)
    {
        var users = cts.Users.Where(x => x.Name != null && x.Name.Contains(name)).OrderBy(x => x.Name).ToArray();
        if (users.Count() is 0) throw new Exception("There are no results");

        return await Task.FromResult(users);
    }

    public async Task<string> NameModifyAsync(string newName, string id)
    {
        User user = cts.Users.Single(u => u.Id.ToString() == id);
        if (user.Role is Role.tourist)
        {
            throw new Exception("Please log in");
        }

        if (cts.Users.Any(u => u.Name == newName))
        {
            throw new Exception("The name cannot access, because it is existing");
        }

        user.Name = newName;

        await cts.SaveChangesAsync();

        var token = JwtService.GenerateToken(user, new JwtSetting());
        return token;

    }
}