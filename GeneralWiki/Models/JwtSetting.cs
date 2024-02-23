namespace GeneralWiki.Models
{
    public class JwtSetting
    {
        public string Key { get; set; } = "FJahTzaUS+8q9ryQ8S8806jG7t2Us1g7PYsLdbOnE6I=";
        public string Issuer { get; set; } = "YourIssuer";
        public string Audience { get; set; } = "YourAudience";
        public int AccessExpiration { get; set; } = 1;//AccessToken过期时间（小时）
    }
}
