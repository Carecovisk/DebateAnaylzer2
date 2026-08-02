using DebateAnalyzer.Infrastructure.Persistence;
using Microsoft.Extensions.Hosting;

namespace DebateAnalyzer.Infrastructure;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<DebateAnalyzerDbContext>("debateanalyzerdb");

        return builder;
    }
}
