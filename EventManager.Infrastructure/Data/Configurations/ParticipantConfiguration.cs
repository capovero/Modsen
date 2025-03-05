using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagement.Infrastructure.Data.Configurations;

public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
{
    public void Configure(EntityTypeBuilder<Participant> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.BirthDate)
            .IsRequired();

        builder.Property(p => p.RegistrationDate)
            .IsRequired()
            .HasDefaultValueSql("NOW()");
        
        builder.HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex(p => new { p.EventId, p.UserId })
            .IsUnique();
   
    }
}