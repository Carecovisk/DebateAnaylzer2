using DebateAnalyzer.Application.Analyses.Interfaces;
using DebateAnalyzer.Infrastructure.ExternalServices.YtDlp;
using DebateAnalyzer.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DebateAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DebateAnalyzerDbContext>("debateanalyzerdb");

        builder.Services.AddScoped<IAnalysisRepository, AnalysisRepository>();
        builder.Services.AddScoped<IVideoMetadataProvider, YtDlpVideoMetadataProvider>();

        return builder;
    }
}
