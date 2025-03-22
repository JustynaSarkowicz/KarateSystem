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
    public class KumiteCategoryConfiguration : IEntityTypeConfiguration<KumiteCategory>
    {
        public void Configure(EntityTypeBuilder<KumiteCategory> builder)
        {
            builder.HasKey(t => t.KumiteCatId);
            builder.Property(t => t.KumiteCatName).IsRequired();
            builder.Property(t => t.KumiteCatGender).IsRequired();
            builder.Property(t => t.KumiteCatAgeMin).IsRequired();
            builder.Property(t => t.KumiteCatAgeMax).IsRequired();
            builder.Property(t => t.KumiteCatWeightMin).IsRequired().HasColumnType("decimal(5,2)");
            builder.Property(t => t.KumiteCatWeightMax).IsRequired().HasColumnType("decimal(5,2)");
        }
    }
}
