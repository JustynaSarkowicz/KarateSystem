using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KarateSystem.Models;
using System.Reflection.Emit;

namespace KarateSystem.Configurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasKey(t => t.ClubId);
            builder.Property(t => t.ClubName).IsRequired();
            builder.Property(t => t.ClubPlace).IsRequired();

            // Relacja: jeden klub ma wielu zawodników
            //          zawodnik należy do jednego klubu (1:N)
            builder.HasMany(c => c.Competitors)
              .WithOne(c => c.Club)
              .HasForeignKey(c => c.CompClubId);
        }
    }
}
