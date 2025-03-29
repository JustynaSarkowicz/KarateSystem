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
            builder.Property(t => t.KataCatGender).IsRequired();
            builder.Property(t => t.KataCatAgeMin).IsRequired();
            builder.Property(t => t.KataCatAgeMax).IsRequired();

            // Relacja: jeden stopień należy do wielu kategorii kata
            //          kategoria kata ma jeden stopień
            builder.HasOne(c => c.Degree)
                   .WithMany(c => c.KataCategories)
                   .HasForeignKey(c => c.KataCatDegreeId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(kc => kc.TourCatKatas)
               .WithOne(tck => tck.KataCategory)
               .HasForeignKey(tck => tck.KataCatId);
        }
    }
}
