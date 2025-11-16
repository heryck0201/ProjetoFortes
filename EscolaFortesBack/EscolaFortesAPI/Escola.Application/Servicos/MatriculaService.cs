
using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;

namespace Escola.Application.Servicos
{
    public class MatriculaService : IMatriculaService
    {
        private readonly IMatriculaRepository _matriculaRepository;
        private readonly IAlunoRepository _alunoRepository;
        private readonly ICursoRepository _cursoRepository;

        public MatriculaService(
            IMatriculaRepository matriculaRepository,
            IAlunoRepository alunoRepository,
            ICursoRepository cursoRepository)
        {
            _matriculaRepository = matriculaRepository;
            _alunoRepository = alunoRepository;
            _cursoRepository = cursoRepository;
        }

        public async Task<IEnumerable<MatriculaDTO>> GetAllMatriculasAsync()
        {
            var matriculas = await _matriculaRepository.GetAllMatriculasAsync();
            return matriculas.Select(m => new MatriculaDTO
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula,
                AlunoNome = m.Aluno.Nome,
                CursoNome = m.Curso.Nome
            });
        }

        public async Task<MatriculaDTO> CreateMatriculaAsync(CreateMatriculaDTO createMatriculaDto)
        {
            var aluno = await _alunoRepository.GetByIdAsync(createMatriculaDto.AlunoId);
            if (aluno == null)
            {
                throw new InvalidOperationException("Aluno não encontrado.");
            }

            var curso = await _cursoRepository.GetByIdAsync(createMatriculaDto.CursoId);
            if (curso == null)
            {
                throw new InvalidOperationException("Curso não encontrado.");
            }

            if (await _matriculaRepository.ExistsAsync(createMatriculaDto.AlunoId, createMatriculaDto.CursoId))
            {
                throw new InvalidOperationException("Aluno já está matriculado neste curso.");
            }

            var matricula = new Matricula
            {
                AlunoId = createMatriculaDto.AlunoId,
                CursoId = createMatriculaDto.CursoId,
                DataMatricula = DateTime.UtcNow
            };

            var created = await _matriculaRepository.AddMatriculaAsync(matricula);

            return new MatriculaDTO
            {
                Id = created.Id,
                AlunoId = created.AlunoId,
                CursoId = created.CursoId,
                DataMatricula = created.DataMatricula,
                AlunoNome = aluno.Nome,
                CursoNome = curso.Nome
            };
        }

        public async Task<bool> DeleteMatriculaAsync(int id)
        {
            var matricula = await _matriculaRepository.GetByIdAsync(id);
            if (matricula == null) return false;

            await _matriculaRepository.DeleteMatriculaAsync(id);
            return true;
        }

        public async Task<bool> RemoveAlunoFromCursoAsync(int alunoId, int cursoId)
        {
            var matricula = await _matriculaRepository.GetByAlunoAndCursoAsync(alunoId, cursoId);
            if (matricula == null) return false;

            await _matriculaRepository.DeleteMatriculaAsync(matricula.Id);
            return true;
        }

        public async Task<IEnumerable<MatriculaDTO>> GetMatriculasByCursoAsync(int cursoId)
        {
            var matriculas = await _matriculaRepository.GetByCursoIdAsync(cursoId);
            return matriculas.Select(m => new MatriculaDTO
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula,
                AlunoNome = m.Aluno.Nome,
                CursoNome = m.Curso.Nome
            });
        }

        public async Task<IEnumerable<MatriculaDTO>> GetMatriculasByAlunoAsync(int alunoId)
        {
            var matriculas = await _matriculaRepository.GetByAlunoIdAsync(alunoId);
            return matriculas.Select(m => new MatriculaDTO
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                CursoId = m.CursoId,
                DataMatricula = m.DataMatricula,
                AlunoNome = m.Aluno.Nome,
                CursoNome = m.Curso.Nome
            });
        }
    }
}
