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
    public class FightConfiguration : IEntityTypeConfiguration<Fight>
    {
        public void Configure(EntityTypeBuilder<Fight> builder)
        {
            builder.HasKey(t => t.FightId);
            builder.Property(t => t.FightScoreA).HasColumnType("decimal(5,1)");
            builder.Property(t => t.FightScoreB).HasColumnType("decimal(5,1)");

        }
    }
}
