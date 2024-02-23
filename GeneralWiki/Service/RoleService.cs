using GeneralWiki.Application;
using GeneralWiki.Data;
using GeneralWiki.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GeneralWiki.Service
{
    public class RoleService(WikiContext cts) : IRoleServiceProvider
    {
        //设置为管理员
        public async Task<string> SetAdminAsync(string applyToken)
        {
            //解析确认Token
            var principal = JwtService.ValidateToken(applyToken);

            if (principal == null) throw new ArgumentNullException(nameof(principal));

            //找到对应User
            var id = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var toUpdateUser = cts.Users.Single(u => u.Id.ToString() == id);

            if(toUpdateUser is null) throw new ArgumentNullException(nameof(toUpdateUser));

            toUpdateUser.Role = Models.Role.adminstrator;

            await cts.SaveChangesAsync();

            var token = JwtService.GenerateToken(toUpdateUser, new JwtSetting());
            return token;
        }

        //申请管理员,发送token
        public async Task<string> ApplyAdminAsync(string id)
        {
            User user = cts.Users.Single(u => u.Id.ToString() == id);
            string token = JwtService.GenerateToken(user, new JwtSetting());
            await Task.Delay(1000);
            return token;
        }

        //取消管理员资格
        public async Task<string> CancelAdminAsync(string quitToken)
        {
            //解析确认Token
            var principal = JwtService.ValidateToken(quitToken);

            if (principal == null) throw new ArgumentNullException(nameof(principal));

            //找到对应User
            var id = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var toUpdateUser = cts.Users.Single(u => u.Id.ToString() == id);
            if (toUpdateUser is null) throw new ArgumentNullException(nameof(toUpdateUser));


            if (toUpdateUser.Role is not Role.adminstrator) 
                throw new Exception("Only administrator can be cancelled administrator");

            toUpdateUser.Role = Models.Role.author;

            await cts.SaveChangesAsync();

            var token = JwtService.GenerateToken(toUpdateUser, new JwtSetting());
            return token;
        }

    }
}
