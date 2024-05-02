using Azure.Data.SchemaRegistry;
using Azure.Identity;
using Confluent.Kafka;
using Microsoft.Azure.Data.SchemaRegistry.ApacheAvro;
using Microsoft.Extensions.Options;
using WebApi;
using JsonSerializer = System.Text.Json.JsonSerializer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<KafkaConfiguration>(
    builder.Configuration.GetSection(KafkaConfiguration.SectionName));

var app = builder.Build();

app.MapPost("/order", async (Order order, IOptions<KafkaConfiguration> options) =>
{
    var kafkaConfig = options.Value;

    var config = new ProducerConfig
    {
        BootstrapServers = kafkaConfig.BootstrapUrl,
        SaslMechanism = SaslMechanism.OAuthBearer,
        SaslOauthbearerMethod = SaslOauthbearerMethod.Oidc,
        SaslOauthbearerClientId = kafkaConfig.OAuthClientId,
        SaslOauthbearerClientSecret = kafkaConfig.OAuthClientSecret,
        SaslOauthbearerTokenEndpointUrl = kafkaConfig.OAuthTokenEndpointUrl,
        SaslOauthbearerScope = kafkaConfig.OAuthScope,
        SecurityProtocol = SecurityProtocol.SaslSsl
    };

    using var producer = new ProducerBuilder<Null, string>(config)
        .Build();

    await producer.ProduceAsync("orders", new Message<Null, string>
    {
        Value = JsonSerializer.Serialize(order)
    });

    return Results.Accepted();
});

app.MapGet("/order/{partitionId}", (int partitionId, IOptions<KafkaConfiguration> options) =>
{
    var kafkaConfig = options.Value;

    var config = new ConsumerConfig
    {
        BootstrapServers = kafkaConfig.BootstrapUrl,
        SaslMechanism = SaslMechanism.OAuthBearer,
        SaslOauthbearerMethod = SaslOauthbearerMethod.Oidc,
        SaslOauthbearerClientId = kafkaConfig.OAuthClientId,
        SaslOauthbearerClientSecret = kafkaConfig.OAuthClientSecret,
        SaslOauthbearerTokenEndpointUrl = kafkaConfig.OAuthTokenEndpointUrl,
        SaslOauthbearerScope = kafkaConfig.OAuthScope,
        SecurityProtocol = SecurityProtocol.SaslSsl,
        GroupId = kafkaConfig.ConsumerGroupId,
        AutoOffsetReset = AutoOffsetReset.Earliest
    };

    using var consumer = new ConsumerBuilder<Ignore, string>(config)
        .Build();

    consumer.Assign(new TopicPartition("orders", new Partition(partitionId)));

    var result = consumer.Consume(TimeSpan.FromSeconds(3));
    if (result is not null)
    {
        consumer.Commit(result);

        return Results.Ok(JsonSerializer.Deserialize<Order>(result.Message.Value));
    }

    return Results.NoContent();
});

app.MapPost("/shipment", async (ShipmentNotification shipment, IOptions<KafkaConfiguration> options) =>
{
    var kafkaConfig = options.Value;

    var config = new ProducerConfig
    {
        BootstrapServers = kafkaConfig.BootstrapUrl,
        SaslMechanism = SaslMechanism.OAuthBearer,
        SaslOauthbearerMethod = SaslOauthbearerMethod.Oidc,
        SaslOauthbearerClientId = kafkaConfig.OAuthClientId,
        SaslOauthbearerClientSecret = kafkaConfig.OAuthClientSecret,
        SaslOauthbearerTokenEndpointUrl = kafkaConfig.OAuthTokenEndpointUrl,
        SaslOauthbearerScope = kafkaConfig.OAuthScope,
        SecurityProtocol = SecurityProtocol.SaslSsl
    };

    var credential = new ClientSecretCredential(kafkaConfig.OAuthTenantId, kafkaConfig.OAuthClientId,
        kafkaConfig.OAuthClientSecret);

    var schemaRegistryClient = new SchemaRegistryClient(kafkaConfig.FullyQualifiedNamespace, credential);

    var serializer = new SchemaRegistryAvroSerializer(schemaRegistryClient, "default",
        new SchemaRegistryAvroSerializerOptions { AutoRegisterSchemas = true });

    using var producer = new ProducerBuilder<Null, ShipmentNotification>(config)
        .SetValueSerializer(new KafkaSchemaRegistryAvroSerializer<ShipmentNotification>(serializer))
        .Build();

    await producer.ProduceAsync("shipments", new Message<Null, ShipmentNotification>
    {
        Value = shipment
    });

    return Results.Accepted();
});

app.Run();

public record Order(string OrderId, string ItemId, int ItemQuantity);
