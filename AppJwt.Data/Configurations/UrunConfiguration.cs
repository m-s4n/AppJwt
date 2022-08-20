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
    public class UrunConfiguration : IEntityTypeConfiguration<Urun>
    {
        public void Configure(EntityTypeBuilder<Urun> builder)
        {
            builder.ToTable("urun").HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int").HasComment("primary key").IsRequired();
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(250)").HasComment("urun tanımı");
            builder.Property(x => x.Price).HasColumnName("price").HasColumnType("numeric").HasPrecision(18, 2);
            builder.Property(x => x.Stock).HasColumnName("stock").HasColumnType("int").HasComment("urun stok bilgisi");
            builder.Property(x => x.UserId).HasColumnName("user_id").HasColumnType("varchar(250)").IsRequired();
        }
    }
}
