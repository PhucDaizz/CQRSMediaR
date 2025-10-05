using Domain.Common;
using Domain.Entities;

namespace Domain.Events
{
    public record ProductUpdatedEvent(Product Product) : DomainEvent;
}
