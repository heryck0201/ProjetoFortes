using Escola.Application.DTOs;

namespace Escola.Application.Servicos.Interface
{
    public interface ICursoService
    {
        Task<IEnumerable<CursoDTO>> GetAllCursosAsync();
        Task<CursoDTO> GetCursoByIdAsync(int id);
        Task<CursoDTO> CreateCursoAsync(CreateCursoDTO createCursoDto);
        Task<bool> UpdateCursoAsync(int id, UpdateCursoDTO updateCursoDto);
        Task<bool> DeleteCursoAsync(int id);
    }
}
