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
            throw new NotImplementedException();
        }
    }
}
