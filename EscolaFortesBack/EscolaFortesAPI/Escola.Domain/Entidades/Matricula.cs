
namespace Escola.Domain.Entidades
{
    public class Matricula
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int CursoId { get; set; }
        public DateTime DataMatricula { get; set; } = DateTime.UtcNow;
        public virtual Aluno Aluno { get; set; }
        public virtual Curso Curso { get; set; }
    }
}
