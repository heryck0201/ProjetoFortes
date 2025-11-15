
using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;

namespace Escola.Application.Servicos
{
    public class CursoService : ICursoService
    {
        private readonly ICursoRepository _cursoRepository;

        public CursoService(ICursoRepository cursoRepository)
        {
            _cursoRepository = cursoRepository;
        }
        public async Task<IEnumerable<CursoDTO>> GetAllCursosAsync()
        {
            var cursos = await _cursoRepository.GetAllCusosAsync();
            return cursos.Select(c => new CursoDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Descricao = c.Descricao
            });
        }

        public async Task<CursoDTO> GetCursoByIdAsync(int id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null) return null;

            return new CursoDTO
            {
                Id = curso.Id,
                Nome = curso.Nome,
                Descricao = curso.Descricao
            };
        }

        public async Task<CursoDTO> CreateCursoAsync(CreateCursoDTO createCursoDto)
        {
            var curso = new Curso
            {
                Nome = createCursoDto.Nome,
                Descricao = createCursoDto.Descricao
            };

            var created = await _cursoRepository.AddCursoAsync(curso);

            return new CursoDTO
            {
                Id = created.Id,
                Nome = created.Nome,
                Descricao = created.Descricao
            };
        }

        public async Task<bool> UpdateCursoAsync(int id, UpdateCursoDTO updateCursoDto)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null) return false;

            curso.Nome = updateCursoDto.Nome;
            curso.Descricao = updateCursoDto.Descricao;

            await _cursoRepository.UpdateCursoAsync(curso);
            return true;
        }

        public async Task<bool> DeleteCursoAsync(int id)
        {
            var curso = await _cursoRepository.GetByIdAsync(id);
            if (curso == null) return false;

            await _cursoRepository.DeleteCursoAsync(id);
            return true;
        }
    }
}
