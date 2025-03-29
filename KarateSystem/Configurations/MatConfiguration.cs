using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarateSystem.Configurations
{
    public class MatConfiguration : IEntityTypeConfiguration<Mat>
    {
        public void Configure(EntityTypeBuilder<Mat> builder)
        {
            builder.HasKey(t => t.MatId);
            builder.Property(t => t.MatName).IsRequired();

            // Relacja: mata może być przypiasana do wielu kategorii kata
            //          kategoria kata jest przypisane do jednej maty (1:N)
            builder.HasMany(m => m.TourCatKatas)
               .WithOne(tck => tck.Mat)
               .HasForeignKey(tck => tck.MatId);

            builder.HasMany(m => m.TourCatKumites)
               .WithOne(tck => tck.Mat)
               .HasForeignKey(tck => tck.MatId);
        }
    }
}
