using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Interfaces;

namespace RB.Application.Members.Queries
{
    public class ListMembersHandler : IRequestHandler<ListMembersQuery, List<ListMembersResponse>>
    {
        private readonly IRBDbContext _dbContext;
        public ListMembersHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListMembersResponse>> Handle(ListMembersQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Members.AsNoTracking()
                                                   .Select(s => new ListMembersResponse
                                                   {
                                                       Id = s.Id,
                                                       Name = s.Name,
                                                       Email = s.Email,
                                                       HasAccess = s.HasAccess,
                                                       IsActive = s.IsActive,
                                                       LastUpdatedAt = s.LastUpdatedAt,
                                                       LastUpdatedBy = s.LastUpdatedBy
                                                   })
                                                   .ToListAsync(cancellationToken);

            return response;
        }
    }

    public class ListMembersQuery : IRequest<List<ListMembersResponse>> { }

    public class ListMembersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool HasAccess { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
