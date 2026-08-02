#:sdk Aspire.AppHost.Sdk@13.4.6
#:package Aspire.Hosting.JavaScript@13.4.6
#:package Aspire.Hosting.PostgreSQL@13.4.6
#:project src/DebateAnalyzer.Api/DebateAnalyzer.Api.csproj

using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithDataVolume();

if (builder.Environment.IsDevelopment())
{
    postgres.WithPgAdmin();
}

var debateAnalyzerDb = postgres.AddDatabase("debateanalyzerdb");

var api = builder.AddProject<Projects.DebateAnalyzer_Api>("api")
    .WithReference(debateAnalyzerDb)
    .WaitFor(debateAnalyzerDb);

var web = builder.AddViteApp("web", "src/DebateAnalyzer.Web", "start")
    .WithReference(api)
    .WaitFor(api)
    .WithExternalHttpEndpoints();

builder.Build().Run();
