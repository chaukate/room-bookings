using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using System.Text.Json.Serialization;

namespace RB.Application.Rooms.Commands
{
    public class UpdateRoomHandler : IRequestHandler<UpdateRoomCommand>
    {
        private readonly IRBDbContext _dbContext;
        public UpdateRoomHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var dbRoom = await _dbContext.Rooms.FindAsync(request.Id);
            if (dbRoom == null)
            {
                throw new NotFoundException();
            }

            if (await _dbContext.Rooms.AnyAsync(a => a.Id != request.Id &&
                                                     a.Name.ToLower() == request.Name.ToLower(),
                                                     cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            dbRoom.Name = request.Name;
            dbRoom.Description = request.Description;
            dbRoom.Capacity = request.Capacity;
            dbRoom.IsActive = true;
            dbRoom.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbRoom.LastUpdatedBy = request.CurrentUser;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateRoomCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
