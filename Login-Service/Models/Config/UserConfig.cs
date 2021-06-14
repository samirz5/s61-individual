using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login_Service.Models.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(prop => prop.Id);
            builder.Property(prop => prop.UserName)
                .HasMaxLength(20)
                .IsRequired();
            builder.Property(prop => prop.Password)
                .IsRequired();
        }
    }
}
