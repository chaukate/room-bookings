using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Interfaces;

namespace RB.Application.Rooms.Queries
{
    public class ListRoomsHandler : IRequestHandler<ListRoomsQuery, List<ListRoomsResponse>>
    {
        private readonly IRBDbContext _dbContext;
        public ListRoomsHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListRoomsResponse>> Handle(ListRoomsQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Rooms
                                           .Select(s => new ListRoomsResponse
                                           {
                                               Id = s.Id,
                                               Name = s.Name,
                                               IsActive = s.IsActive,
                                               LastUpdatedAt = s.LastUpdatedAt,
                                               LastUpdatedBy = s.LastUpdatedBy
                                           })
                                           .ToListAsync(cancellationToken);

            return response;
        }
    }

    public class ListRoomsQuery : IRequest<List<ListRoomsResponse>> { }

    public class ListRoomsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
