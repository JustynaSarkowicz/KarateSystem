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
    public class KataConfiguration : IEntityTypeConfiguration<Kata>
    {
        public void Configure(EntityTypeBuilder<Kata> builder)
        {
            throw new NotImplementedException();
        }
    }
}
