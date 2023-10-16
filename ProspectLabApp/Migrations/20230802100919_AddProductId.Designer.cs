﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ProspectLabApp.Pages;

#nullable disable

namespace ProspectLabApp.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230802100919_AddProductId")]
    partial class AddProductId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ProspectLabApp.Pages.Product", b =>
                {
                    b.Property<string>("Brand")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Quantity")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Discount")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Brand", "Title", "Quantity", "Discount", "Price");

                    b.ToTable("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
