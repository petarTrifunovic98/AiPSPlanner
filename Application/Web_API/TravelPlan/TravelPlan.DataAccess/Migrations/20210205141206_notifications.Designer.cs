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
    [Migration("20210205141206_notifications")]
    partial class notifications
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

                    b.Property<int?>("LocationId")
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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.AddOn", b =>
                {
                    b.Property<int>("AddOnId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("VotableId")
                        .HasColumnType("int");

                    b.HasKey("AddOnId");

                    b.HasIndex("VotableId")
                        .IsUnique();

                    b.ToTable("AddOns");

                    b.HasDiscriminator<string>("Discriminator").HasValue("AddOn");
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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("ItemName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
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

                    b.Property<int>("AddOnId")
                        .HasColumnType("int");

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

                    b.HasIndex("AddOnId");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.TripType", b =>
                {
                    b.Property<int>("TripTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StandardList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TripId")
                        .HasColumnType("int");

                    b.HasKey("TripTypeId");

                    b.HasIndex("TripId")
                        .IsUnique();

                    b.ToTable("TripTypes");

                    b.HasDiscriminator<string>("Discriminator").HasValue("TripType");
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

                    b.ToTable("Votables");
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

                    b.ToTable("Votes");
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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.OtherAddOn", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.AddOn");

                    b.HasDiscriminator().HasValue("OtherAddOn");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SeaAddOn", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.AddOn");

                    b.HasDiscriminator().HasValue("SeaAddOn");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SpaAddOn", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.AddOn");

                    b.HasDiscriminator().HasValue("SpaAddOn");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.WinterAddOn", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.AddOn");

                    b.HasDiscriminator().HasValue("WinterAddOn");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.OtherType", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TripType");

                    b.HasDiscriminator().HasValue("OtherType");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SeaType", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TripType");

                    b.HasDiscriminator().HasValue("SeaType");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SpaType", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TripType");

                    b.HasDiscriminator().HasValue("SpaType");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.WinterType", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TripType");

                    b.HasDiscriminator().HasValue("WinterType");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SeaDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SeaAddOn");

                    b.Property<int>("DecoratorId")
                        .HasColumnType("int");

                    b.HasIndex("DecoratorId");

                    b.HasDiscriminator().HasValue("SeaDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SpaDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SpaAddOn");

                    b.Property<int>("DecoratorId")
                        .HasColumnType("int")
                        .HasColumnName("SpaDecorator_DecoratorId");

                    b.HasIndex("DecoratorId");

                    b.HasDiscriminator().HasValue("SpaDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.WinterDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.WinterAddOn");

                    b.Property<int>("DecoratorId")
                        .HasColumnType("int")
                        .HasColumnName("WinterDecorator_DecoratorId");

                    b.HasIndex("DecoratorId");

                    b.HasDiscriminator().HasValue("WinterDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Aquapark", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SeaDecorator");

                    b.HasDiscriminator().HasValue("Aquapark");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Cruise", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SeaDecorator");

                    b.HasDiscriminator().HasValue("Cruise");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SunBeds", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SeaDecorator");

                    b.HasDiscriminator().HasValue("SunBeds");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Waterboard", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SeaDecorator");

                    b.HasDiscriminator().HasValue("Waterboard");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.BikeRent", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SpaDecorator");

                    b.HasDiscriminator().HasValue("BikeRent");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.ScooterRent", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SpaDecorator");

                    b.HasDiscriminator().HasValue("ScooterRent");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.TrainTour", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SpaDecorator");

                    b.HasDiscriminator().HasValue("TrainTour");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Walk", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SpaDecorator");

                    b.HasDiscriminator().HasValue("Walk");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SkiEquipment", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.WinterDecorator");

                    b.HasDiscriminator().HasValue("SkiEquipment");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SkiPass", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.WinterDecorator");

                    b.HasDiscriminator().HasValue("SkiPass");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.CruiseDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.Cruise");

                    b.Property<int>("Lvl1DependId")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("CruiseDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.TrainTourDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TrainTour");

                    b.Property<int>("Lvl1DependId")
                        .HasColumnType("int")
                        .HasColumnName("TrainTourDecorator_Lvl1DependId");

                    b.HasDiscriminator().HasValue("TrainTourDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.WalkDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.Walk");

                    b.Property<int>("Lvl1DependId")
                        .HasColumnType("int")
                        .HasColumnName("WalkDecorator_Lvl1DependId");

                    b.HasDiscriminator().HasValue("WalkDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipment");

                    b.Property<int>("Lvl1DependId")
                        .HasColumnType("int")
                        .HasColumnName("SkiEquipmentDecorator_Lvl1DependId");

                    b.HasDiscriminator().HasValue("SkiEquipmentDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Breakfast", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.CruiseDecorator");

                    b.HasDiscriminator().HasValue("Breakfast");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Lunch", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.CruiseDecorator");

                    b.HasDiscriminator().HasValue("Lunch");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Meal", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.TrainTourDecorator");

                    b.HasDiscriminator().HasValue("Meal");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.TourGuide", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.WalkDecorator");

                    b.HasDiscriminator().HasValue("TourGuide");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SkiBoots", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator");

                    b.HasDiscriminator().HasValue("SkiBoots");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SkiPoles", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator");

                    b.HasDiscriminator().HasValue("SkiPoles");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Skis", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator");

                    b.HasDiscriminator().HasValue("Skis");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Sled", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator");

                    b.HasDiscriminator().HasValue("Sled");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Snowboard", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.SkiEquipmentDecorator");

                    b.HasDiscriminator().HasValue("Snowboard");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.BreakfastDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.Breakfast");

                    b.Property<int>("Lvl2DependId")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue("BreakfastDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.LunchDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.Lunch");

                    b.Property<int>("Lvl2DependId")
                        .HasColumnType("int")
                        .HasColumnName("LunchDecorator_Lvl2DependId");

                    b.HasDiscriminator().HasValue("LunchDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.MealDecorator", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.Meal");

                    b.Property<int>("Lvl2DependId")
                        .HasColumnType("int")
                        .HasColumnName("MealDecorator_Lvl2DependId");

                    b.HasDiscriminator().HasValue("MealDecorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Coffee", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.BreakfastDecorator");

                    b.HasDiscriminator().HasValue("Coffee");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Juice", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.BreakfastDecorator");

                    b.HasDiscriminator().HasValue("Juice");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Tea", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.BreakfastDecorator");

                    b.HasDiscriminator().HasValue("Tea");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Dessert", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.LunchDecorator");

                    b.HasDiscriminator().HasValue("Dessert");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Wine", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.LunchDecorator");

                    b.HasDiscriminator().HasValue("Wine");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Pogaca", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.MealDecorator");

                    b.HasDiscriminator().HasValue("Pogaca");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Schnapps", b =>
                {
                    b.HasBaseType("TravelPlan.DataAccess.Entities.MealDecorator");

                    b.HasDiscriminator().HasValue("Schnapps");
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
                        .OnDelete(DeleteBehavior.NoAction);

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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.AddOn", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Votable", "Votable")
                        .WithOne()
                        .HasForeignKey("TravelPlan.DataAccess.Entities.AddOn", "VotableId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Votable");
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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Notification", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.User", "User")
                        .WithMany("MyNotifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Trip", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.AddOn", "AddOn")
                        .WithMany()
                        .HasForeignKey("AddOnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddOn");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.TripType", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.Trip", "Trip")
                        .WithOne("TripType")
                        .HasForeignKey("TravelPlan.DataAccess.Entities.TripType", "TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trip");
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

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SeaDecorator", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.SeaAddOn", "Decorator")
                        .WithMany()
                        .HasForeignKey("DecoratorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Decorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.SpaDecorator", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.SpaAddOn", "Decorator")
                        .WithMany()
                        .HasForeignKey("DecoratorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Decorator");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.WinterDecorator", b =>
                {
                    b.HasOne("TravelPlan.DataAccess.Entities.WinterAddOn", "Decorator")
                        .WithMany()
                        .HasForeignKey("DecoratorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Decorator");
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

                    b.Navigation("TripType");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.User", b =>
                {
                    b.Navigation("MyItems");

                    b.Navigation("MyNotifications");
                });

            modelBuilder.Entity("TravelPlan.DataAccess.Entities.Votable", b =>
                {
                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}
