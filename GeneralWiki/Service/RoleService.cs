using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using System.Security.Claims;

namespace GeneralWiki.Service
{
    public class RoleService(WikiContext cts) : IRoleServiceProvider
    {
        //设置为管理员
        public async Task<string> SetAdminAsync(string applyToken)
        {
            //首先要确认Jwt
            var principal = JwtService.ValidateToken(applyToken);
            if(principal is null)
            {
                throw new ArgumentNullException(nameof(applyToken));
            }

            //从含有Claim的ClaimPrincipal中获取用户的Id
            var id = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var toUpdateUser = cts.Users.Single(u => u.Id.ToString() == id);

            if(toUpdateUser is null) throw new ArgumentNullException(nameof(toUpdateUser));

            toUpdateUser.Role = Models.Role.adminstrator;

            await cts.SaveChangesAsync();

            var token = JwtService.GenerateToken(toUpdateUser, JwtService.GenerateSecretKey());
            return token;
        }

        //申请管理员,发送token
        public async Task<string> ApplyAdminAsync()
        {
            string token = JwtService.GenerateToken(Startup.user, JwtService.GenerateSecretKey());
            await Task.Delay(1000);
            return token;
        }

        //取消管理员资格(这个还需要实现吗)
        public Task<string> CancelAdminAsync(int id)
        {
            throw new NotImplementedException();
        }

    }
}
