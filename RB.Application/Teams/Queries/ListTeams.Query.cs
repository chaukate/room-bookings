using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Interfaces;

namespace RB.Application.Teams.Queries
{
    public class ListTeamsHandler : IRequestHandler<ListTeamsQuery, List<ListTeamsResponse>>
    {
        private readonly IRBDbContext _dbContext;
        public ListTeamsHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListTeamsResponse>> Handle(ListTeamsQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Teams.Include(i => i.Lead)
                                                 .Select(s => new ListTeamsResponse
                                                 {
                                                     Id = s.Id,
                                                     Name = s.Name,
                                                     LeadName = s.Lead.Name,
                                                     IsActive = s.IsActive,
                                                     LastUpdatedAt = s.LastUpdatedAt,
                                                     LastUpdatedBy = s.LastUpdatedBy,
                                                 })
                                                 .ToListAsync(cancellationToken);

            return response;
        }
    }

    public class ListTeamsQuery : IRequest<List<ListTeamsResponse>> { }

    public class ListTeamsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LeadName { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
