using Microsoft.Extensions.Options;
using RB.Application.Common.Models;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;
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

        public async Task InviteMemberToChannelAsync(string slackUserId, CancellationToken cancellationToken)
        {
            //https://slack.com/api/conversations.invite
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{_slackConfiguration.Instance}conversations.invite");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var jsonPostContent = System.Text.Json.JsonSerializer.Serialize(new { channel = _slackConfiguration.Channel, users = slackUserId, is_archived = false });
            postRequest.Content = new StringContent(jsonPostContent, Encoding.UTF8, "application/json");

            var apiResponse = await httpClient.SendAsync(postRequest, cancellationToken);
            var responseData = await apiResponse.Content.ReadAsStringAsync(cancellationToken);

            if (responseData.Contains("\"is_archived\":true"))
            {
                await UnArchiveChannelAsync(cancellationToken);
                await InviteMemberToChannelAsync(slackUserId, cancellationToken);
            }
        }

        public async Task RemoveMemberFromChannelAsync(string slackUserId, CancellationToken cancellationToken)
        {
            //https://slack.com/api/conversations.kick
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{_slackConfiguration.Instance}conversations.kick");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var jsonPostContent = System.Text.Json.JsonSerializer.Serialize(new { channel = _slackConfiguration.Channel, user = slackUserId });
            postRequest.Content = new StringContent(jsonPostContent, Encoding.UTF8, "application/json");

            var apiResponse = await httpClient.SendAsync(postRequest, cancellationToken);
            var responseData = await apiResponse.Content.ReadAsStringAsync(cancellationToken);
        }

        public async Task<SlackUserModel> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            //https://slack.com/api/users.lookupByEmail
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Get, $"{_slackConfiguration.Instance}users.lookupByEmail?email={email}");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var apiResponse = await httpClient.SendAsync(postRequest, cancellationToken);
            var responseData = await apiResponse.Content.ReadFromJsonAsync<SlackUserModel>(cancellationToken: cancellationToken);
            return responseData;
        }

        public async Task<SlackMembersModel> ListWorkspaceUsers(CancellationToken cancellationToken)
        {
            //https://slack.com/api/users.list
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Get, $"{_slackConfiguration.Instance}users.list?email=sshrestha@devfinity.io");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var apiResponse = await httpClient.SendAsync(postRequest, cancellationToken);
            var responseData = await apiResponse.Content.ReadFromJsonAsync<SlackMembersModel>(cancellationToken: cancellationToken);
            return responseData;
        }

        public async Task UnArchiveChannelAsync(CancellationToken cancellationToken)
        {
            //https://slack.com/api/conversations.unarchive
            var httpClient = new HttpClient();
            var postRequest = new HttpRequestMessage(HttpMethod.Post, $"{_slackConfiguration.Instance}conversations.unarchive");
            postRequest.Headers.Accept.Clear();
            postRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _slackConfiguration.Token);

            var jsonPostContent = System.Text.Json.JsonSerializer.Serialize(new { channel = _slackConfiguration.Channel });
            postRequest.Content = new StringContent(jsonPostContent, Encoding.UTF8, "application/json");

            var apiResponse = await httpClient.SendAsync(postRequest, cancellationToken);
            var responseData = await apiResponse.Content.ReadAsStringAsync(cancellationToken);
        }
    }
}
