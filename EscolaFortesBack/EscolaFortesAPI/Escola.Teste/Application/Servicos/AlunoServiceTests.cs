using Escola.Application.DTOs;
using Escola.Application.Servicos;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Moq;

namespace Escola.Teste.Application.Servicos
{
    public class AlunoServiceTests
    {
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly AlunoService _alunoService;

        public AlunoServiceTests()
        {
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _alunoService = new AlunoService(_alunoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAlunosAsync_DeveRetornarListaDeAlunos()
        {
            var alunos = new List<Aluno>
        {
            new Aluno { Id = 2, Nome = "Lucas Andrade", Email = "lucas.andrade@hotmail.com", DataNascimento = new DateTime(1999, 9, 10) },
            new Aluno { Id = 3, Nome = "Mariana Costa", Email = "mariana.costa@gmail.com", DataNascimento = new DateTime(2005, 12, 2) }
        };

            _alunoRepositoryMock.Setup(repo => repo.GetAllAlunosAsync()).ReturnsAsync(alunos);

            var result = await _alunoService.GetAllAlunosAsync();

            Assert.Equal(2, result.Count());
            Assert.Contains(result, a => a.Nome == "Lucas Andrade");
            Assert.Contains(result, a => a.Nome == "Mariana Costa");
        }

        [Fact]
        public async Task GetAlunoByIdAsync_DeveRetornarAluno_QuandoExistir()
        {
            var aluno = new Aluno { Id = 2, Nome = "Lucas Andrade", Email = "lucas.andrade@hotmail.com", DataNascimento = new DateTime(1999, 9, 10) };
            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(aluno);

            var result = await _alunoService.GetAlunoByIdAsync(2);

            Assert.NotNull(result);
            Assert.Equal("Lucas Andrade", result.Nome);
        }

        [Fact]
        public async Task GetAlunoByIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Aluno)null);

            var result = await _alunoService.GetAlunoByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAlunoAsync_DeveLancarExcecao_QuandoMenorDeIdade()
        {
            var dto = new CreateAlunoDTO
            {
                Nome = "Mariana Costa",
                Email = "mariana.costa@gmail.com",
                DataNascimento = new DateTime(2008, 12, 2)
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _alunoService.CreateAlunoAsync(dto));
            Assert.Equal("Não é possível cadastrar aluno menor de idade.", ex.Message);
        }

        [Fact]
        public async Task CreateAlunoAsync_DeveLancarExcecao_QuandoEmailJaExistir()
        {
            var dto = new CreateAlunoDTO
            {
                Nome = "Lucas Andrade",
                Email = "lucas.andrade@hotmail.com",
                DataNascimento = new DateTime(1999, 9, 10)
            };

            _alunoRepositoryMock
                .Setup(repo => repo.EmailExistsAsync(It.IsAny<string>(), It.IsAny<int?>()))
                .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _alunoService.CreateAlunoAsync(dto));

            Assert.Equal("Já existe um aluno cadastrado com este e-mail.", ex.Message);
        }

        [Fact]
        public async Task UpdateAlunoAsync_DeveRetornarTrue_QuandoAtualizacaoForBemSucedida()
        {
            var alunoExistente = new Aluno { Id = 2, Nome = "Lucas", Email = "lucas@teste.com", DataNascimento = new DateTime(1999, 9, 10) };
            var updateDto = new UpdateAlunoDTO { Nome = "Lucas Andrade", Email = "lucas.andrade@hotmail.com", DataNascimento = new DateTime(1999, 9, 10) };

            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(alunoExistente);
            _alunoRepositoryMock.Setup(repo => repo.EmailExistsAsync(updateDto.Email, 2)).ReturnsAsync(false);

            var result = await _alunoService.UpdateAlunoAsync(2, updateDto);

            Assert.True(result);
            _alunoRepositoryMock.Verify(repo => repo.UpdateAlunoAsync(It.IsAny<Aluno>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAlunoAsync_DeveRetornarFalse_QuandoAlunoNaoExistir()
        {
            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Aluno)null);

            var dto = new UpdateAlunoDTO { Nome = "Teste", Email = "teste@teste.com", DataNascimento = new DateTime(1990, 1, 1) };

            var result = await _alunoService.UpdateAlunoAsync(99, dto);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAlunoAsync_DeveRetornarTrue_QuandoAlunoExistir()
        {
            var aluno = new Aluno { Id = 2, Nome = "Lucas" };
            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(2)).ReturnsAsync(aluno);

            var result = await _alunoService.DeleteAlunoAsync(2);

            Assert.True(result);
            _alunoRepositoryMock.Verify(repo => repo.DeleteAlunoAsync(2), Times.Once);
        }

        [Fact]
        public async Task DeleteAlunoAsync_DeveRetornarFalse_QuandoAlunoNaoExistir()
        {
            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Aluno)null);

            var result = await _alunoService.DeleteAlunoAsync(99);

            Assert.False(result);
        }
    }
}
