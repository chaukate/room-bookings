using Microsoft.Extensions.Options;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;
using RB.Infrastructure.Common.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace RB.Infrastructure.Services
{
    public class SlackService : ISlackService
    {
        private readonly SlackConfiguration _slackConfiguration;
        public SlackService(IOptions<SlackConfiguration> options)
        {
            _slackConfiguration = options.Value;
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken)
        {
            await ListWorkspaceUsers(cancellationToken);

            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{_slackConfiguration.Instance}chat.postMessage");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var jsonPostContent = System.Text.Json.JsonSerializer.Serialize(new { channel = _slackConfiguration.Channel, text = message });
            postRequest.Content = new StringContent(jsonPostContent, Encoding.UTF8, "application/json");

            var apiResponse = await httpClient.SendAsync(postRequest);
            var responseData = await apiResponse.Content.ReadAsStringAsync();
        }

        public async Task ListWorkspaceUsers(CancellationToken cancellationToken)
        {
            //https://slack.com/api/users.list
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Get, $"{_slackConfiguration.Instance}users.list");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var apiResponse = await httpClient.SendAsync(postRequest);
            var responseData = await apiResponse.Content.ReadFromJsonAsync<SlackUserModel>();
        }
    }
}
