using System;
using System.Collections.Generic;
using JustSaying.Models;

namespace JustSaying.Messaging.MessageSerialisation
{
    public class MessageSerialisationRegister : IMessageSerialisationRegister
    {
        private readonly Func<Type, string> _getMessageTypeString;

        public MessageSerialisationRegister(Func<Type, string> getMessageTypeString)
        {
            _getMessageTypeString = getMessageTypeString;
        }

        private readonly Dictionary<string, TypeSerialiser> _map = new Dictionary<string, TypeSerialiser>();

        public void AddSerialiser<T>(IMessageSerialiser serialiser) where T : Message
        {
            var keyname = _getMessageTypeString(typeof(T));
            if (!_map.ContainsKey(keyname))
            {
                _map.Add(keyname, new TypeSerialiser(typeof(T), serialiser));
            }
        }

        public Message DeserializeMessage(string body)
        {
            foreach (var formatter in _map)
            {
                var stringType = formatter.Value.Serialiser.GetMessageType(body);
                if (string.IsNullOrWhiteSpace(stringType))
                {
                    continue;
                }

                var matchedType = formatter.Value.Type;
                if (!string.Equals(_getMessageTypeString(matchedType), stringType, StringComparison.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                return formatter.Value.Serialiser.Deserialise(body, matchedType);
            }

            throw new MessageFormatNotSupportedException(
                $"Message can not be handled - type undetermined. Message body: '{body}'");
        }

        public string Serialise(Message message, bool serializeForSnsPublishing)
        {
            var formatter = _map[_getMessageTypeString(message.GetType())];
            return formatter.Serialiser.Serialise(message, serializeForSnsPublishing);
        }

    }
}
