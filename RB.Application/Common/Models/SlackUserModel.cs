using System.Text.Json.Serialization;

namespace RB.Application.Common.Models
{
    public class SlackUserModel
    {
        [JsonPropertyName("ok")]
        public bool Ok { get; set; }

        [JsonPropertyName("user")]
        public UserModel User { get; set; }

        public class UserModel
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("real_name")]
            public string RealName { get; set; }

            [JsonPropertyName("profile")]
            public UserProfileModel Profile { get; set; }
        }

        public class UserProfileModel
        {
            [JsonPropertyName("image_original")]
            public string Image { get; set; }
        }
    }
}
