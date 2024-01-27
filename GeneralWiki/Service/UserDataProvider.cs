using GeneralWiki.Application;
using GeneralWiki.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeneralWiki.Service;

public class UserDataProvider:IUserDataProvider
{
    public Task<ActionResult<User>> Register(User user)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult> SetAdmin(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<User>> Login(User user)
    {
        throw new NotImplementedException();
    }
}