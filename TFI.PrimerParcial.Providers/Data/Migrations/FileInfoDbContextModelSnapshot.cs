﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TFI.PrimerParcial.Source.Data;

namespace TFI.PrimerParcial.Source.Data.Migrations
{
    [DbContext(typeof(FileInfoDbContext))]
    partial class FileInfoDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TFI.PrimerParcial.Domain.FileUploadInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("PrintDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("FileUploadInfo");
                });
#pragma warning restore 612, 618
        }
    }
}