namespace RB.Infrastructure.Common.Configurations
{
    public class SlackConfiguration
    {
        public const string SECTION_NAME = "Slack";
        public string Instance { get; set; }
        public string Token { get; set; }
        public string Channel { get; set; }
    }
}
