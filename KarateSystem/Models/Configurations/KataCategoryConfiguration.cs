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
    public class KataCategoryConfiguration : IEntityTypeConfiguration<KataCategory>
    {
        public void Configure(EntityTypeBuilder<KataCategory> builder)
        {
            builder.HasKey(t => t.KataCatId);
            builder.Property(t => t.KataCatName).IsRequired();
            builder.Property(t => t.KataCatGender).IsRequired(false);
            builder.Property(t => t.KataCatAgeMin).IsRequired();
            builder.Property(t => t.KataCatAgeMax).IsRequired();

            builder.HasMany(kc => kc.TourCatKatas)
               .WithOne(tck => tck.KataCategory)
               .HasForeignKey(tck => tck.KataCatId);

            builder.HasMany(kc => kc.CatKataDegrees)
               .WithOne(tck => tck.KataCategory)
               .HasForeignKey(tck => tck.KataCatId);
        }
    }
}
