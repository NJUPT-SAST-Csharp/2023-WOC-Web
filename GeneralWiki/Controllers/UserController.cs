using GeneralWiki.Application;

namespace GeneralWiki.Controllers;

public class UserController
{
    private readonly IUserDataProvider _userDataProvider;

    public UserController(IUserDataProvider userDataProvider)
    {
        _userDataProvider = userDataProvider;
    }}