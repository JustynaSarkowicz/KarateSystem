using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateSystem.Models.Configurations
{
    public class CatKataDegreeConfiguration : IEntityTypeConfiguration<CatKataDegree>
    {
        public void Configure(EntityTypeBuilder<CatKataDegree> builder)
        {
            builder.HasKey(c => c.CatKataDegreeId);

            builder.HasOne(c => c.KataCategory)
                .WithMany(k => k.CatKataDegrees)
                .HasForeignKey(c => c.KataCatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Degree)
                .WithMany(d => d.CatKataDegrees)
                .HasForeignKey(c => c.DegreeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
