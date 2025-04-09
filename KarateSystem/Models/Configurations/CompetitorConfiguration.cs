using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Configurations
{
    public class CompetitorConfiguration : IEntityTypeConfiguration<Competitor>
    {
        public void Configure(EntityTypeBuilder<Competitor> builder)
        {
            builder.HasKey(c => c.CompId);
            builder.Property(c => c.CompFirstName).IsRequired();
            builder.Property(c => c.CompLastName).IsRequired();
            builder.Property(c => c.CompDateOfBirth).IsRequired().HasColumnType("date");
            builder.Property(c => c.CompWeight).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(c => c.CompGender).IsRequired();

            // Relacja: jeden klub ma wielu zawodników
            //          zawodnik należy do jednego klubu (1:N)
            builder.HasOne(c => c.Club)         
                   .WithMany(c => c.Competitors) 
                   .HasForeignKey(c => c.CompClubId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacja: jeden stopień ma wielu zawodników
            //          zawodnik ma jeden stopień (1:N)
            builder.HasOne(c => c.Degree)         
                   .WithMany(d => d.Competitors) 
                   .HasForeignKey(c => c.CompDegreeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); // ?

            builder.HasMany(c => c.TourCompetitors)
                   .WithOne(c => c.Competitor)
                   .HasForeignKey(c => c.CompId);
        }
    }
}
