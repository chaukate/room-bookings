using MediatR;
using RB.Application.Common.Exceptions;
using RB.Application.Interfaces;

namespace RB.Application.Members.Queries
{
    public class GetMemberHandler : IRequestHandler<GetMemberQuery, GetMemberResponse>
    {
        private readonly IRBDbContext _dbContext;
        public GetMemberHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetMemberResponse> Handle(GetMemberQuery request, CancellationToken cancellationToken)
        {
            var dbMember = await _dbContext.Members.FindAsync(request.Id);
            if (dbMember == null)
            {
                throw new NotFoundException();
            }

            var response = new GetMemberResponse
            {
                Id = dbMember.Id,
                Name = dbMember.Name,
                Email = dbMember.Email,
                HasAccess = dbMember.HasAccess,
                IsActive = dbMember.IsActive,
                LastUpdatedAt = dbMember.LastUpdatedAt,
                LastUpdatedBy = dbMember.LastUpdatedBy
            };

            return response;
        }
    }

    public class GetMemberQuery : IRequest<GetMemberResponse>
    {
        public int Id { get; set; }
    }

    public class GetMemberResponse
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
