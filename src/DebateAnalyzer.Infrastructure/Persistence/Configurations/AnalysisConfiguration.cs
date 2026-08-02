using DebateAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DebateAnalyzer.Infrastructure.Persistence.Configurations;

public class AnalysisConfiguration : IEntityTypeConfiguration<Analysis>
{
    public void Configure(EntityTypeBuilder<Analysis> builder)
    {
        builder.HasKey(a => a.Id);

        builder.OwnsOne(a => a.Video);

        builder.HasMany(a => a.Speakers).WithOne().HasForeignKey("AnalysisId").IsRequired();
        builder.HasMany(a => a.TranscriptSegments).WithOne().HasForeignKey("AnalysisId").IsRequired();
        builder.HasMany(a => a.Fallacies).WithOne().HasForeignKey("AnalysisId").IsRequired();
        builder.HasMany(a => a.Claims).WithOne().HasForeignKey("AnalysisId").IsRequired();
        builder.HasMany(a => a.KeyMoments).WithOne().HasForeignKey("AnalysisId").IsRequired();
    }
}
