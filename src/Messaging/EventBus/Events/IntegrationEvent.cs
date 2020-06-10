using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Events
{
    // TODO: Add support for serialization
    public class IntegrationEvent
    {
        public Guid Id { get; private set; }

        public DateTime CreationDate { get; set; }
    }
}
