using Azure.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Models;
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
        private readonly ITokenAcquisition _tokenAcquisition;
        public GraphEmailService(ITokenAcquisition tokenAcquisition)
        {
            _tokenAcquisition = tokenAcquisition;
        }

        public async Task SendMailAsync(string recipent, string subject, StringBuilder content, CancellationToken cancellationToken)
        {
            var message = new Message
            {
                Body = new ItemBody { Content = content.ToString(), ContentType = BodyType.Html },
                Subject = subject,
                ToRecipients = new List<Recipient> { new Recipient { EmailAddress = new EmailAddress { Address = recipent } } }
            };

            await SendMailAsync(message, true, cancellationToken);
        }

        private async Task SendMailAsync(Message message, bool saveToSentItems, CancellationToken cancellationToken)
        {
            // TODO : Use Token Acquirer TokenCredential (TokenAcquirerTokenCredential)
            var credential = new TokenAcquisitionTokenCredential(_tokenAcquisition);
            var client = new GraphServiceClient(credential);

            var requestBody = new Microsoft.Graph.Me.SendMail.SendMailPostRequestBody { Message = message, SaveToSentItems = saveToSentItems };

            try
            {
                await client.Me.SendMail.PostAsync(requestBody, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
