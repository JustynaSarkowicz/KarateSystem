using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KarateSystem.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(t => t.UserId);
            builder.Property(t => t.UserFirstName).IsRequired();
            builder.Property(t => t.UserLastName).IsRequired();
            builder.Property(t => t.UserLogin).IsRequired();
            builder.Property(t => t.UserPass).IsRequired();
            builder.Property(t => t.UserRole).IsRequired();
        }
    }
}
