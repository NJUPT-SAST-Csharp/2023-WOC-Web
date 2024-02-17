using GeneralWiki.Models;

namespace GeneralWiki
{
    public class Startup
    {
        //这个类中的账号对象表示你进入wiki客户端的登录状态，就是启动时候的初始状态
        //这里使用了单例的思想，整个进程结束，这个用户状态就会被释放
        public static User user = new() { Role = Role.tourist };
    }
}
