using Escola.Domain.Entidades;

namespace Escola.Domain.IRepositorio
{
    public interface IMatriculaRepository
    {
        Task<Matricula> GetByIdAsync(int id);
        Task<IEnumerable<Matricula>> GetAllMatriculasAsync();
        Task<Matricula> AddMatriculaAsync(Matricula matricula);
        Task DeleteMatriculaAsync(int id);
        Task<bool> ExistsAsync(int alunoId, int cursoId);
        Task<IEnumerable<Matricula>> GetByCursoIdAsync(int cursoId);
        Task<IEnumerable<Matricula>> GetByAlunoIdAsync(int alunoId);
        Task<Matricula> GetByAlunoAndCursoAsync(int alunoId, int cursoId);
    }
}
