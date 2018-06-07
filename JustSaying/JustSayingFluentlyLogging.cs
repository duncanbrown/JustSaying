using System;
using Microsoft.Extensions.Logging;

namespace JustSaying
{
    public class JustSayingFluentlyLogging
    {
        public ILoggerFactory LoggerFactory { get; set; }

        public Func<Type, string> GetMessageTypeString { get; set; } = t => t.Name;
    }
}
