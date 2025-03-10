using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using KarateSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace KarateSystem.Configurations
{
    public class CompetitorConfiguration : IEntityTypeConfiguration<Competitor>
    {
        public void Configure(EntityTypeBuilder<Competitor> builder)
        {
            throw new NotImplementedException();
        }
    }
}
