using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Service;

public class UserDataProvider(WikiContext cts): IUserDataProvider
{

    //1.��¼�����䣬ƥ�������Ƿ����
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

    //2.ע��һ���µ��˺�
    //����Ҫ���������Ƿ��Ѿ�ע�ᣬ�Ǿ�Ҫ��ʾ�޷�ע��
    //��Ҫ���û����Ƿ��Ѿ����ڣ������޷�ע��
    public bool EmailExistOrDefault(string email) => cts.Users.Any(u => u.Email == email);
    public bool NameExistOrDefault(string name) => cts.Users.Any(u => u.Name == name);
    public async Task<string> SignupAsync(string name, string email, string password)
    {
        if (EmailExistOrDefault(email)) throw new Exception("The email has signed up");

        if (NameExistOrDefault(name)) throw new Exception("The name cannot access,because it is existing");

        Startup.user.Name = name;
        Startup.user.Email = email;
        Startup.user.Password = password;
        Startup.user.Role = Role.consumer; //ע���¼֮�����Id�˺�Ȩ�޴��οͱ����ͨ�û�

        cts.Users.Add(Startup.user);
        await cts.SaveChangesAsync();
        await Task.Delay(1000);

        return "Signup success";
    }

    //3.ע���˺�,�ο�״̬�޷�ע��
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

    //4.�˳���¼
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

    //5.ͨ��id�����û�
    public async Task<User> IdSelectUserAsync(int id)
    {
        User? user = cts.Users.SingleOrDefault(x => x.Id == id);
        if (user is null) throw new Exception();
        return await Task.FromResult(user);
    }

    //6.�����û��������û��������ض�����������ַ��������ݣ�����������
    public async Task<IQueryable<User>> NameSelectUsersAsync(string name)
    {
        var users = cts.Users.Where(x => x.Name != null && x.Name.Contains(name)).OrderBy(x => x.Name);
        if (users.Count() is 0) throw new Exception();

        return await Task.FromResult(users);
    }

    //7.�û�����
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
            // �����û������ڵ����
            throw new Exception("The user is not existing");
        }

        // �޸��û�����
        userToUpdate.Name = newName;
        Startup.user.Name = newName;

        // �������
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