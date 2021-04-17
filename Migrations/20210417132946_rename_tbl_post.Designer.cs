﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WorkAppReactAPI.Data;

namespace WorkAppReactAPI.Migrations
{
    [DbContext(typeof(WorkerServiceContext))]
    [Migration("20210417132946_rename_tbl_post")]
    partial class rename_tbl_post
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
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Feelback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Feelbacks");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.HistoryAdress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AddressText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Positon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("HistoryAdress");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FinishAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Positon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("WorkerOfServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("WorkerOfServiceId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Preferential", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Percents")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Preferentials");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.PreferentialOfService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PreferentialId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PreferentialId");

                    b.HasIndex("ServiceId");

                    b.ToTable("PreferentialOfServices");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FunctionCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("isDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("isExport")
                        .HasColumnType("bit");

                    b.Property<bool>("isImport")
                        .HasColumnType("bit");

                    b.Property<bool>("isInsert")
                        .HasColumnType("bit");

                    b.Property<bool>("isSearch")
                        .HasColumnType("bit");

                    b.Property<bool>("isUpdate")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Service", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("RewardPoints")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isOnline")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.UserRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Worker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CMND")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrlOfCMND")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.WorkerOfService", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("FeelbackId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ServiceId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WorkerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("isApproval")
                        .HasColumnType("bit");

                    b.Property<bool>("isOnline")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("FeelbackId");

                    b.HasIndex("ServiceId");

                    b.HasIndex("WorkerId");

                    b.ToTable("WorkerOfServices");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Customer", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.User", "User")
                        .WithMany("Customers")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.HistoryAdress", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Customer", "Customer")
                        .WithMany("HistotyAddress")
                        .HasForeignKey("CustomerId");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Post", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Customer", "Customer")
                        .WithMany("Bookings")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkAppReactAPI.Models.WorkerOfService", "WorkerOfService")
                        .WithMany("Bookings")
                        .HasForeignKey("WorkerOfServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("WorkerOfService");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.PreferentialOfService", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Preferential", "Preferential")
                        .WithMany("PreferentialOfServices")
                        .HasForeignKey("PreferentialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkAppReactAPI.Models.Service", "Service")
                        .WithMany("PreferentialOfServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Preferential");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.UserRole", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId");

                    b.HasOne("WorkAppReactAPI.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId");

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Worker", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.User", "User")
                        .WithMany("Workers")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.WorkerOfService", b =>
                {
                    b.HasOne("WorkAppReactAPI.Models.Feelback", "Feelback")
                        .WithMany("WorkerOfServices")
                        .HasForeignKey("FeelbackId");

                    b.HasOne("WorkAppReactAPI.Models.Service", "Service")
                        .WithMany("WorkerOfServices")
                        .HasForeignKey("ServiceId");

                    b.HasOne("WorkAppReactAPI.Models.Worker", "Worker")
                        .WithMany("WorkerOfCategories")
                        .HasForeignKey("WorkerId");

                    b.Navigation("Feelback");

                    b.Navigation("Service");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Customer", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("HistotyAddress");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Feelback", b =>
                {
                    b.Navigation("WorkerOfServices");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Preferential", b =>
                {
                    b.Navigation("PreferentialOfServices");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Service", b =>
                {
                    b.Navigation("PreferentialOfServices");

                    b.Navigation("WorkerOfServices");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.User", b =>
                {
                    b.Navigation("Customers");

                    b.Navigation("UserRoles");

                    b.Navigation("Workers");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.Worker", b =>
                {
                    b.Navigation("WorkerOfCategories");
                });

            modelBuilder.Entity("WorkAppReactAPI.Models.WorkerOfService", b =>
                {
                    b.Navigation("Bookings");
                });
#pragma warning restore 612, 618
        }
    }
}
