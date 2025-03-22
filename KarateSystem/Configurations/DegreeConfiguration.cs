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
    public class DegreeConfiguration : IEntityTypeConfiguration<Degree>
    {
        public void Configure(EntityTypeBuilder<Degree> builder)
        {
            builder.HasKey(t => t.DegreeId);
            builder.Property(c => c.DegreeName).IsRequired();


            // Relacja: jeden stopień ma wielu zawodników
            //          zawodnik ma jeden stopień (1:N)
            builder.HasMany(c => c.Competitors)
                   .WithOne(c => c.Degree)
                   .HasForeignKey(c => c.CompDegreeId)
                   .OnDelete(DeleteBehavior.Cascade); // ?

            // Relacja: jeden stopień należy do wielu kategorii kata
            //          kategoria kata ma jeden stopień (1:N)
            builder.HasMany(c => c.KataCategories)
                   .WithOne(c => c.Degree)
                   .HasForeignKey(c => c.KataCatDegreeId)
                   .OnDelete(DeleteBehavior.Cascade); // ?
        }
    }
}
