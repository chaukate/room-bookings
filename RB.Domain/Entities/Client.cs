using RB.Domain.Enumerations;
using RB.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace RB.Domain.Entities
{
    public class Client : BaseEntity, ICreatedEvent, IUpdatedEvent
    {
        public string Name { get; set; }
        public string AdminEmail { get; set; }
        public string TenantId { get; set; }
        public string SecretKey { get; set; }

        [NotMapped]
        public ClientActivity Activity { get; set; }

    }
}
