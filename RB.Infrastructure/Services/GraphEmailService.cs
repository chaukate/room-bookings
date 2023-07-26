using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RB.Infrastructure.Services
{
    public class GraphEmailService : IGraphEmailService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly GraphConfiguration _graphConfiguration;
        public GraphEmailService(IHttpClientFactory httpClient,
                                 ITokenAcquisition tokenAcquisition,
                                 IOptions<GraphConfiguration> options)
        {
            _httpClient = httpClient.CreateClient();
            _tokenAcquisition = tokenAcquisition;
            _graphConfiguration = options.Value;
        }

        public async Task SendMailAsync(string recipent, string subject, StringBuilder content, CancellationToken cancellationToken)
        {
            var message = new Message
            {
                Body = new ItemBody { Content = content.ToString(), ContentType = BodyType.Html },
                Subject = subject,
                ToRecipients = new List<Recipient> { new Recipient { EmailAddress = new EmailAddress { Address = recipent } } }
            };

            await SendMailAsync(message, cancellationToken);
        }

        private async Task SendMailAsync(Message message, CancellationToken cancellationToken, bool isSaveToSaveItems = false)
        {
            var token = await _tokenAcquisition.GetAccessTokenForUserAsync(new string[] { ".default" });
            // make API call
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_graphConfiguration.Instance}me/sendMail");
            request.Headers.Accept.Clear();
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var jsonContent = JsonSerializer.Serialize(new { message, isSaveToSaveItems });
            request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var apiResponse = await _httpClient.SendAsync(request, cancellationToken);
            var data = await apiResponse.Content.ReadAsStringAsync();
            if (!apiResponse.StatusCode.Equals(HttpStatusCode.Accepted))
            {
                throw new Exception("Error sending mail.");
            }
        }
    }
}
