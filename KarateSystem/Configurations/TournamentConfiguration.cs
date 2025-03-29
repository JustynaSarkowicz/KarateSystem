using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Configurations
{
    public class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
    {
        public void Configure(EntityTypeBuilder<Tournament> builder)
        {
            builder.HasKey(t => t.TourId);
            builder.Property(t => t.TourName).IsRequired();
            builder.Property(t => t.TourPlace).IsRequired();
            builder.Property(t => t.TourDate).IsRequired().HasColumnType("date");

            builder.HasMany(c => c.TourCompetitors)
                   .WithOne(c => c.Tournament)
                   .HasForeignKey(c => c.TourId);

            builder.HasMany(t => t.TourCatKatas)
               .WithOne(tck => tck.Tour)
               .HasForeignKey(tck => tck.TourId);

            builder.HasMany(t => t.TourCatKumites)
               .WithOne(tck => tck.Tour)
               .HasForeignKey(tck => tck.TourId);
        }
    }
}
