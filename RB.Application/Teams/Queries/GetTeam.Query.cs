using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;

namespace RB.Application.Teams.Queries
{
    public class GetTeamHandler : MediatR.IRequestHandler<GetTeamQuery, GetTeamResponse>
    {
        private readonly IRBDbContext _dbContext;
        public GetTeamHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetTeamResponse> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Teams.Include(i => i.Lead)
                                                 .Where(w => w.Id == request.Id)
                                                 .Select(s => new GetTeamResponse
                                                 {
                                                     Id = s.Id,
                                                     Name = s.Name,
                                                     Description = s.Description,
                                                     LeadId = s.LeadId,
                                                     LeadName = s.Lead.Name,
                                                     IsActive = s.IsActive,
                                                     LastUpdatedAt = s.LastUpdatedAt,
                                                     LastUpdatedBy = s.LastUpdatedBy
                                                 })
                                                 .FirstOrDefaultAsync(cancellationToken);

            return response ?? throw new NotFoundException();
        }
    }

    public class GetTeamQuery : IRequest<GetTeamResponse>
    {
        public int Id { get; set; }
    }

    public class GetTeamResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int LeadId { get; set; }
        public string LeadName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
