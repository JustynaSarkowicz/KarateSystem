using KarateSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Configurations
{
    public class TourCatKataConfiguration : IEntityTypeConfiguration<TourCatKata>
    {
        public void Configure(EntityTypeBuilder<TourCatKata> builder)
        {
            builder.HasKey(tck => tck.TourCatKataId);

            // Relacja z Tournament (1:N)
            builder.HasOne(tck => tck.Tour)
                   .WithMany(t => t.TourCatKatas)
                   .HasForeignKey(tck => tck.TourId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

            // Relacja z KataCategory (1:N)
            builder.HasOne(tck => tck.KataCategory)
                   .WithMany(kc => kc.TourCatKatas)
                   .HasForeignKey(tck => tck.KataCatId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict); 

            // Relacja z Mat (1:N)
            builder.HasOne(tck => tck.Mat)
                   .WithMany(m => m.TourCatKatas)
                   .HasForeignKey(tck => tck.MatId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacja N:M z Competitor (przez TourCompetitor)
            builder.HasMany(tck => tck.TourCompetitors)
                   .WithOne(tc => tc.TourCatKata)
                   .HasForeignKey(tc => tc.TourCatKataId);
        }
    }

}
