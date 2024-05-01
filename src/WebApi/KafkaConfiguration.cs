namespace WebApi;

public sealed class KafkaConfiguration
{
    public const string SectionName = "Kafka";
    
    public string FullyQualifiedNamespace { get; init; }
    
    public string OAuthTenantId { get; init; }
    
    public string OAuthClientId { get; init; }
    
    public string OAuthClientSecret { get; init; }
    
    public string BootstrapUrl => $"{FullyQualifiedNamespace}:9093";
    
    public string OAuthTokenEndpointUrl => $"https://login.microsoftonline.com/{OAuthTenantId}/oauth2/v2.0/token";
    
    public string OAuthScope => $"https://{FullyQualifiedNamespace}/.default";

    public string ConsumerGroupId { get; init; }

}