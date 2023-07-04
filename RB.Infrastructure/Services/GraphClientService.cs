using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using RB.Application.Interfaces;
using RB.Infrastructure.Common.Configurations;

namespace RB.Infrastructure.Services
{
    public class GraphClientService : IGraphService
    {
        private readonly AzureAdConfiguration _clientConfiguration;
        public GraphClientService(IOptionsSnapshot<AzureAdConfiguration> azureAdOptions)
        {
            _clientConfiguration = azureAdOptions.Get(AzureAdConfiguration.CLIENT_SECTION_NAME);
        }

        public async Task CreateCalendarEventAsync(CancellationToken cancellationToken)
        {
            var graphClient = GetGraphClient("");

            var result = await graphClient.Me.Calendar.Events.PostAsync(new Microsoft.Graph.Models.Event(), cancellationToken: cancellationToken);
        }



        public GraphServiceClient GetGraphClient(string tenantId)
        {
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, _clientConfiguration.ClientId, _clientConfiguration.ClientSecret,
                new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                });

            var graphClient = new GraphServiceClient(clientSecretCredential);
            return graphClient;
        }
    }
}
