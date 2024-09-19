using LocalStack.Client;
using LocalStack.Client.Options;
using Amazon.APIGateway;

Environment.SetEnvironmentVariable("AWS_SERVICE_URL", string.Empty);

var sessionOptions = new SessionOptions();

var configOptions = new ConfigOptions();

var session = SessionStandalone
    .Init()
    .WithSessionOptions(sessionOptions)
    .WithConfigurationOptions(configOptions).Create();

var localstackClient = session.CreateClientByImplementation<AmazonAPIGatewayClient>();

await localstackClient.CreateRestApiAsync(new() { Name = Guid.NewGuid().ToString() });