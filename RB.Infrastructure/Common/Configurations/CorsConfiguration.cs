namespace RB.Infrastructure.Common.Configurations
{
    public class CorsConfiguration
    {
        public const string SECTION_NAME = "CORS";
        public string[] AllowedOrigins { get; set; }
        public string[] ExposedHeaders { get; set; }
    }
}
