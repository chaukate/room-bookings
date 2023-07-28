using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Identity.Web;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;

namespace RB.Infrastructure.Services
{
    public class GraphAdminService : IGraphAdminService
    {
        private readonly AzureAdConfiguration _adminConfiguration;
        private readonly GraphConfiguration _graphConfiguration;
        private readonly ITokenAcquisition _tokenAcquisition;
        public GraphAdminService(IOptionsSnapshot<AzureAdConfiguration> optionsSnapshot,
                                 IOptions<GraphConfiguration> options,
                                 ITokenAcquisition tokenAcquisition)
        {
            _adminConfiguration = optionsSnapshot.Get(AzureAdConfiguration.ADMIN_SECTION_NAME);
            _graphConfiguration = options.Value;
            _tokenAcquisition = tokenAcquisition;
        }

        private async Task SendMail(Message message, bool saveToSentItems)
        {
            // TODO : Use Token Acquirer TokenCredential (TokenAcquirerTokenCredential)
            var credential = new TokenAcquisitionTokenCredential(_tokenAcquisition);
            var client = new GraphServiceClient(credential);

            var requestBody = new Microsoft.Graph.Me.SendMail.SendMailPostRequestBody { Message = message, SaveToSentItems = saveToSentItems };

            await client.Me.SendMail.PostAsync(requestBody);
        }

        private GraphServiceClient GetGraphClient()
        {
            var clientSecretCredential = new ClientSecretCredential(_adminConfiguration.TenantId,
                                                                    _adminConfiguration.ClientId,
                                                                    _adminConfiguration.ClientSecret,
                                                                    new TokenCredentialOptions
                                                                    {
                                                                        AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                                                                    });

            var graphClient = new GraphServiceClient(clientSecretCredential);
            return graphClient;
        }

        public async Task SendAdminEmailAsync(string recipent,
                                              string subject,
                                              string content,
                                              CancellationToken cancellationToken)
        {
            var graphClient = GetGraphClient();

            var message = new Message
            {
                Body = new ItemBody { Content = content, ContentType = BodyType.Html },
                Subject = subject,
                ToRecipients = new List<Recipient> { new Recipient { EmailAddress = new EmailAddress { Address = recipent } } }
            };

            var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true
            };

            await graphClient.Users[_graphConfiguration.AdminEmailId]
                             .SendMail
                             .PostAsync(requestBody,
                                        cancellationToken: cancellationToken);
        }
    }
}
