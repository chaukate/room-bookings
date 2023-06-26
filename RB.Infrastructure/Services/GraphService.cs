using Microsoft.Graph;
using RB.Application.Interfaces;

namespace RB.Infrastructure.Services
{
    public class GraphService : IGraphService
    {
        public async Task CreateCalendarEventAsync(CancellationToken cancellationToken)
        {
            var graphClient = CreateGraphClient("");

            var result = await graphClient.Me.Calendar.Events.PostAsync(new Microsoft.Graph.Models.Event(), cancellationToken: cancellationToken);
        }

        public GraphServiceClient CreateGraphClient(string tenantId)
        {
            //var graphClient = new GraphServiceClient(null);

            return null;
        }
    }
}
