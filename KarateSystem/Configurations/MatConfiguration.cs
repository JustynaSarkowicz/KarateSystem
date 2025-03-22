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

            // Relacja: mata może być przypiasana do wielu kata
            //          kata jest przypisane do jednej maty (1:N)
            builder.HasMany(c => c.Katas)
                   .WithOne(c => c.Mat)
                   .HasForeignKey(c => c.KataMatId)
                   .OnDelete(DeleteBehavior.Cascade); // ?

            // Relacja: jedna mata może być przypisana do wielu walk
            //          walka jest przypisana do jednej maty (1:N)
            builder.HasMany(c => c.Fights)
                   .WithOne(c => c.Mat)
                   .HasForeignKey(c => c.FightMatId)
                   .OnDelete(DeleteBehavior.Cascade); // ?
        }
    }
}
