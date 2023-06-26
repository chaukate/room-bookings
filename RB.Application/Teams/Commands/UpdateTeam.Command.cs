using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using System.Text.Json.Serialization;

namespace RB.Application.Teams.Commands
{
    public class UpdateTeamHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly IRBDbContext _dbContext;
        public UpdateTeamHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var dbTeam = await _dbContext.Teams.FindAsync(request.Id);
            if (dbTeam == null)
            {
                throw new NotFoundException();
            }

            if (await _dbContext.Teams.AnyAsync(a => a.Id != request.Id &&
                                                     a.Name.ToLower() == request.Name.ToLower(),
                                                     cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            dbTeam.Name = request.Name;
            dbTeam.Description = request.Description;
            dbTeam.LeadId = request.LeadId;
            dbTeam.IsActive = true;
            dbTeam.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbTeam.LastUpdatedBy = request.CurrentUser;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateTeamCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
