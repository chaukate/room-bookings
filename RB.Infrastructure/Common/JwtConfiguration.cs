namespace RB.Infrastructure.Common
{
    public class JwtConfiguration
    {
        public const string SECTION_NAME = "JWT";
        public string Key { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int RefreshInMinutes { get; set; } = 15;
        public int ExpireInMinutes { get; set; } = 30;
    }
}
