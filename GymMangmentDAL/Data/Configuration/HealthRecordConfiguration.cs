using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymMangmentDAL.Data.Configuration
{
    internal class HealthRecordConfiguration : IEntityTypeConfiguration<HealthRecord>
    {
        public void Configure(EntityTypeBuilder<HealthRecord> builder)
        {
            builder.ToTable("Members").HasKey(hr => hr.Id);
            builder.HasOne<Member>()
                   .WithOne(m => m.HealthRecord)
                   .HasForeignKey<HealthRecord>(hr => hr.Id);
            builder.Ignore(X=>X.CreatedAt);
            builder.Ignore(X=>X.UpdatedAt);
        }
    }
}
