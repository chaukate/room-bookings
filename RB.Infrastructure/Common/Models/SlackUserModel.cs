using System.Text.Json.Serialization;

namespace RB.Infrastructure.Common.Models
{
    public class SlackUserModel
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("members")]
        public List<Member> Members { get; set; }
    }

    public class Member
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("real_name")]
        public string RealName { get; set; }

        [JsonPropertyName("profile")]
        public Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
