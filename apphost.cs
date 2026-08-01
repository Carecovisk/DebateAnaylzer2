#:sdk Aspire.AppHost.Sdk@13.4.6
#:package Aspire.Hosting.JavaScript@13.4.6
#:project src/DebateAnalyzer.Api/DebateAnalyzer.Api.csproj

var builder = DistributedApplication.CreateBuilder(args);

var api = builder.AddProject<Projects.DebateAnalyzer_Api>("api");

var web = builder.AddViteApp("web", "src/DebateAnalyzer.Web", "start")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
