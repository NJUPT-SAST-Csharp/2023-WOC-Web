using GeneralWiki.Application;

namespace GeneralWiki.Controllers;

public class UserController(IUserDataProvider userDataProvider)
{
    private readonly IUserDataProvider _userDataProvider = userDataProvider;
}