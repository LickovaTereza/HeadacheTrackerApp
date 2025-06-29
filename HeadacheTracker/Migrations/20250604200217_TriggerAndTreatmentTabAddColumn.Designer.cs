﻿// <auto-generated />
using System;
using HeadacheTracker;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HeadacheTracker.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250604200217_TriggerAndTreatmentTabAddColumn")]
    partial class TriggerAndTreatmentTabAddColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HeadacheEntryTreatment", b =>
                {
                    b.Property<int>("HeadacheEntriesId")
                        .HasColumnType("int");

                    b.Property<int>("TreatmentsId")
                        .HasColumnType("int");

                    b.HasKey("HeadacheEntriesId", "TreatmentsId");

                    b.HasIndex("TreatmentsId");

                    b.ToTable("HeadacheEntryTreatment");
                });

            modelBuilder.Entity("HeadacheEntryTrigger", b =>
                {
                    b.Property<int>("HeadacheEntriesId")
                        .HasColumnType("int");

                    b.Property<int>("TriggersId")
                        .HasColumnType("int");

                    b.HasKey("HeadacheEntriesId", "TriggersId");

                    b.HasIndex("TriggersId");

                    b.ToTable("HeadacheEntryTrigger");
                });

            modelBuilder.Entity("HeadacheTracker.Models.HeadacheEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<int>("Intensity")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("HeadacheEntries");
                });

            modelBuilder.Entity("HeadacheTracker.Models.Treatment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Treatments");
                });

            modelBuilder.Entity("HeadacheTracker.Models.Trigger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Triggers");
                });

            modelBuilder.Entity("HeadacheEntryTreatment", b =>
                {
                    b.HasOne("HeadacheTracker.Models.HeadacheEntry", null)
                        .WithMany()
                        .HasForeignKey("HeadacheEntriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeadacheTracker.Models.Treatment", null)
                        .WithMany()
                        .HasForeignKey("TreatmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HeadacheEntryTrigger", b =>
                {
                    b.HasOne("HeadacheTracker.Models.HeadacheEntry", null)
                        .WithMany()
                        .HasForeignKey("HeadacheEntriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HeadacheTracker.Models.Trigger", null)
                        .WithMany()
                        .HasForeignKey("TriggersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
