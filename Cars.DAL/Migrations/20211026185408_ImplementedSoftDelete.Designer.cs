﻿// <auto-generated />
using System;
using Cars.DAL.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Cars.DAL.Migrations
{
    [DbContext(typeof(CarDbContext))]
    [Migration("20211026185408_ImplementedSoftDelete")]
    partial class ImplementedSoftDelete
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cars.DAL.Models.Bodytype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bodytypes");
                });

            modelBuilder.Entity("Cars.DAL.Models.CarV2", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BodyTypeId")
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ColorId")
                        .HasColumnType("int");

                    b.Property<string>("Condition")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ExternalId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<int?>("ModelId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("PrimaryPhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Vin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BodyTypeId");

                    b.HasIndex("ColorId");

                    b.HasIndex("ModelId");

                    b.ToTable("CarsV2");
                });

            modelBuilder.Entity("Cars.DAL.Models.Color", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Cars.DAL.Models.Make", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Makes");
                });

            modelBuilder.Entity("Cars.DAL.Models.Model", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MakeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MakeId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("Cars.DAL.Models.PhotoUrl", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarV2Id")
                        .HasColumnType("int");

                    b.Property<string>("Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<int>("SourceType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarV2Id");

                    b.ToTable("PhotoUrls");
                });

            modelBuilder.Entity("Cars.DAL.Models.CarV2", b =>
                {
                    b.HasOne("Cars.DAL.Models.Bodytype", "BodyType")
                        .WithMany("Cars")
                        .HasForeignKey("BodyTypeId");

                    b.HasOne("Cars.DAL.Models.Color", "Color")
                        .WithMany("Cars")
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Cars.DAL.Models.Model", "Model")
                        .WithMany("Cars")
                        .HasForeignKey("ModelId");

                    b.Navigation("BodyType");

                    b.Navigation("Color");

                    b.Navigation("Model");
                });

            modelBuilder.Entity("Cars.DAL.Models.Model", b =>
                {
                    b.HasOne("Cars.DAL.Models.Make", "Make")
                        .WithMany("Models")
                        .HasForeignKey("MakeId");

                    b.Navigation("Make");
                });

            modelBuilder.Entity("Cars.DAL.Models.PhotoUrl", b =>
                {
                    b.HasOne("Cars.DAL.Models.CarV2", null)
                        .WithMany("PhotoUrls")
                        .HasForeignKey("CarV2Id");
                });

            modelBuilder.Entity("Cars.DAL.Models.Bodytype", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("Cars.DAL.Models.CarV2", b =>
                {
                    b.Navigation("PhotoUrls");
                });

            modelBuilder.Entity("Cars.DAL.Models.Color", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("Cars.DAL.Models.Make", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("Cars.DAL.Models.Model", b =>
                {
                    b.Navigation("Cars");
                });
#pragma warning restore 612, 618
        }
    }
}
