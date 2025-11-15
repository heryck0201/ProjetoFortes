using Escola.Domain.Entidades;

namespace Escola.Domain.IRepositorio
{
    public interface IAlunoRepository
    {
        Task<Aluno> GetByIdAsync(int id);
        Task<IEnumerable<Aluno>> GetAllAlunosAsync();
        Task<Aluno> AddAlunoAsync(Aluno aluno);
        Task UpdateAlunoAsync(Aluno aluno);
        Task DeleteAlunoAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}
