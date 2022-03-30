using ChatVia.Domain.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatVia.Domain.Base
{
    public class EntityBase<TKey>
    {
        public EntityBase()
        {
            CreationTime = DateTime.Now;
        }

        public TKey Id { get; protected set; }
        public DateTime CreationTime { get; protected set; }

        // Domain Event
        [NotMapped]
        private readonly ConcurrentQueue<IDomainEvent> _domainEvents = new ConcurrentQueue<IDomainEvent>();

        [NotMapped]
        public IProducerConsumerCollection<IDomainEvent> DomainEvents => _domainEvents;

        protected void PublishEvent(IDomainEvent @event)
        {
            _domainEvents.Enqueue(@event);
        }
    }

    public class EntityBase : EntityBase<string>
    {
        public EntityBase() : base()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
