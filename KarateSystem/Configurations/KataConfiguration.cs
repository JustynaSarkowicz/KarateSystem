using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Infrastructure;

namespace KarateSystem.Configurations
{
    public class KataConfiguration : IEntityTypeConfiguration<Kata>
    {
        public void Configure(EntityTypeBuilder<Kata> builder)
        {
            builder.HasKey(t => t.KataId);
            builder.Property(t => t.KataRate1).HasColumnType("decimal(5,1)");
            builder.Property(t => t.KataRate2).HasColumnType("decimal(5,1)");
            builder.Property(t => t.KataRate3).HasColumnType("decimal(5,1)");
            builder.Property(t => t.KataRate4).HasColumnType("decimal(5,1)");
            builder.Property(t => t.KataRate5).HasColumnType("decimal(5,1)");
            builder.Property(t => t.KataScore).HasColumnType("decimal(5,2)");

            // Relacja: mata jest przypiasana do wielu kata
            //          kata jest przypisane do jednej maty (1:N)
            builder.HasOne(t => t.Mat)
                   .WithMany(t => t.Katas)
                   .HasForeignKey(t => t.KataMatId)
                   .IsRequired()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
