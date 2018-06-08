using System;
using JustSaying.AwsTools.QueueCreation;

namespace JustSaying
{
    public interface INamingStrategy
    {
        string GetTopicName(string topicName, string messageTypeString, Type messageType);
        string GetQueueName(SqsReadConfiguration sqsConfig, string messageTypeString, Type messageType);
    }
}
