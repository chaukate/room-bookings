namespace RB.Infrastructure.Common
{
    public class CorsConfiguration
    {
        public const string SECTION_NAME = "CORS";
        public string[] AllowedOrigins { get; set; }
        public string[] ExposedHeaders { get; set; }
    }
}
