using System.Text;
using Azure.Messaging;
using Confluent.Kafka;
using Microsoft.Azure.Data.SchemaRegistry.ApacheAvro;

namespace WebApi;

public class KafkaSchemaRegistryAvroSerializer<T> : ISerializer<T>, IDeserializer<T>
{
    private readonly SchemaRegistryAvroSerializer _innerSerializer;

    public KafkaSchemaRegistryAvroSerializer(SchemaRegistryAvroSerializer innerSerializer)
    {
        _innerSerializer = innerSerializer;
    }

    public byte[] Serialize(T o, SerializationContext context)
    {
        MessageContent content = _innerSerializer.Serialize<MessageContent, T>(o);

        var schemaIdBytes = Encoding.UTF8.GetBytes(content.ContentType.ToString());
        context.Headers.Add("content-type", schemaIdBytes);
        return content.Data.ToArray();
    }

    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull) return default;

        var schemaIdBytes = context.Headers.FirstOrDefault(h => h.Key == "content-type")
            .GetValueBytes();
        var contentType = Encoding.UTF8.GetString(schemaIdBytes);

        var content = new MessageContent
        {
            Data = new BinaryData(data.ToArray()),
            ContentType = contentType
        };

        return _innerSerializer.Deserialize<T>(content);
    }
}