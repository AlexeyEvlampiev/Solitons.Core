using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Solitons.Queues
{
    public sealed class DomainQueueMessageOptions : IQueueMessageOptions
    {
        
        private Dictionary<string, string> _properties;

        internal DomainQueueMessageOptions(IQueueServiceProvider provider)
        {
            MessageMaxTimeToLive = provider.MessageMaxTimeToLive;
            MessageMaxSizeInBytes = provider.MessageMaxSizeInBytes;
        }

        public TimeSpan MessageMinTimeToLive => TimeSpan.FromSeconds(1.0);
        public TimeSpan MessageMaxTimeToLive { get; }
        public int MessageMaxSizeInBytes { get; }

        public TimeSpan? TimeToLive { get; internal set; }

        public TimeSpan? VisibilityTimeout { get; internal set; }

        public IEnumerable<string> Properties => _properties?.Keys ?? Enumerable.Empty<string>();

        public string GetProperty(string name)
        {
            LazyInitializer.EnsureInitialized(ref _properties,
                () => new Dictionary<string, string>(StringComparer.Ordinal));
            return _properties[name];
        }

        public DomainQueueMessageOptions WithVisibilityTimeout(TimeSpan timeout)
        {
            if (timeout < TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout));
            VisibilityTimeout = timeout;
            return this;
        }

        public DomainQueueMessageOptions WithMessageTimeToLive(TimeSpan timeToLive)
        {
            TimeToLive = timeToLive.ThrowIfArgumentOutOfRange(
                MessageMinTimeToLive,
                MessageMaxTimeToLive,
                nameof(timeToLive));
            return this;
        }

        public DomainQueueMessageOptions WithProperty(string name, string value)
        {
            LazyInitializer.EnsureInitialized(ref _properties,
                () => new Dictionary<string, string>(StringComparer.Ordinal));
            _properties[name] = value;
            return this;
        }

        public void CopyPropertiesTo(IDictionary<string, string> properties)
        {
            if (properties == null) throw new ArgumentNullException(nameof(properties));
            if (_properties is null) return;
            foreach (var property in _properties)
            {
                properties[property.Key] = property.Value;
            }
        }
    }
}
