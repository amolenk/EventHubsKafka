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

app.Run();
