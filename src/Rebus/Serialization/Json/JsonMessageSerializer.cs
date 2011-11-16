﻿using System.Text;
using Newtonsoft.Json;
using Rebus.Messages;
using Rebus.Persistence.InMemory;
using System.Linq;

namespace Rebus.Serialization.Json
{
    /// <summary>
    /// Implementation of <see cref="InMemorySubscriptionStorage"/> that uses
    /// the ubiquitous NewtonSoft JSON serializer to serialize and deserialize messages.
    /// </summary>
    public class JsonMessageSerializer : ISerializeMessages
    {
        static readonly JsonSerializerSettings Settings =
            new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};

        static readonly Encoding Encoding = Encoding.UTF8;

        public TransportMessageToSend Serialize(Message message)
        {
            var messageAsString = JsonConvert.SerializeObject(message, Formatting.Indented, Settings);
            
            return new TransportMessageToSend
                       {
                           Data = messageAsString,
                           Headers = message.Headers.ToDictionary(k => k.Key, v => v.Value),
                       };
        }

        public Message Deserialize(ReceivedTransportMessage transportMessage)
        {
            var messageAsString = transportMessage.Data;

            return (Message) JsonConvert.DeserializeObject(messageAsString, Settings);
        }
    }
}