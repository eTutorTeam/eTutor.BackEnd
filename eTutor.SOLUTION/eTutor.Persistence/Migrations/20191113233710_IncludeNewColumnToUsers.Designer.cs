﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eTutor.Persistence;

namespace eTutor.Persistence.Migrations
{
    [DbContext(typeof(ETutorContext))]
    [Migration("20191113233710_IncludeNewColumnToUsers")]
    partial class IncludeNewColumnToUsers
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("eTutor.Core.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("MeetingId");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("eTutor.Core.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("EndDateTime");

                    b.Property<int?>("ParentAutorizationId");

                    b.Property<DateTime>("StartDateTime");

                    b.Property<int>("Status");

                    b.Property<int>("StudentId");

                    b.Property<int>("TopicId");

                    b.Property<int>("TutorId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("ParentAutorizationId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TopicId");

                    b.HasIndex("TutorId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentAutorization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AuthorizationDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("ParentId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("ParentAutorizations");
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentStudent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("ParentId");

                    b.Property<int>("Relationship");

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("StudentId");

                    b.ToTable("ParentStudents");
                });

            modelBuilder.Entity("eTutor.Core.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("InvoiceId");

                    b.Property<double>("PayedAmount");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("UserId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("eTutor.Core.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Calification");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("MeetingId");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("UserId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("eTutor.Core.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ConcurrencyStamp = "935b8d1f-9f59-41c9-ac6c-c3fd08e5571e",
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "admin",
                            NormalizedName = "admin",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 2,
                            ConcurrencyStamp = "ec87cae6-5939-4ab7-8621-432a46ba6b90",
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "tutor",
                            NormalizedName = "tutor",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 3,
                            ConcurrencyStamp = "8f40a0a6-af30-4e7a-b022-6bfdc9204d2a",
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "student",
                            NormalizedName = "student",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 4,
                            ConcurrencyStamp = "467922d2-8726-4e1a-8ae8-1a9e4fc89d3c",
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "parent",
                            NormalizedName = "parent",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        });
                });

            modelBuilder.Entity("eTutor.Core.Models.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<int>("RoleId");

                    b.Property<int?>("RoleId1");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("RoleId1");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("eTutor.Core.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("eTutor.Core.Models.TopicInterest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("StudentId");

                    b.Property<int>("TopicId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("StudentId");

                    b.HasIndex("TopicId");

                    b.ToTable("TopicInterests");
                });

            modelBuilder.Entity("eTutor.Core.Models.TutorTopic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("TopicId");

                    b.Property<int>("TutorId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.HasIndex("TutorId");

                    b.ToTable("TutorTopics");
                });

            modelBuilder.Entity("eTutor.Core.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Address");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int>("Gender");

                    b.Property<bool>("IsEmailValidated");

                    b.Property<bool>("IsTemporaryPassword");

                    b.Property<string>("LastName");

                    b.Property<float?>("Latitude");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<float?>("Longitude");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PersonalId");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.Property<int?>("UserId1");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ProviderDisplayName");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.Property<int?>("UserId1");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasAlternateKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserRole", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("RoleId1");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int?>("UserId1");

                    b.HasKey("UserId", "RoleId");

                    b.HasAlternateKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("RoleId1");

                    b.HasIndex("UserId1");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserToken", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int?>("UserId1");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.HasAlternateKey("Id");

                    b.HasIndex("UserId1");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("eTutor.Core.Models.Invoice", b =>
                {
                    b.HasOne("eTutor.Core.Models.Meeting", "Meeting")
                        .WithMany("Invoices")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithMany("Invoices")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Meeting", b =>
                {
                    b.HasOne("eTutor.Core.Models.ParentAutorization", "Type")
                        .WithMany("Meetings")
                        .HasForeignKey("ParentAutorizationId");

                    b.HasOne("eTutor.Core.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "Tutor")
                        .WithMany()
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentAutorization", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "Parent")
                        .WithMany("Autorizations")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentStudent", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "Student")
                        .WithMany("Parents")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Payment", b =>
                {
                    b.HasOne("eTutor.Core.Models.Invoice", "Invoice")
                        .WithMany("Payments")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Rating", b =>
                {
                    b.HasOne("eTutor.Core.Models.Meeting", "Meeting")
                        .WithMany("Ratings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithMany("Ratings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.RoleClaim", b =>
                {
                    b.HasOne("eTutor.Core.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId1");
                });

            modelBuilder.Entity("eTutor.Core.Models.TopicInterest", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.TutorTopic", b =>
                {
                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany("Tutors")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User", "Tutor")
                        .WithMany("TutorTopics")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.UserClaim", b =>
                {
                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserLogin", b =>
                {
                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserRole", b =>
                {
                    b.HasOne("eTutor.Core.Models.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId1");

                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserToken", b =>
                {
                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId1");
                });
#pragma warning restore 612, 618
        }
    }
}
