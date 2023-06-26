using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;
using System.Text.Json.Serialization;

namespace RB.Application.Members.Commands
{
    public class UpdateMemberHandler : IRequestHandler<UpdateMemberCommand>
    {
        private readonly IRBDbContext _dbContext;
        public UpdateMemberHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Handle(UpdateMemberCommand request, CancellationToken cancellationToken)
        {
            var dbMember = await _dbContext.Members.FindAsync(request.Id);
            if (dbMember == null)
            {
                throw new NotFoundException();
            }

            if (await _dbContext.Members.AnyAsync(a => a.Id != request.Id &&
                                                       a.Email.ToLower() == request.Email.ToLower(),
                                                       cancellationToken))
            {
                throw new BadRequestException($"{request.Email} is already registered.");
            }

            dbMember.Name = request.Name;
            dbMember.Email = request.Email;
            dbMember.HasAccess = request.HasAccess;
            dbMember.IsActive = true;
            dbMember.LastUpdatedAt = DateTimeOffset.UtcNow;
            dbMember.LastUpdatedBy = request.CurrentUser;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public class UpdateMemberCommand : IRequest
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public bool HasAccess { get; set; }

        [JsonIgnore]
        public string CurrentUser { get; set; }
    }
}
