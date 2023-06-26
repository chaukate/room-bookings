using MediatR;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;

namespace RB.Application.Rooms.Queries
{
    public class GetRoomHandler : IRequestHandler<GetRoomQuery, GetRoomResponse>
    {
        private readonly IRBDbContext _dbContext;
        public GetRoomHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetRoomResponse> Handle(GetRoomQuery request, CancellationToken cancellationToken)
        {
            var dbRoom = await _dbContext.Rooms.FindAsync(request.Id);
            if (dbRoom == null)
            {
                throw new NotFoundException();
            }

            var response = new GetRoomResponse
            {
                Id = dbRoom.Id,
                Name = dbRoom.Name,
                Description = dbRoom.Description,
                IsActive = dbRoom.IsActive,
                LastUpdatedAt = dbRoom.LastUpdatedAt,
                LastUpdatedBy = dbRoom.LastUpdatedBy
            };

            return response;
        }
    }

    public class GetRoomQuery : IRequest<GetRoomResponse>
    {
        public int Id { get; set; }
    }

    public class GetRoomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset LastUpdatedAt { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
