using Microsoft.AspNetCore.Http.Timeouts;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Forwarder;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

var origins = builder.Configuration.GetSection("Origin:ClientOrigins").Get<string>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddReverseProxy()
    .LoadFromMemory(GetRoutes(), GetClusters(builder.Configuration))
    .AddTransforms(transforms =>
    {
        transforms.AddRequestTransform(context =>
        {
            context.ProxyRequest.Headers.Add("X-Forwarded-Host", context.HttpContext.Request.Host.Value);
            context.ProxyRequest.Headers.Add("X-Request-Id", context.HttpContext.TraceIdentifier);
            return ValueTask.CompletedTask;
        });
    });

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; // 100MB
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(60);
});

var app = builder.Build();

app.MapReverseProxy();
app.UseCors();

app.Run();

static IReadOnlyList<RouteConfig> GetRoutes() =>
    [
        new ()
        {
            RouteId = "identity-route",
            ClusterId = "identityCluster",
            Match = new RouteMatch {Path = "/api/identity/{**catch-all}"}
        },
        new ()
        {
            RouteId = "contents-route",
            ClusterId = "contentsCluster",
            Match = new RouteMatch {Path = "/api/contents/{**catch-all}"}
        },
        new ()
        {
            RouteId = "ai-route",
            ClusterId = "aiCluster",
            Match = new RouteMatch {Path = "/api/ai/{**catch-all}"}
        }
    ];

static IReadOnlyList<ClusterConfig> GetClusters(IConfiguration configuration)
{
    var clustersDict = configuration.GetSection("Clusters").Get<Dictionary<string, ClusterConf>>();

    if (clustersDict == null || clustersDict.Count == 0)
    {
        throw new InvalidOperationException("No cluster configurations found in the configuration.");
    }

    return clustersDict.Select(kvp => new ClusterConfig
    {
        // Use the Key from the dictionary as the ClusterId
        ClusterId = kvp.Key,
        Destinations = new Dictionary<string, DestinationConfig>
        {
            { "destination1", new DestinationConfig { Address = kvp.Value.Address } }
        },
        HttpRequest = new ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromMinutes(10) }
    }).ToList();
}

class ClusterConf
{
    public string ClusterId { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}