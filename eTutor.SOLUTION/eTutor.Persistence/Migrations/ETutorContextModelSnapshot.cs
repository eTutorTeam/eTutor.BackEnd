﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eTutor.Persistence;

namespace eTutor.Persistence.Migrations
{
    [DbContext(typeof(ETutorContext))]
    partial class ETutorContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("StudentId");

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

            modelBuilder.Entity("eTutor.Core.Models.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("BirthDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Parents");
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

                    b.Property<int>("StudentId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.HasIndex("StudentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("eTutor.Core.Models.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Calification");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("MeetingId");

                    b.Property<int?>("StudentId");

                    b.Property<int?>("TutorId");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TutorId");

                    b.ToTable("Ratings");
                });

            modelBuilder.Entity("eTutor.Core.Models.Role", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "admin",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "tutor",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "student",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769),
                            Name = "parent",
                            UpdatedDate = new DateTime(2019, 11, 2, 12, 12, 22, 916, DateTimeKind.Local).AddTicks(8769)
                        });
                });

            modelBuilder.Entity("eTutor.Core.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("BirthDate");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("Gender");

                    b.Property<string>("LastName");

                    b.Property<float>("Latitude");

                    b.Property<float>("Longitude");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Students");
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

            modelBuilder.Entity("eTutor.Core.Models.Tutor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Tutors");
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

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<int>("Gender");

                    b.Property<bool>("IsEmailValidated");

                    b.Property<bool>("IsTemporaryPassword");

                    b.Property<string>("LastName");

                    b.Property<string>("Name");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eTutor.Core.Models.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<int>("RoleId");

                    b.Property<int?>("RoleId1");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId1");

                    b.HasIndex("UserId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("eTutor.Core.Models.Invoice", b =>
                {
                    b.HasOne("eTutor.Core.Models.Meeting", "Meeting")
                        .WithMany("Invoices")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Student", "Student")
                        .WithMany("Invoices")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Meeting", b =>
                {
                    b.HasOne("eTutor.Core.Models.ParentAutorization", "Type")
                        .WithMany("Meetings")
                        .HasForeignKey("ParentAutorizationId");

                    b.HasOne("eTutor.Core.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Tutor", "Tutor")
                        .WithMany()
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Parent", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithOne("Parent")
                        .HasForeignKey("eTutor.Core.Models.Parent", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentAutorization", b =>
                {
                    b.HasOne("eTutor.Core.Models.Parent", "Parent")
                        .WithMany("Autorizations")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.ParentStudent", b =>
                {
                    b.HasOne("eTutor.Core.Models.Parent", "Parent")
                        .WithMany("Students")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Student", "Student")
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

                    b.HasOne("eTutor.Core.Models.Student", "Student")
                        .WithMany("Payments")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Rating", b =>
                {
                    b.HasOne("eTutor.Core.Models.Meeting", "Meeting")
                        .WithMany("Ratings")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Student", "Student")
                        .WithMany("Ratings")
                        .HasForeignKey("StudentId");

                    b.HasOne("eTutor.Core.Models.Tutor", "Tutor")
                        .WithMany("Ratings")
                        .HasForeignKey("TutorId");
                });

            modelBuilder.Entity("eTutor.Core.Models.Student", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithOne("Student")
                        .HasForeignKey("eTutor.Core.Models.Student", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.TopicInterest", b =>
                {
                    b.HasOne("eTutor.Core.Models.Student", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.Tutor", b =>
                {
                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithOne("Tutor")
                        .HasForeignKey("eTutor.Core.Models.Tutor", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.TutorTopic", b =>
                {
                    b.HasOne("eTutor.Core.Models.Topic", "Topic")
                        .WithMany("Tutors")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("eTutor.Core.Models.Tutor", "Tutor")
                        .WithMany("Topics")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("eTutor.Core.Models.UserRole", b =>
                {
                    b.HasOne("eTutor.Core.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId1");

                    b.HasOne("eTutor.Core.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
