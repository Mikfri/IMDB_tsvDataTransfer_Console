using IMDB_EfDbCons.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=IMDb_DB; Integrated Security=True; Connect Timeout=30; Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //--------------------Konfiguration for forholdet mellem Person og MovieBase--------------------
            modelBuilder.Entity<BlockBuster>()
                .HasKey(bb => new { bb.Nconst, bb.Tconst });

            modelBuilder.Entity<BlockBuster>()
                .HasOne(bb => bb.Person)
                .WithMany(p => p.BlockBusters)
                .HasForeignKey(bb => bb.Nconst);

            modelBuilder.Entity<BlockBuster>()
                .HasOne(bb => bb.MovieBase)
                .WithMany()
                .HasForeignKey(bb => bb.Tconst);

            //-------------------Konfiguration for forholdet mellem Person og PersonalCareer-------------------
            modelBuilder.Entity<PersonalCareer>()
                .HasKey(pc => new { pc.Nconst, pc.PrimProf });

            modelBuilder.Entity<PersonalCareer>()
                .HasOne(pc => pc.Person)
                .WithMany(p => p.PersonalCareers)
                .HasForeignKey(pc => pc.Nconst);

            modelBuilder.Entity<PersonalCareer>()
                .HasOne(pc => pc.Profession)
                .WithMany()
                .HasForeignKey(pc => pc.PrimProf);
        }
              

        public DbSet<Person> Persons { get; set; }
        public DbSet<Profession> Professions { get; set; }
        public DbSet<PersonalCareer> PersonalCareers { get; set; }
        public DbSet<MovieBase> MovieBases { get; set; }
        public DbSet<BlockBuster> PersonalBlockbusters { get; set; }
    }
}
