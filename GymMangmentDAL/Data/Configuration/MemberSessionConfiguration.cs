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
    internal class MemberSessionConfiguration : IEntityTypeConfiguration<MemberSession>
    {
        public void Configure(EntityTypeBuilder<MemberSession> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("BookingDate").HasDefaultValueSql("GETDATE()");
            builder.HasKey(x => new { x.MemberId, x.SessionId });
            builder.Ignore(x => x.Id);
        }
    }
}
