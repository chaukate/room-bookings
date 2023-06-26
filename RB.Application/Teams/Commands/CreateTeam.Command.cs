using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Teams.Commands
{
    public class CreateTeamHandler : IRequestHandler<CreateTeamCommand, int>
    {
        private readonly IRBDbContext _dbContext;
        public CreateTeamHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
            if (await _dbContext.Teams.AnyAsync(a => a.Name.ToLower() == request.Name.ToLower(), cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            var team = new Team
            {
                Name = request.Name,
                Description = request.Description,
                LeadId = request.LeadId,
                IsActive = true,
                LastUpdatedBy = request.CurrentUser
            };

            _dbContext.Teams.Add(team);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return team.Id;
        }
    }

    public class CreateTeamCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
