using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;

namespace Escola.Application.Servicos
{
    public class AlunoService : IAlunoService
    {
        private readonly IAlunoRepository _alunoRepository;

        public AlunoService(IAlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<IEnumerable<AlunoDTO>> GetAllAlunosAsync()
        {
            var alunos = await _alunoRepository.GetAllAlunosAsync();
            return alunos.Select(a => new AlunoDTO
            {
                Id = a.Id,
                Nome = a.Nome,
                Email = a.Email,
                DataNascimento = a.DataNascimento
            });
        }

        public async Task<AlunoDTO> GetAlunoByIdAsync(int id)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);
            if (aluno == null) return null;

            return new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                Email = aluno.Email,
                DataNascimento = aluno.DataNascimento
            };
        }

        public async Task<AlunoDTO> CreateAlunoAsync(CreateAlunoDTO createAlunoDto)
        {
            if (!IsMaiorIdade(createAlunoDto.DataNascimento))
            {
                throw new InvalidOperationException("Não é possível cadastrar aluno menor de idade.");
            }

            if (await _alunoRepository.EmailExistsAsync(createAlunoDto.Email))
            {
                throw new InvalidOperationException("Já existe um aluno cadastrado com este e-mail.");
            }

            var aluno = new Aluno
            {
                Nome = createAlunoDto.Nome,
                Email = createAlunoDto.Email,
                DataNascimento = createAlunoDto.DataNascimento.Date
            };

            var created = await _alunoRepository.AddAlunoAsync(aluno);

            return new AlunoDTO
            {
                Id = created.Id,
                Nome = created.Nome,
                Email = created.Email,
                DataNascimento = created.DataNascimento
            };
        }

        public async Task<bool> UpdateAlunoAsync(int id, UpdateAlunoDTO updateAlunoDto)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);
            if (aluno == null) return false;

            if (!IsMaiorIdade(updateAlunoDto.DataNascimento))
            {
                throw new InvalidOperationException("Não é possível atualizar para data de nascimento de menor de idade.");
            }

            if (await _alunoRepository.EmailExistsAsync(updateAlunoDto.Email, id))
            {
                throw new InvalidOperationException("Já existe outro aluno cadastrado com este e-mail.");
            }

            aluno.Nome = updateAlunoDto.Nome;
            aluno.Email = updateAlunoDto.Email;
            aluno.DataNascimento = updateAlunoDto.DataNascimento.Date;

            await _alunoRepository.UpdateAlunoAsync(aluno);
            return true;
        }

        public async Task<bool> DeleteAlunoAsync(int id)
        {
            var aluno = await _alunoRepository.GetByIdAsync(id);
            if (aluno == null) return false;

            await _alunoRepository.DeleteAlunoAsync(id);
            return true;
        }

        private bool IsMaiorIdade(DateTime dataNascimento)
        {
            var idade = DateTime.Today.Year - dataNascimento.Year;
            if (DateTime.Today < dataNascimento.AddYears(idade)) idade--;
            return idade >= 18;
        }
    }
}
