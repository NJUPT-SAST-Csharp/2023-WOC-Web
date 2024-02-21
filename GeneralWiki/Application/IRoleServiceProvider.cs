namespace GeneralWiki.Application
{
    public interface IRoleServiceProvider
    {
        //Post:设置为管理员
        public Task<string> SetAdminAsync(string ApplyToken);

        //Post:申请管理员权限
        public Task<string> ApplyAdminAsync();

        //Post:取消用户的管理员权限
        public Task<string> CancelAdminAsync(int id);
    }
}
