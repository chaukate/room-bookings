namespace RB.Infrastructure.Common
{
    public class AzureAdConfiguration
    {
        public const string CLIENT_SECTION_NAME = "Client";
        public const string ADMIN_SECTION_NAME = "Admin";

        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
