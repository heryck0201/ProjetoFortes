using Escola.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infraestrutura.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Descricao).HasMaxLength(500);
            });

            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Nome).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(a => a.Email).IsUnique();
                entity.Property(a => a.DataNascimento).IsRequired();
            });

            modelBuilder.Entity<Matricula>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.DataMatricula).IsRequired();

                entity.HasOne(m => m.Aluno)
                    .WithMany(a => a.Matriculas)
                    .HasForeignKey(m => m.AlunoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Curso)
                    .WithMany(c => c.Matriculas)
                    .HasForeignKey(m => m.CursoId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(m => new { m.AlunoId, m.CursoId })
                    .IsUnique();
            });
        }
    }
}
