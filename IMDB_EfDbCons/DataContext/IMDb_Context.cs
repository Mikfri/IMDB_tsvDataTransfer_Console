using IMDB_EfDbCons.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_EfDbCons.DataContext
{
    public class IMDb_Context : DbContext
    {
        public IMDb_Context(DbContextOptions<IMDb_Context> options) : base(options) { }

        public IMDb_Context() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IMDb_DB; Integrated Security=True;");                   
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //--------------------Konfiguration for forholdet mellem Person og MovieBase--------------------
            modelBuilder.Entity<BlockBuster>()
                .HasKey(bb => new { bb.Nconst, bb.Tconst });

            modelBuilder.Entity<BlockBuster>()
                .HasOne(bb => bb.Person)        // en BlockBuster har en Person
                .WithMany(p => p.BlockBusters)  // en Person har mange BlockBusters
                .HasForeignKey(bb => bb.Nconst);// en BlockBuster har en fremmednøgle Nconst

            modelBuilder.Entity<BlockBuster>()
                .HasOne(bb => bb.MovieBase)     // en BlockBuster har en MovieBase
                .WithMany()
                .HasForeignKey(bb => bb.Tconst);// en BlockBuster har en fremmednøgle Tconst

            //-------------------Konfiguration for forholdet mellem Person og Profession-------------------
            modelBuilder.Entity<PersonalCareer>()
                .HasKey(pc => new { pc.Nconst, pc.PrimProf });

            modelBuilder.Entity<PersonalCareer>()
                .HasOne(pc => pc.Person)            // en PersonalCareer har en Person
                .WithMany(p => p.PersonalCareers)   // en Person har mange PersonalCareers
                .HasForeignKey(pc => pc.Nconst);    // en PersonalCareer har en fremmednøgle Nconst

            modelBuilder.Entity<PersonalCareer>()
                .HasOne(pc => pc.Profession)        // en PersonalCareer har en Profession
                .WithMany()
                .HasForeignKey(pc => pc.PrimProf);  // en PersonalCareer har en fremmednøgle PrimProf

            //-------------------Konfiguration for forholdet mellem MovieBase og Genre-------------------
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.Tconst, mg.GenreType });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.MovieBase)         // en MovieGenre har en MovieBase
                .WithMany(mb => mb.MovieGenres)     // en MovieBase har mange MovieGenres
                .HasForeignKey(mg => mg.Tconst);    // en MovieGenre har en fremmednøgle Tconst

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)             // en MovieGenre har en Genre
                .WithMany()
                .HasForeignKey(mg => mg.GenreType); // en MovieGenre har en fremmednøgle GenreType

            //-------------------Configuration for the relationship between MovieBase and Director-------------------
            modelBuilder.Entity<Director>()
                .HasKey(d => new { d.Tconst, d.Nconst });

            modelBuilder.Entity<Director>()
                .HasOne(d => d.MovieBase)       // en Director har en MovieBase
                .WithMany(mb => mb.Directors)   // en MovieBase har mange Directors
                .HasForeignKey(d => d.Tconst);  // en Director har en fremmednøgle Tconst

            modelBuilder.Entity<Director>()
                .HasOne(d => d.Person)          // en Director har en Person
                .WithMany(p => p.Directors)     // en Person har mange Directors
                .HasForeignKey(d => d.Nconst);  // en Director har en fremmednøgle Nconst

            //-------------------Configuration for the relationship between MovieBase and Writer-------------------
            modelBuilder.Entity<Writer>()
                .HasKey(w => new { w.Tconst, w.Nconst });

            modelBuilder.Entity<Writer>()
                .HasOne(w => w.MovieBase)
                .WithMany(mb => mb.Writers)
                .HasForeignKey(w => w.Tconst);

            modelBuilder.Entity<Writer>()
                .HasOne(w => w.Person)
                .WithMany(p => p.Writers)
                .HasForeignKey(w => w.Nconst);
        }

        //--------- title.basics.tsv ---------
        public DbSet<MovieBase> MovieBases { get; set; }
        public DbSet<TitleType> TitleTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }


        //--------- name.basics.tsv ---------
        public DbSet<Person> Persons { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<PersonalCareer> PersonalCareers { get; set; }
        public DbSet<BlockBuster> PersonalBlockbusters { get; set; }

        //--------- title.crew.tsv ---------
        public DbSet<Director> Directors { get; set; }
        public DbSet<Writer> Writers { get; set; }
    }
}
