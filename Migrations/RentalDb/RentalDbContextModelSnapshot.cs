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
                        .HasColumnType("text");

                    b.Property<string>("CnhPicture")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("CnhType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

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
                        .HasColumnType("text");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProductionYear")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.ToTable("Motorbikes");
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
