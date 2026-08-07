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
    internal class MemberConfiguration : GymUserConfiguration<Member>,IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("JoinDate").HasDefaultValueSql("GETDATE()");
            base.Configure(builder);
        }
    }
}
