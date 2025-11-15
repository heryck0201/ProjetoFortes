using Escola.Application.DTOs;

namespace Escola.Application.Servicos.Interface
{
    public interface IMatriculaService
    {
        Task<IEnumerable<MatriculaDTO>> GetAllMatriculasAsync();
        Task<MatriculaDTO> CreateMatriculaAsync(CreateMatriculaDTO createMatriculaDto);
        Task<bool> DeleteMatriculaAsync(int id);
        Task<bool> RemoveAlunoFromCursoAsync(int alunoId, int cursoId);
        Task<IEnumerable<MatriculaDTO>> GetMatriculasByCursoAsync(int cursoId);
        Task<IEnumerable<MatriculaDTO>> GetMatriculasByAlunoAsync(int alunoId);
    }
}
