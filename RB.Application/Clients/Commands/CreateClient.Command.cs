using MediatR;
using RB.Application.Common.Helpers;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Clients.Commands
{
    public class CreateClientHandler : IRequestHandler<CreateClientCommand, int>
    {
        private readonly IRBDbContext _dbContext;
        public CreateClientHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            var dbClient = new Client
            {
                Name = request.Name,
                AdminEmail = request.AdminEmail,
                SecretKey = StringHelper.RandomString(100),
                LastUpdatedBy = request.CurrentUser
            };

            _dbContext.Clients.Add(dbClient);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return dbClient.Id;
        }
    }

    public class CreateClientCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string AdminEmail { get; set; }
        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
