using System.Text.Json.Serialization;

namespace RB.Application.Common.Models
{
    public class SlackMembersModel
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("members")]
        public List<MemberModel> Members { get; set; }
    }

    public class MemberModel
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("real_name")]
        public string RealName { get; set; }

        [JsonPropertyName("profile")]
        public MemberProfileModel Profile { get; set; }
    }

    public class MemberProfileModel
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
