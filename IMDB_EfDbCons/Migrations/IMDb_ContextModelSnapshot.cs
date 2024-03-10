﻿// <auto-generated />
using System;
using IMDB_EfDbCons.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IMDB_EfDbCons.Migrations
{
    [DbContext(typeof(IMDb_Context))]
    partial class IMDb_ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IMDB_EfDbCons.Models.BlockBuster", b =>
                {
                    b.Property<string>("Nconst")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Tconst")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Nconst", "Tconst");

                    b.HasIndex("Tconst");

                    b.ToTable("PersonalBlockbusters");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.MovieBase", b =>
                {
                    b.Property<string>("Tconst")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("EndYear")
                        .HasColumnType("int");

                    b.Property<bool>("IsAdult")
                        .HasColumnType("bit");

                    b.Property<string>("OriginalTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrimaryTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("RuntimeMins")
                        .HasColumnType("int");

                    b.Property<int?>("StartYear")
                        .HasColumnType("int");

                    b.Property<string>("TitleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Tconst");

                    b.ToTable("MovieBases");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.Person", b =>
                {
                    b.Property<string>("Nconst")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly?>("BirthYear")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("DeathYear")
                        .HasColumnType("date");

                    b.Property<string>("PrimaryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Nconst");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.PersonalCareer", b =>
                {
                    b.Property<string>("Nconst")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PrimProf")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Nconst", "PrimProf");

                    b.HasIndex("PrimProf");

                    b.ToTable("PersonalCareers");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.Profession", b =>
                {
                    b.Property<string>("PrimaryProfession")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PrimaryProfession");

                    b.ToTable("Professions");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.BlockBuster", b =>
                {
                    b.HasOne("IMDB_EfDbCons.Models.Person", "Person")
                        .WithMany("BlockBusters")
                        .HasForeignKey("Nconst")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IMDB_EfDbCons.Models.MovieBase", "MovieBase")
                        .WithMany()
                        .HasForeignKey("Tconst")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MovieBase");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.PersonalCareer", b =>
                {
                    b.HasOne("IMDB_EfDbCons.Models.Person", "Person")
                        .WithMany("PersonalCareers")
                        .HasForeignKey("Nconst")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("IMDB_EfDbCons.Models.Profession", "Profession")
                        .WithMany()
                        .HasForeignKey("PrimProf")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Profession");
                });

            modelBuilder.Entity("IMDB_EfDbCons.Models.Person", b =>
                {
                    b.Navigation("BlockBusters");

                    b.Navigation("PersonalCareers");
                });
#pragma warning restore 612, 618
        }
    }
}
