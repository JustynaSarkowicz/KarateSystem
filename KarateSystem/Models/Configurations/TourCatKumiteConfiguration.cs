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
    public class TourCatKumiteConfiguration : IEntityTypeConfiguration<TourCatKumite>
    {
        public void Configure(EntityTypeBuilder<TourCatKumite> builder)
        {
            builder.HasKey(tck => tck.TourCatKumiteId);

            // Relacja z Tournament (1:N)
            builder.HasOne(tck => tck.Tour)
                   .WithMany(t => t.TourCatKumites)
                   .HasForeignKey(tck => tck.TourId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacja z KumiteCategory (1:N)
            builder.HasOne(tck => tck.KumiteCategory)
                   .WithMany(kc => kc.TourCatKumites)
                   .HasForeignKey(tck => tck.KumiteCatId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacja z Mat (1:N)
            builder.HasOne(tck => tck.Mat)
                   .WithMany(m => m.TourCatKumites)
                   .HasForeignKey(tck => tck.MatId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            // Relacja z Competitor (przez TourCompetitor)
            builder.HasMany(tck => tck.TourCompetitors)
                   .WithOne(tc => tc.TourCatKumite)
                   .HasForeignKey(tc => tc.TourCatKumiteId);
        }
    }
}
