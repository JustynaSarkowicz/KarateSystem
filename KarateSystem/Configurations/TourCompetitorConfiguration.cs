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
                   .OnDelete(DeleteBehavior.Cascade); // ??

            builder.HasOne(t => t.Competitor)
                   .WithMany(c => c.TourCompetitors)
                   .HasForeignKey(t => t.CompId)
                   .OnDelete(DeleteBehavior.Cascade);  // ??
        }
    }
}
