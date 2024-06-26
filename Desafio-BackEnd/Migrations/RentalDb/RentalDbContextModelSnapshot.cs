﻿// <auto-generated />
using System;
using Desafio_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Desafio_Backend.Migrations.RentalDb
{
    [DbContext(typeof(RentalDbContext))]
    partial class RentalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Desafio_Backend.Models.DeliveryPerson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CnhNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<string>("CnhPicture")
                        .HasColumnType("text");

                    b.Property<string>("CnhType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("character varying(18)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IdentityUserId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IdentityUserId");

                    b.HasIndex("Cnpj", "CnhNumber")
                        .IsUnique();

                    b.ToTable("DeliveryPeople", t =>
                        {
                            t.HasComment("AKA Users, Couriers, etc");
                        });
                });

            modelBuilder.Entity("Desafio_Backend.Models.Motorbike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AdminId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("character varying(7)");

                    b.Property<int>("ProductionYear")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.ToTable("Motorbikes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AdminId = "SYSTEM",
                            Model = "YAMAHA",
                            Plate = "ABC1234",
                            ProductionYear = 2000
                        },
                        new
                        {
                            Id = 2,
                            AdminId = "SYSTEM",
                            Model = "HONDA",
                            Plate = "DEF5678",
                            ProductionYear = 2010
                        });
                });

            modelBuilder.Entity("Desafio_Backend.Models.MotorbikeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MotorbikeId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MotorbikeId");

                    b.ToTable("MotorbikeLogs");
                });

            modelBuilder.Entity("Desafio_Backend.Models.Plan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("PenaltyFeePercent")
                        .HasColumnType("integer");

                    b.Property<int>("RentalCostPerDay")
                        .HasColumnType("integer");

                    b.Property<int>("RentalDays")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Plans");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PenaltyFeePercent = 20,
                            RentalCostPerDay = 300,
                            RentalDays = 7
                        },
                        new
                        {
                            Id = 2,
                            PenaltyFeePercent = 40,
                            RentalCostPerDay = 280,
                            RentalDays = 15
                        },
                        new
                        {
                            Id = 3,
                            PenaltyFeePercent = 40,
                            RentalCostPerDay = 220,
                            RentalDays = 30
                        },
                        new
                        {
                            Id = 4,
                            PenaltyFeePercent = 40,
                            RentalCostPerDay = 200,
                            RentalDays = 45
                        },
                        new
                        {
                            Id = 5,
                            PenaltyFeePercent = 40,
                            RentalCostPerDay = 180,
                            RentalDays = 50
                        });
                });

            modelBuilder.Entity("Desafio_Backend.Models.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("DeliveryPersonId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpectedEndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("MotorbikeId")
                        .HasColumnType("integer");

                    b.Property<int>("PlanId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryPersonId");

                    b.HasIndex("MotorbikeId");

                    b.HasIndex("PlanId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Desafio_Backend.Models.DeliveryPerson", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("IdentityUserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Desafio_Backend.Models.MotorbikeLog", b =>
                {
                    b.HasOne("Desafio_Backend.Models.Motorbike", "Motorbike")
                        .WithMany()
                        .HasForeignKey("MotorbikeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Motorbike");
                });

            modelBuilder.Entity("Desafio_Backend.Models.Rental", b =>
                {
                    b.HasOne("Desafio_Backend.Models.DeliveryPerson", "DeliveryPerson")
                        .WithMany("Rentals")
                        .HasForeignKey("DeliveryPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desafio_Backend.Models.Motorbike", "Motorbike")
                        .WithMany("Rentals")
                        .HasForeignKey("MotorbikeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Desafio_Backend.Models.Plan", "Plan")
                        .WithMany("Rentals")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DeliveryPerson");

                    b.Navigation("Motorbike");

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("Desafio_Backend.Models.DeliveryPerson", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("Desafio_Backend.Models.Motorbike", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("Desafio_Backend.Models.Plan", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
