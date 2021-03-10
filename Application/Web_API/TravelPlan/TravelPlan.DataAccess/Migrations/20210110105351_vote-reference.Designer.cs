﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TravelPlan.DataAccess;

namespace TravelPlan.DataAccess.Migrations
{
    [DbContext(typeof(TravelPlanDbContext))]
    [Migration("20210110105351_vote-reference")]
    partial class votereference
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.Property<int>("MembersUserId")
                        .HasColumnType("int");

                    b.Property<int>("MyTeamsTeamId")
                        .HasColumnType("int");

                    b.HasKey("MembersUserId", "MyTeamsTeamId");

                    b.HasIndex("MyTeamsTeamId");

                    b.ToTable("TeamUser");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Accommodation", b =>
                {
                    b.Property<int>("AccommodationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("VotableId")
                        .HasColumnType("int");

                    b.HasKey("AccommodationId");

                    b.HasIndex("LocationId");

                    b.HasIndex("VotableId");

                    b.ToTable("Accommodations");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.AccommodationPicture", b =>
                {
                    b.Property<int>("AccommodationPictureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("AccommodationId")
                        .HasColumnType("int");

                    b.Property<string>("Picture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccommodationPictureId");

                    b.HasIndex("AccommodationId");

                    b.ToTable("AccommodationPictures");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Checked")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.Property<string>("Unit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ItemId");

                    b.HasIndex("TripId");

                    b.HasIndex("UserId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.Property<int>("VotableId")
                        .HasColumnType("int");

                    b.HasKey("LocationId");

                    b.HasIndex("TripId");

                    b.HasIndex("VotableId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TeamId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Trip", b =>
                {
                    b.Property<int>("TripId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("TripId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Votable", b =>
                {
                    b.Property<int>("VotableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("NegativeVotes")
                        .HasColumnType("int");

                    b.Property<int>("PositiveVotes")
                        .HasColumnType("int");

                    b.HasKey("VotableId");

                    b.ToTable("Votable");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<bool>("Positive")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("VotableId")
                        .HasColumnType("int");

                    b.HasKey("VoteId");

                    b.HasIndex("UserId");

                    b.HasIndex("VotableId");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.Property<int>("MyTripsTripId")
                        .HasColumnType("int");

                    b.Property<int>("TravelersUserId")
                        .HasColumnType("int");

                    b.HasKey("MyTripsTripId", "TravelersUserId");

                    b.HasIndex("TravelersUserId");

                    b.ToTable("TripUser");
                });

            modelBuilder.Entity("TeamUser", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("MembersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.Team", null)
                        .WithMany()
                        .HasForeignKey("MyTeamsTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Accommodation", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Location", "Location")
                        .WithMany("Accommodations")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.Votable", "Votable")
                        .WithMany()
                        .HasForeignKey("VotableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("Votable");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.AccommodationPicture", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Accommodation", "Accommodation")
                        .WithMany("Pictures")
                        .HasForeignKey("AccommodationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Accommodation");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Item", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Trip", "Trip")
                        .WithMany("ItemList")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.User", "User")
                        .WithMany("MyItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Location", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Trip", "Trip")
                        .WithMany("Locations")
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.Votable", "Votable")
                        .WithMany()
                        .HasForeignKey("VotableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");

                    b.Navigation("Votable");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Vote", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.Votable", "Votable")
                        .WithMany("Votes")
                        .HasForeignKey("VotableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("Votable");
                });

            modelBuilder.Entity("TripUser", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Trip", null)
                        .WithMany()
                        .HasForeignKey("MyTripsTripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TravelPlan.DataAccess.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("TravelersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Accommodation", b =>
                {
                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Location", b =>
                {
                    b.Navigation("Accommodations");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Trip", b =>
                {
                    b.Navigation("ItemList");

                    b.Navigation("Locations");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.User", b =>
                {
                    b.Navigation("MyItems");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Votable", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
