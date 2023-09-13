using MediatR;
using Microsoft.EntityFrameworkCore;
using RB.Application.Interfaces;

namespace RB.Application.Clients.Queries
{
    public class ListClientHandler : IRequestHandler<ListClientsQuery, List<ListClientsResponse>>
    {
        private readonly IRBDbContext _dbContext;
        public ListClientHandler(IRBDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ListClientsResponse>> Handle(ListClientsQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Clients
                                           .Select(s => new ListClientsResponse
                                           {
                                               Id = s.Id,
                                               Name = s.Name,
                                               AdminEmail = s.AdminEmail
                                           })
                                           .OrderBy(o => o.Name)
                                           .ToListAsync(cancellationToken);

            return response;
        }
    }

    public class ListClientsQuery : IRequest<List<ListClientsResponse>> { }

    public class ListClientsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdminEmail { get; set; }
    }
}
