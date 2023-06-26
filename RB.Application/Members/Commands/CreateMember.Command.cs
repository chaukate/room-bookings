using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using RB.Domain.Entities;
using System.Text.Json.Serialization;

namespace RB.Application.Members.Commands
{
    public class CreateMemberHandler : IRequestHandler<CreateMemberCommand, int>
    {
        private readonly IRBDbContext _dbContext;
        public CreateMemberHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
        {
            if (await _dbContext.Members.AnyAsync(a => a.Email.ToLower() == request.Email.ToLower(), cancellationToken))
            {
                throw new BadRequestException($"{request.Email} is already registered.");
            }

            var member = new Member
            {
                Email = request.Email,
                Name = request.Name,
                HasAccess = request.HasAccess,
                IsActive = true,
                LastUpdatedBy = request.CurrentUser
            };

            _dbContext.Members.Add(member);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return member.Id;
        }
    }

    public class CreateMemberCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool HasAccess { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
