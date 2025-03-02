using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(e => e.Description)
            .IsRequired()
            .HasMaxLength(2000);
        builder.Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(e => e.Date)
            .IsRequired();
        builder.Property(e => e.MaxParticipants)
            .IsRequired()
            .HasDefaultValue(0);
        builder.HasMany(e => e.Participants)
            .WithOne(p => p.Event)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}