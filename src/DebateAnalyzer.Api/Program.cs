using System.Text.Json.Serialization;
using Asp.Versioning;
using DebateAnalyzer.Api.Analyses;
using DebateAnalyzer.Api.Common;
using DebateAnalyzer.Application;
using DebateAnalyzer.Infrastructure;

const string WebClientCorsPolicy = "WebClient";

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddInfrastructure();
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
        options.AddPolicy(WebClientCorsPolicy, policy => policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()));
}

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "DebateAnalyzer API v1"));
    app.UseCors(WebClientCorsPolicy);
}

app.UseHttpsRedirection();

app.MapAnalysesEndpoints();

app.Run();
