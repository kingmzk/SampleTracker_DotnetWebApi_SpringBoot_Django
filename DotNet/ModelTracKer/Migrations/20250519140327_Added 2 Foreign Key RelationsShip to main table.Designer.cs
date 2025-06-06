﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModelTracKer.Data;


#nullable disable

namespace ModelTracKer.Migrations
{
    [DbContext(typeof(TrackerDbContext))]
    [Migration("20250519140327_Added 2 Foreign Key RelationsShip to main table")]
    partial class Added2ForeignKeyRelationsShiptomaintable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ModelTracKer.Models.Accelerator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("acclerator");
                });

            modelBuilder.Entity("ModelTracKer.Models.Competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("competition");
                });

            modelBuilder.Entity("ModelTracKer.Models.GenAiTool", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool?>("isDisabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("GenAiTool");
                });

            modelBuilder.Entity("ModelTracKer.Models.MicroService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("microservice");
                });

            modelBuilder.Entity("ModelTracKer.Models.ReasonForNoGenAiAdoptation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool?>("isDisabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ReasonForNoGenAiAdoptation");
                });

            modelBuilder.Entity("ModelTracKer.Models.Tracker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Client_Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("GenAiAdoptation")
                        .HasColumnType("bit");

                    b.Property<int>("GenAiTool_Id")
                        .HasColumnType("int");

                    b.Property<decimal>("Investment")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ReasonForNoGenAiAdoptation_Id")
                        .HasColumnType("int");

                    b.Property<string>("Tracker_Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Tracker_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GenAiTool_Id");

                    b.HasIndex("ReasonForNoGenAiAdoptation_Id");

                    b.ToTable("tracker");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_Accelerator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AcceleratorId")
                        .HasColumnType("int");

                    b.Property<int>("TrackerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AcceleratorId");

                    b.HasIndex("TrackerId");

                    b.ToTable("opp_accelerator");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("competion_id")
                        .HasColumnType("int");

                    b.Property<int>("competition_id")
                        .HasColumnType("int");

                    b.Property<int>("tracker_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("competion_id");

                    b.HasIndex("tracker_id");

                    b.ToTable("opp_competition");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_microservice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("microservice_id")
                        .HasColumnType("int");

                    b.Property<int>("tracker_id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("microservice_id");

                    b.HasIndex("tracker_id");

                    b.ToTable("opp_microservice");
                });

            modelBuilder.Entity("ModelTracKer.Models.Tracker", b =>
                {
                    b.HasOne("ModelTracKer.Models.GenAiTool", "GenAiTool")
                        .WithMany("trackers")
                        .HasForeignKey("GenAiTool_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelTracKer.Models.ReasonForNoGenAiAdoptation", "ReasonForNoGenAiAdoptation")
                        .WithMany("Trackers")
                        .HasForeignKey("ReasonForNoGenAiAdoptation_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GenAiTool");

                    b.Navigation("ReasonForNoGenAiAdoptation");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_Accelerator", b =>
                {
                    b.HasOne("ModelTracKer.Models.Accelerator", "accelerators")
                        .WithMany("OppAccelerators")
                        .HasForeignKey("AcceleratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelTracKer.Models.Tracker", "trackers")
                        .WithMany("oppAccelerators")
                        .HasForeignKey("TrackerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("accelerators");

                    b.Navigation("trackers");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_competition", b =>
                {
                    b.HasOne("ModelTracKer.Models.Competition", "competition")
                        .WithMany("oppCompetitions")
                        .HasForeignKey("competion_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelTracKer.Models.Tracker", "tracker")
                        .WithMany("oppCompetition")
                        .HasForeignKey("tracker_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("competition");

                    b.Navigation("tracker");
                });

            modelBuilder.Entity("ModelTracKer.Models.opp_microservice", b =>
                {
                    b.HasOne("ModelTracKer.Models.MicroService", "microService")
                        .WithMany("OppMicroservice")
                        .HasForeignKey("microservice_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ModelTracKer.Models.Tracker", "trackers")
                        .WithMany("oppMicroservices")
                        .HasForeignKey("tracker_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("microService");

                    b.Navigation("trackers");
                });

            modelBuilder.Entity("ModelTracKer.Models.Accelerator", b =>
                {
                    b.Navigation("OppAccelerators");
                });

            modelBuilder.Entity("ModelTracKer.Models.Competition", b =>
                {
                    b.Navigation("oppCompetitions");
                });

            modelBuilder.Entity("ModelTracKer.Models.GenAiTool", b =>
                {
                    b.Navigation("trackers");
                });

            modelBuilder.Entity("ModelTracKer.Models.MicroService", b =>
                {
                    b.Navigation("OppMicroservice");
                });

            modelBuilder.Entity("ModelTracKer.Models.ReasonForNoGenAiAdoptation", b =>
                {
                    b.Navigation("Trackers");
                });

            modelBuilder.Entity("ModelTracKer.Models.Tracker", b =>
                {
                    b.Navigation("oppAccelerators");

                    b.Navigation("oppCompetition");

                    b.Navigation("oppMicroservices");
                });
#pragma warning restore 612, 618
        }
    }
}
