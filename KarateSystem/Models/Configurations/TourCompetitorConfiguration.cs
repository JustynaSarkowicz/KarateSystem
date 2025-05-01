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
    public class TourCompetitorConfiguration : IEntityTypeConfiguration<TourCompetitor>
    {
        public void Configure(EntityTypeBuilder<TourCompetitor> builder)
        {
            builder.HasKey(t => t.TourCompId);

            builder.HasOne(t => t.Tournament)
                   .WithMany(t => t.TourCompetitors)
                   .HasForeignKey(t => t.TourId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Competitor)
                   .WithMany(c => c.TourCompetitors)
                   .HasForeignKey(t => t.CompId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(t => t.TourCatKata)
                   .WithMany(c => c.TourCompetitors)
                   .HasForeignKey(t => t.TourCatKataId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false); 

            builder.HasOne(t => t.TourCatKumite)
                   .WithMany(c => c.TourCompetitors)
                   .HasForeignKey(t => t.TourCatKumiteId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            builder.HasOne(tc => tc.Kata)
                    .WithOne(k => k.TourCompetitor)
                    .HasForeignKey<Kata>(k => k.TourCompId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Cascade);  
        }
    }
}
