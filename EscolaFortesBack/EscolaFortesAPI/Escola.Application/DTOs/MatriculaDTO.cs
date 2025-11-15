
namespace Escola.Application.DTOs
{
    public class MatriculaDTO
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public int CursoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public string AlunoNome { get; set; }
        public string CursoNome { get; set; }
    }
}
