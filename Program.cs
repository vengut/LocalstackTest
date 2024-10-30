using LocalStack.Client;
using LocalStack.Client.Options;
using Amazon.APIGateway;
using Testcontainers.LocalStack;
using Amazon;
using Amazon.Runtime;

var container = new LocalStackBuilder()
    .WithImage($"localstack/localstack:latest")
    .WithName($"localStack-latest-{Guid.NewGuid().ToString().ToUpperInvariant()}")
    .WithEnvironment("DOCKER_HOST", "unix:///var/run/docker.sock")
    .WithEnvironment("DEBUG", "1")
    .WithEnvironment("LS_LOG", "trace-internal")
    .WithPortBinding(4566, 4566)
    .WithCleanUp(true)
    .Build();

Console.WriteLine("Starting LocalStack Container");
await container.StartAsync();
Console.WriteLine("LocalStack Container started");


Console.WriteLine("Creating REST API using manual client");
var manualClient = new AmazonAPIGatewayClient(new BasicAWSCredentials("abc", "def"), new AmazonAPIGatewayConfig { ServiceURL = "https://localhost.localstack.cloud:4566" });
await manualClient.CreateRestApiAsync(new() { Name = Guid.NewGuid().ToString() });
Console.WriteLine("Created REST API using manual client");

Console.WriteLine("Creating REST API using auto configured client");
Environment.SetEnvironmentVariable("AWS_SERVICE_URL", string.Empty);
var session = SessionStandalone
    .Init()
    .WithSessionOptions(new SessionOptions())
    .WithConfigurationOptions(new ConfigOptions())
    .Create();
var autoClient = session.CreateClientByImplementation<AmazonAPIGatewayClient>();
await autoClient.CreateRestApiAsync(new() { Name = Guid.NewGuid().ToString() });
Console.WriteLine("Created REST API using auto configured client");
