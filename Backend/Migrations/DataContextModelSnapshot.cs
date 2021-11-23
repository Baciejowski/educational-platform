﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Backend;

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Backend.Models.Answer", b =>
            {
                b.Property<int>("AnswerID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<string>("Argumentation")
                    .HasColumnType("text");

                b.Property<string>("Content")
                    .HasColumnType("text");

                b.Property<bool>("Correct")
                    .HasColumnType("boolean");

                b.Property<int?>("QuestionID")
                    .HasColumnType("integer");

                b.HasKey("AnswerID");

                b.HasIndex("QuestionID");

                b.ToTable("Answers");
            });

            modelBuilder.Entity("Backend.Models.Class", b =>
            {
                b.Property<int>("ClassID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<string>("FriendlyName")
                    .HasColumnType("text");

                b.Property<int?>("TeacherID")
                    .HasColumnType("integer");

                b.HasKey("ClassID");

                b.HasIndex("TeacherID");

                b.ToTable("Classes");
            });

            modelBuilder.Entity("Backend.Models.Game", b =>
            {
                b.Property<int>("GameID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<int?>("ScenarioID")
                    .HasColumnType("integer");

                b.Property<int?>("StudentID")
                    .HasColumnType("integer");

                b.HasKey("GameID");

                b.HasIndex("ScenarioID");

                b.HasIndex("StudentID");

                b.ToTable("Games");
            });

            modelBuilder.Entity("Backend.Models.Question", b =>
            {
                b.Property<int>("QuestionID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<bool?>("BooleanAnswer")
                    .HasColumnType("boolean");

                b.Property<string>("Content")
                    .HasColumnType("text");

                b.Property<byte>("Difficulty")
                    .HasColumnType("smallint");

                b.Property<string>("Hint")
                    .HasColumnType("text");

                b.Property<bool>("IsImportant")
                    .HasColumnType("boolean");

                b.Property<bool>("IsObligatory")
                    .HasColumnType("boolean");

                b.Property<byte>("QuestionType")
                    .HasColumnType("smallint");

                b.HasKey("QuestionID");

                b.ToTable("Questions");
            });

            modelBuilder.Entity("Backend.Models.Scenario", b =>
            {
                b.Property<int>("ScenarioID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<string>("Name")
                    .HasColumnType("text");

                b.Property<int?>("TopicID")
                    .HasColumnType("integer");

                b.Property<string>("Url")
                    .HasColumnType("text");

                b.HasKey("ScenarioID");

                b.HasIndex("TopicID");

                b.ToTable("Scenarios");
            });

            modelBuilder.Entity("Backend.Models.Session", b =>
            {
                b.Property<int>("SessionID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<int>("Attempts")
                    .HasColumnType("integer");

                b.Property<string>("Code")
                    .HasColumnType("text");

                b.Property<DateTime>("EndGame")
                    .HasColumnType("timestamp without time zone");

                b.Property<bool>("RandomCategorization")
                    .HasColumnType("boolean");

                b.Property<bool>("RandomTest")
                    .HasColumnType("boolean");

                b.Property<int?>("ScenarioID")
                    .HasColumnType("integer");

                b.Property<DateTime>("StartGame")
                    .HasColumnType("timestamp without time zone");

                b.Property<int?>("StudentID")
                    .HasColumnType("integer");

                b.HasKey("SessionID");

                b.HasIndex("ScenarioID");

                b.HasIndex("StudentID");

                b.ToTable("Sessions");
            });

            modelBuilder.Entity("Backend.Models.Student", b =>
            {
                b.Property<int>("StudentID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<int?>("ClassID")
                    .HasColumnType("integer");

                b.Property<string>("Email")
                    .HasColumnType("text");

                b.Property<string>("FirstName")
                    .HasColumnType("text");

                b.Property<string>("LastName")
                    .HasColumnType("text");

                b.HasKey("StudentID");

                b.HasIndex("ClassID");

                b.ToTable("Students");
            });

            modelBuilder.Entity("Backend.Models.Teacher", b =>
            {
                b.Property<int>("TeacherID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<string>("AuthName")
                    .HasColumnType("text");

                b.HasKey("TeacherID");

                b.ToTable("Teachers");
            });

            modelBuilder.Entity("Backend.Models.Topic", b =>
            {
                b.Property<int>("TopicID")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("integer")
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                b.Property<int?>("TeacherID")
                    .HasColumnType("integer");

                b.Property<string>("TopicName")
                    .HasColumnType("text");

                b.HasKey("TopicID");

                b.HasIndex("TeacherID");

                b.ToTable("Topics");
            });

            modelBuilder.Entity("QuestionScenario", b =>
            {
                b.Property<int>("QuestionsQuestionID")
                    .HasColumnType("integer");

                b.Property<int>("ScenariosScenarioID")
                    .HasColumnType("integer");

                b.HasKey("QuestionsQuestionID", "ScenariosScenarioID");

                b.HasIndex("ScenariosScenarioID");

                b.ToTable("QuestionScenario");
            });

            modelBuilder.Entity("Backend.Models.Answer", b =>
            {
                b.HasOne("Backend.Models.Question", "Question")
                    .WithMany("ABCDAnswers")
                    .HasForeignKey("QuestionID");

                b.Navigation("Question");
            });

            modelBuilder.Entity("Backend.Models.Class", b =>
            {
                b.HasOne("Backend.Models.Teacher", "Teacher")
                    .WithMany("Classes")
                    .HasForeignKey("TeacherID");

                b.Navigation("Teacher");
            });

            modelBuilder.Entity("Backend.Models.Game", b =>
            {
                b.HasOne("Backend.Models.Scenario", "Scenario")
                    .WithMany("Games")
                    .HasForeignKey("ScenarioID");

                b.HasOne("Backend.Models.Student", "Student")
                    .WithMany("Games")
                    .HasForeignKey("StudentID");

                b.Navigation("Scenario");

                b.Navigation("Student");
            });

            modelBuilder.Entity("Backend.Models.Scenario", b =>
            {
                b.HasOne("Backend.Models.Topic", "Topic")
                    .WithMany("Scenarios")
                    .HasForeignKey("TopicID");

                b.Navigation("Topic");
            });

            modelBuilder.Entity("Backend.Models.Session", b =>
            {
                b.HasOne("Backend.Models.Scenario", "Scenario")
                    .WithMany("Sessions")
                    .HasForeignKey("ScenarioID");

                b.HasOne("Backend.Models.Student", "Student")
                    .WithMany("Sessions")
                    .HasForeignKey("StudentID");

                b.Navigation("Scenario");

                b.Navigation("Student");
            });

            modelBuilder.Entity("Backend.Models.Student", b =>
            {
                b.HasOne("Backend.Models.Class", "Class")
                    .WithMany("Students")
                    .HasForeignKey("ClassID");

                b.Navigation("Class");
            });

            modelBuilder.Entity("Backend.Models.Topic", b =>
            {
                b.HasOne("Backend.Models.Teacher", "Teacher")
                    .WithMany("Topics")
                    .HasForeignKey("TeacherID");

                b.Navigation("Teacher");
            });

            modelBuilder.Entity("QuestionScenario", b =>
            {
                b.HasOne("Backend.Models.Question", null)
                    .WithMany()
                    .HasForeignKey("QuestionsQuestionID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Backend.Models.Scenario", null)
                    .WithMany()
                    .HasForeignKey("ScenariosScenarioID")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Backend.Models.Class", b =>
            {
                b.Navigation("Students");
            });

            modelBuilder.Entity("Backend.Models.Question", b =>
            {
                b.Navigation("ABCDAnswers");
            });

            modelBuilder.Entity("Backend.Models.Scenario", b =>
            {
                b.Navigation("Games");

                b.Navigation("Sessions");
            });

            modelBuilder.Entity("Backend.Models.Student", b =>
            {
                b.Navigation("Games");

                b.Navigation("Sessions");
            });

            modelBuilder.Entity("Backend.Models.Teacher", b =>
            {
                b.Navigation("Classes");

                b.Navigation("Topics");
            });

            modelBuilder.Entity("Backend.Models.Topic", b =>
            {
                b.Navigation("Scenarios");
            });
#pragma warning restore 612, 618
        }
    }
}
