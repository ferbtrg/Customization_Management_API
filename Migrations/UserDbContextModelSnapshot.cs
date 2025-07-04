﻿// <auto-generated />
using System;
using Customization_Management_API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Customization_Management_API.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.6");

            modelBuilder.Entity("CustomizationCustomizationRequest", b =>
                {
                    b.Property<Guid>("CustomizationRequestsId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomizationsId")
                        .HasColumnType("TEXT");

                    b.HasKey("CustomizationRequestsId", "CustomizationsId");

                    b.HasIndex("CustomizationsId");

                    b.ToTable("CustomizationCustomizationRequest");
                });

            modelBuilder.Entity("Customization_Management_API.Domain.Entities.Customization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Customizations");
                });

            modelBuilder.Entity("Customization_Management_API.Domain.Entities.CustomizationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TotalValue")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UnitId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UnitId");

                    b.ToTable("CustomizationRequests");
                });

            modelBuilder.Entity("Customization_Management_API.Domain.Entities.Unit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientCPF")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("DevelopmentName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UnitNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("CustomizationCustomizationRequest", b =>
                {
                    b.HasOne("Customization_Management_API.Domain.Entities.CustomizationRequest", null)
                        .WithMany()
                        .HasForeignKey("CustomizationRequestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Customization_Management_API.Domain.Entities.Customization", null)
                        .WithMany()
                        .HasForeignKey("CustomizationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Customization_Management_API.Domain.Entities.CustomizationRequest", b =>
                {
                    b.HasOne("Customization_Management_API.Domain.Entities.Unit", "Unit")
                        .WithMany("CustomizationRequests")
                        .HasForeignKey("UnitId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Customization_Management_API.Domain.Entities.Unit", b =>
                {
                    b.Navigation("CustomizationRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
