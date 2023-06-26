using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Rooms.Commands
{
    public class CreateRoomHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IRBDbContext _dbContext;
        public CreateRoomHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            if (await _dbContext.Rooms.AnyAsync(a => a.Name.ToLower() == request.Name.ToLower(), cancellationToken))
            {
                throw new BadRequestException($"{request.Name} is already registered.");
            }

            var room = new Room
            {
                Name = request.Name,
                Description = request.Description,
                Capacity = request.Capacity,
                IsActive = true,
                LastUpdatedBy = request.CurrentUser
            };

            _dbContext.Rooms.Add(room);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return room.Id;
        }
    }

    public class CreateRoomCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Capacity { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
