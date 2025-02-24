﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NL_THUD.Data;

#nullable disable

namespace NL_THUD.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250215084658_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("NL_THUD.Models.AcademicTranscript", b =>
                {
                    b.Property<Guid>("AcademicTranscript_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("AVG_1st")
                        .HasColumnType("real");

                    b.Property<float>("AVG_2st")
                        .HasColumnType("real");

                    b.Property<float>("AVG_Final")
                        .HasColumnType("real");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Conduct")
                        .HasColumnType("int");

                    b.Property<int>("Great")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Performance")
                        .HasColumnType("int");

                    b.Property<string>("TeacherId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AcademicTranscript_Id");

                    b.HasIndex("TeacherId");

                    b.ToTable("AcademicTranscripts");
                });

            modelBuilder.Entity("NL_THUD.Models.Address", b =>
                {
                    b.Property<Guid>("Address_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address_Detail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PersonId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Person_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Province_Id")
                        .HasColumnType("int");

                    b.Property<int>("ProvincesProvince_Id")
                        .HasColumnType("int");

                    b.HasKey("Address_Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("ProvincesProvince_Id");

                    b.ToTable("Addressses");
                });

            modelBuilder.Entity("NL_THUD.Models.Class_Student", b =>
                {
                    b.Property<string>("Student_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Class_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Schedule_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Student_Id", "Class_Id");

                    b.HasIndex("Class_Id");

                    b.HasIndex("Schedule_Id");

                    b.ToTable("Class_Student");
                });

            modelBuilder.Entity("NL_THUD.Models.Classes", b =>
                {
                    b.Property<Guid>("Classes_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Classes_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Classes_Quantity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Classes_Id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("NL_THUD.Models.Districts", b =>
                {
                    b.Property<int>("Districts_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Districts_Id"));

                    b.Property<string>("Districts_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Province_Id")
                        .HasColumnType("int");

                    b.Property<int>("ProvincesProvince_Id")
                        .HasColumnType("int");

                    b.HasKey("Districts_Id");

                    b.HasIndex("ProvincesProvince_Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("NL_THUD.Models.Person", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("nvarchar(21)");

                    b.Property<DateOnly>("DoB")
                        .HasColumnType("date");

                    b.Property<int>("ERole")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpireTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasDiscriminator().HasValue("Person");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("NL_THUD.Models.Provinces", b =>
                {
                    b.Property<int>("Province_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Province_Id"));

                    b.Property<string>("Province_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Province_Id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("NL_THUD.Models.Schedule_Detail", b =>
                {
                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ScheduleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("PeriodNumber")
                        .HasColumnType("int");

                    b.Property<int>("Session")
                        .HasColumnType("int");

                    b.HasKey("SubjectId", "ScheduleId");

                    b.HasIndex("ScheduleId");

                    b.ToTable("Schedule_Detail");
                });

            modelBuilder.Entity("NL_THUD.Models.Schedules", b =>
                {
                    b.Property<Guid>("Schedules_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Schedules_Semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Schedules_Id");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("NL_THUD.Models.Score", b =>
                {
                    b.Property<Guid>("Score_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("Score_1")
                        .HasColumnType("real");

                    b.Property<float>("Score_2")
                        .HasColumnType("real");

                    b.Property<float>("Score_3")
                        .HasColumnType("real");

                    b.Property<float>("Score_final")
                        .HasColumnType("real");

                    b.HasKey("Score_Id");

                    b.ToTable("Scores");
                });

            modelBuilder.Entity("NL_THUD.Models.Student_Score", b =>
                {
                    b.Property<string>("StudentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("ScoreId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SubjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ATId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Semester")
                        .HasColumnType("int");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId", "ScoreId", "SubjectId", "ATId");

                    b.HasIndex("ATId");

                    b.HasIndex("ScoreId");

                    b.HasIndex("SubjectId");

                    b.ToTable("Student_Score");
                });

            modelBuilder.Entity("NL_THUD.Models.Subjects", b =>
                {
                    b.Property<Guid>("Subject_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Subject_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Subject_Id");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher_Class", b =>
                {
                    b.Property<string>("Teacher_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Class_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Teacher_Id", "Class_Id");

                    b.HasIndex("Class_Id");

                    b.ToTable("Teacher_Class");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher_Subject", b =>
                {
                    b.Property<Guid>("Subject_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Teacher_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("Class_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Semester")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Year")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Subject_Id", "Teacher_Id", "Class_Id");

                    b.HasIndex("Class_Id");

                    b.HasIndex("Teacher_Id");

                    b.ToTable("Teacher_Subject");
                });

            modelBuilder.Entity("NL_THUD.Models.Wards", b =>
                {
                    b.Property<int>("Ward_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Ward_Id"));

                    b.Property<int>("Districts_Id")
                        .HasColumnType("int");

                    b.Property<int>("Districts_Id1")
                        .HasColumnType("int");

                    b.Property<string>("Ward_Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Ward_Id");

                    b.HasIndex("Districts_Id1");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("NL_THUD.Models.ManagementStaff", b =>
                {
                    b.HasBaseType("NL_THUD.Models.Person");

                    b.Property<string>("ManagementStaff_Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SystemAdmin_Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ManagementStaff");
                });

            modelBuilder.Entity("NL_THUD.Models.Parents", b =>
                {
                    b.HasBaseType("NL_THUD.Models.Person");

                    b.Property<string>("SystemAdminId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SystemAdmin_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasIndex("SystemAdminId");

                    b.HasDiscriminator().HasValue("Parents");
                });

            modelBuilder.Entity("NL_THUD.Models.Students", b =>
                {
                    b.HasBaseType("NL_THUD.Models.Person");

                    b.Property<string>("ParentId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SystemAdminId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("ParentId")
                        .IsUnique()
                        .HasFilter("[ParentId] IS NOT NULL");

                    b.HasIndex("SystemAdminId");

                    b.ToTable("AspNetUsers", t =>
                        {
                            t.Property("SystemAdminId")
                                .HasColumnName("Students_SystemAdminId");
                        });

                    b.HasDiscriminator().HasValue("Students");
                });

            modelBuilder.Entity("NL_THUD.Models.SystemAdmin", b =>
                {
                    b.HasBaseType("NL_THUD.Models.Person");

                    b.HasDiscriminator().HasValue("SystemAdmin");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher", b =>
                {
                    b.HasBaseType("NL_THUD.Models.Person");

                    b.Property<string>("SystemAdminId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasIndex("SystemAdminId");

                    b.ToTable("AspNetUsers", t =>
                        {
                            t.Property("SystemAdminId")
                                .HasColumnName("Teacher_SystemAdminId");
                        });

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NL_THUD.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NL_THUD.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NL_THUD.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NL_THUD.Models.AcademicTranscript", b =>
                {
                    b.HasOne("NL_THUD.Models.Teacher", "Teacher")
                        .WithMany("AcademicTranscript")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("NL_THUD.Models.Address", b =>
                {
                    b.HasOne("NL_THUD.Models.Person", "Person")
                        .WithMany("Addresses")
                        .HasForeignKey("PersonId");

                    b.HasOne("NL_THUD.Models.Provinces", "Provinces")
                        .WithMany("Addresses")
                        .HasForeignKey("ProvincesProvince_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Provinces");
                });

            modelBuilder.Entity("NL_THUD.Models.Class_Student", b =>
                {
                    b.HasOne("NL_THUD.Models.Classes", "Class")
                        .WithMany("Class_Student")
                        .HasForeignKey("Class_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Schedules", "schedules")
                        .WithMany("Class_Students")
                        .HasForeignKey("Schedule_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Students", "Students")
                        .WithMany("Class_Students")
                        .HasForeignKey("Student_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Students");

                    b.Navigation("schedules");
                });

            modelBuilder.Entity("NL_THUD.Models.Districts", b =>
                {
                    b.HasOne("NL_THUD.Models.Provinces", "Provinces")
                        .WithMany("Districts")
                        .HasForeignKey("ProvincesProvince_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Provinces");
                });

            modelBuilder.Entity("NL_THUD.Models.Schedule_Detail", b =>
                {
                    b.HasOne("NL_THUD.Models.Schedules", "Schedules")
                        .WithMany("Schedules_Detail")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Subjects", "Subjects")
                        .WithMany("Schedule_Detail")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedules");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("NL_THUD.Models.Student_Score", b =>
                {
                    b.HasOne("NL_THUD.Models.AcademicTranscript", "AcademicTranscript")
                        .WithMany("StudentScores")
                        .HasForeignKey("ATId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Score", "Score")
                        .WithMany("student_Scores")
                        .HasForeignKey("ScoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Students", "Students")
                        .WithMany("student_Scores")
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Subjects", "Subjects")
                        .WithMany("student_Scores")
                        .HasForeignKey("SubjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AcademicTranscript");

                    b.Navigation("Score");

                    b.Navigation("Students");

                    b.Navigation("Subjects");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher_Class", b =>
                {
                    b.HasOne("NL_THUD.Models.Classes", "Class")
                        .WithMany("TeacherClass")
                        .HasForeignKey("Class_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Teacher", "Teacher")
                        .WithMany("TeacherClass")
                        .HasForeignKey("Teacher_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher_Subject", b =>
                {
                    b.HasOne("NL_THUD.Models.Classes", "Classes")
                        .WithMany("TeacherSubject")
                        .HasForeignKey("Class_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Subjects", "Subjects")
                        .WithMany("Teacher_Subjects")
                        .HasForeignKey("Subject_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.Teacher", "Teacher")
                        .WithMany("TeacherSubject")
                        .HasForeignKey("Teacher_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Classes");

                    b.Navigation("Subjects");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("NL_THUD.Models.Wards", b =>
                {
                    b.HasOne("NL_THUD.Models.Districts", "Districts")
                        .WithMany("Wards")
                        .HasForeignKey("Districts_Id1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Districts");
                });

            modelBuilder.Entity("NL_THUD.Models.Parents", b =>
                {
                    b.HasOne("NL_THUD.Models.SystemAdmin", "SystemAdmin")
                        .WithMany("Parents")
                        .HasForeignKey("SystemAdminId");

                    b.Navigation("SystemAdmin");
                });

            modelBuilder.Entity("NL_THUD.Models.Students", b =>
                {
                    b.HasOne("NL_THUD.Models.Parents", "Parents")
                        .WithOne("Students")
                        .HasForeignKey("NL_THUD.Models.Students", "ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NL_THUD.Models.SystemAdmin", "SystemAdmin")
                        .WithMany("Students")
                        .HasForeignKey("SystemAdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Parents");

                    b.Navigation("SystemAdmin");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher", b =>
                {
                    b.HasOne("NL_THUD.Models.SystemAdmin", "SystemAdmin")
                        .WithMany("Teachers")
                        .HasForeignKey("SystemAdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SystemAdmin");
                });

            modelBuilder.Entity("NL_THUD.Models.AcademicTranscript", b =>
                {
                    b.Navigation("StudentScores");
                });

            modelBuilder.Entity("NL_THUD.Models.Classes", b =>
                {
                    b.Navigation("Class_Student");

                    b.Navigation("TeacherClass");

                    b.Navigation("TeacherSubject");
                });

            modelBuilder.Entity("NL_THUD.Models.Districts", b =>
                {
                    b.Navigation("Wards");
                });

            modelBuilder.Entity("NL_THUD.Models.Person", b =>
                {
                    b.Navigation("Addresses");
                });

            modelBuilder.Entity("NL_THUD.Models.Provinces", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Districts");
                });

            modelBuilder.Entity("NL_THUD.Models.Schedules", b =>
                {
                    b.Navigation("Class_Students");

                    b.Navigation("Schedules_Detail");
                });

            modelBuilder.Entity("NL_THUD.Models.Score", b =>
                {
                    b.Navigation("student_Scores");
                });

            modelBuilder.Entity("NL_THUD.Models.Subjects", b =>
                {
                    b.Navigation("Schedule_Detail");

                    b.Navigation("Teacher_Subjects");

                    b.Navigation("student_Scores");
                });

            modelBuilder.Entity("NL_THUD.Models.Parents", b =>
                {
                    b.Navigation("Students")
                        .IsRequired();
                });

            modelBuilder.Entity("NL_THUD.Models.Students", b =>
                {
                    b.Navigation("Class_Students");

                    b.Navigation("student_Scores");
                });

            modelBuilder.Entity("NL_THUD.Models.SystemAdmin", b =>
                {
                    b.Navigation("Parents");

                    b.Navigation("Students");

                    b.Navigation("Teachers");
                });

            modelBuilder.Entity("NL_THUD.Models.Teacher", b =>
                {
                    b.Navigation("AcademicTranscript");

                    b.Navigation("TeacherClass");

                    b.Navigation("TeacherSubject");
                });
#pragma warning restore 612, 618
        }
    }
}
