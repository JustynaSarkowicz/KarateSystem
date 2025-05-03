using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Configurations
{
    public class FightConfiguration : IEntityTypeConfiguration<Fight>
    {
        public void Configure(EntityTypeBuilder<Fight> builder)
        {
            builder.HasKey(t => t.FightId);
            builder.Property(t => t.RedCompetitorScore).HasColumnType("decimal(5,1)");
            builder.Property(t => t.BlueCompetitorScore).HasColumnType("decimal(5,1)");
            builder.Property(t => t.FightTime);
            builder.Property(t => t.FightNumOverTime);
            builder.Property(t => t.FightWalkover);
            builder.Property(t => t.Round);

            builder.HasOne(f => f.RedCompetitor)
                   .WithMany(c => c.RedFights)
                   .HasForeignKey(f => f.RedCompetitorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.BlueCompetitor)
                   .WithMany(c => c.BlueFights)
                   .HasForeignKey(f => f.BlueCompetitorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Winner)
                   .WithMany(c => c.WonFights)
                   .HasForeignKey(f => f.WinnerId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(f => f.NextFight)
                   .WithMany()
                   .HasForeignKey(f => f.NextFightId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.TourCatKumite)
                   .WithMany(c => c.Fights)
                   .HasForeignKey(f => f.TourCatKumiteId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
