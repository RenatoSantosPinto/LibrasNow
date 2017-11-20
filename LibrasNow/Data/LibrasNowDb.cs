using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibrasNow.Models;

namespace LibrasNow.Data
{
    public class LibrasNowDb : DbContext
    {
        public DbSet<Alternativa> Alternativas { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<ExercicioModulo> ExerciciosModulo { get; set; }
        public DbSet<MaterialEstudo> MateriaisEstudo { get; set; }
        public DbSet<Modulo> Modulos { get; set; }
        public DbSet<ModuloResolvido> ModulosResolvidos { get; set; }          
        public DbSet<Termo> Dicionario { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Video> Videos { get; set; }


        public LibrasNowDb()
        {
            this.Database.AutoTransactionsEnabled = false;
        }

        public LibrasNowDb(DbContextOptions<LibrasNowDb> opcoes) : base(opcoes)
        {
            this.Database.AutoTransactionsEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Alternativa>().ToTable("Alternativa");
            modelBuilder.Entity<Exercicio>().ToTable("Exercicio");
            modelBuilder.Entity<ExercicioModulo>().ToTable("ExercicioModulo");
            modelBuilder.Entity<MaterialEstudo>().ToTable("MaterialEstudo");
            modelBuilder.Entity<Modulo>().ToTable("Modulo");
            modelBuilder.Entity<ModuloResolvido>().ToTable("ModuloResolvido");
            modelBuilder.Entity<Termo>().ToTable("Termo");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
            modelBuilder.Entity<Video>().ToTable("Video");

        }
        
    }
}
