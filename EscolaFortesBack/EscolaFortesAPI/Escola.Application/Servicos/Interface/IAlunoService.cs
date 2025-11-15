using Escola.Application.DTOs;

namespace Escola.Application.Servicos.Interface
{
    public interface IAlunoService
    {
        Task<IEnumerable<AlunoDTO>> GetAllAlunosAsync();
        Task<AlunoDTO> GetAlunoByIdAsync(int id);
        Task<AlunoDTO> CreateAlunoAsync(CreateAlunoDTO createAlunoDto);
        Task<bool> UpdateAlunoAsync(int id, UpdateAlunoDTO updateAlunoDto);
        Task<bool> DeleteAlunoAsync(int id);
    }
}
