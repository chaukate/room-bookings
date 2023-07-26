using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;

namespace RB.Infrastructure.Services
{
    public class GraphAdminService : IGraphAdminService
    {
        private readonly AzureAdConfiguration _adminConfiguration;
        private readonly GraphConfiguration _graphConfiguration;
        public GraphAdminService(IOptionsSnapshot<AzureAdConfiguration> optionsSnapshot,
                                 IOptions<GraphConfiguration> options)
        {
            _adminConfiguration = optionsSnapshot.Get(AzureAdConfiguration.ADMIN_SECTION_NAME);
            _graphConfiguration = options.Value;
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

            await graphClient.Users[_graphConfiguration.AdminEmailId].SendMail(message)
                                                                     .Request()
                                                                     .PostAsync(cancellationToken);
        }
    }
}
