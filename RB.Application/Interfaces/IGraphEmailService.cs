using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Application.Interfaces
{
    public interface IGraphEmailService
    {
        Task SendMailAsync(string recipent, string subject, StringBuilder content, CancellationToken cancellationToken);
    }
}
