using AppJwt.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJwt.Data.Configurations
{
    internal class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
    {
        public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
        {
            builder.ToTable("user_refresh_token").HasKey(x => x.UserId);
            builder.Property(x => x.UserId).HasColumnType("varchar(250)").HasColumnName("user_id");
            builder.Property(x => x.Token).HasColumnName("token").HasColumnType("varchar").IsRequired();
            builder.Property(x => x.Expiration).HasColumnName("expiration");

        }
    }
}
