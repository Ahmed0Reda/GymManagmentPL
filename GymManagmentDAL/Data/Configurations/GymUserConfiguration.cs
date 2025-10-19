﻿using GymManagmentDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser 
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(x => x.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);
            builder.Property(x => x.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);
            builder.Property(x => x.PhoneNumber)
                .HasColumnType("varchar")
                .HasMaxLength(11);
            builder.OwnsOne(x => x.Address, address =>
            {
                address.Property(a => a.BuildingNumber)
                .HasColumnName("BuildingNumber");
                address.Property(a => a.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("City");
                address.Property(a => a.Street)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("Street");
            });
            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.PhoneNumber).IsUnique();
            builder.ToTable(x =>
            {
                x.HasCheckConstraint("GymUser_EmailCheck", "Email LIKE '_%@_%._%'");
                x.HasCheckConstraint("GymUser_PhoneCheck", "PhoneNumber LIKE '01%' AND PhoneNumber NOT LIKE '%[^0-9]%'");
            });
        }
    }
}
