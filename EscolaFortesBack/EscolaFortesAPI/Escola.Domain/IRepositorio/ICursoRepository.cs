using Escola.Domain.Entidades;

namespace Escola.Domain.IRepositorio
{
    public interface ICursoRepository
    {
        Task<Curso> GetByIdAsync(int id);
        Task<IEnumerable<Curso>> GetAllCusosAsync();
        Task<Curso> AddCursoAsync(Curso curso);
        Task UpdateCursoAsync(Curso curso);
        Task DeleteCursoAsync(int id);
    }
}
