﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkAppReactAPI.Data;
using WorkAppReactAPI.Models;

namespace WorkAppReactAPI.Migrations
{
    [DbContext(typeof(WorkerServiceContext))]
    [Migration("20210326125300_night")]
    partial class night
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WorkAppReactAPI.Models.Customer", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<Guid?>("Userid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("id");

                    b.HasIndex("Userid");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.HistoryExChange", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("id");

                    b.ToTable("HistoryExChange");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Order", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Customerid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("HistoryExChangeid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("OrderCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.HasIndex("Customerid");

                    b.HasIndex("HistoryExChangeid");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.User", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Customer", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("Userid");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Order", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Customer", null)
                        .WithMany("Orders")
                        .HasForeignKey("Customerid");

                    b.HasOne("WorkAppReactAPI.Models.HistoryExChange", "HistoryExChange")
                        .WithMany()
                        .HasForeignKey("HistoryExChangeid");

                    b.Navigation("HistoryExChange");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Customer", b =>
                {
                    b.Navigation("Orders");
                });
            
#pragma warning restore 612, 618
        }
    }
}
