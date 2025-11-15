using Escola.Application.DTOs;
using Escola.Application.Servicos;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Moq;

namespace Escola.Teste.Application.Servicos
{
    public class MatriculaServiceTests
    {
        private readonly Mock<IMatriculaRepository> _matriculaRepositoryMock;
        private readonly Mock<IAlunoRepository> _alunoRepositoryMock;
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
        private readonly MatriculaService _matriculaService;

        public MatriculaServiceTests()
        {
            _matriculaRepositoryMock = new Mock<IMatriculaRepository>();
            _alunoRepositoryMock = new Mock<IAlunoRepository>();
            _cursoRepositoryMock = new Mock<ICursoRepository>();

            _matriculaService = new MatriculaService(
                _matriculaRepositoryMock.Object,
                _alunoRepositoryMock.Object,
                _cursoRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetAllMatriculasAsync_DeveRetornarListaDeMatriculas()
        {
            var matriculas = new List<Matricula>
        {
            new Matricula
            {
                Id = 1,
                AlunoId = 2,
                CursoId = 1,
                DataMatricula = DateTime.Now,
                Aluno = new Aluno { Id = 2, Nome = "Lucas" },
                Curso = new Curso { Id = 1, Nome = "Física" }
            },
            new Matricula
            {
                Id = 2,
                AlunoId = 2,
                CursoId = 2,
                DataMatricula = DateTime.Now,
                Aluno = new Aluno { Id = 2, Nome = "Lucas" },
                Curso = new Curso { Id = 2, Nome = "Português" }
            }
        };

            _matriculaRepositoryMock.Setup(repo => repo.GetAllMatriculasAsync())
                .ReturnsAsync(matriculas);

            var resultado = await _matriculaService.GetAllMatriculasAsync();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("Lucas", resultado.First().AlunoNome);
        }

        [Fact]
        public async Task CreateMatriculaAsync_DeveLancarExcecao_SeAlunoNaoExistir()
        {
            var dto = new CreateMatriculaDTO { AlunoId = 99, CursoId = 1 };

            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.AlunoId))
                .ReturnsAsync((Aluno)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _matriculaService.CreateMatriculaAsync(dto));

            Assert.Equal("Aluno não encontrado.", ex.Message);
        }

        [Fact]
        public async Task CreateMatriculaAsync_DeveLancarExcecao_SeCursoNaoExistir()
        {
            var dto = new CreateMatriculaDTO { AlunoId = 2, CursoId = 99 };

            _alunoRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.AlunoId))
                .ReturnsAsync(new Aluno { Id = 2, Nome = "Lucas" });

            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(dto.CursoId))
                .ReturnsAsync((Curso)null);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _matriculaService.CreateMatriculaAsync(dto));

            Assert.Equal("Curso não encontrado.", ex.Message);
        }

        [Fact]
        public async Task CreateMatriculaAsync_DeveLancarExcecao_SeJaEstiverMatriculado()
        {
            var dto = new CreateMatriculaDTO { AlunoId = 2, CursoId = 1 };

            _alunoRepositoryMock.Setup(r => r.GetByIdAsync(dto.AlunoId))
                .ReturnsAsync(new Aluno { Id = 2, Nome = "Lucas" });

            _cursoRepositoryMock.Setup(r => r.GetByIdAsync(dto.CursoId))
                .ReturnsAsync(new Curso { Id = 1, Nome = "Física" });

            _matriculaRepositoryMock.Setup(r => r.ExistsAsync(dto.AlunoId, dto.CursoId))
                .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _matriculaService.CreateMatriculaAsync(dto));

            Assert.Equal("Aluno já está matriculado neste curso.", ex.Message);
        }

        [Fact]
        public async Task CreateMatriculaAsync_DeveCriarMatriculaComSucesso()
        {
            var dto = new CreateMatriculaDTO { AlunoId = 2, CursoId = 1 };

            var aluno = new Aluno { Id = 2, Nome = "Lucas" };
            var curso = new Curso { Id = 1, Nome = "Física" };

            _alunoRepositoryMock.Setup(r => r.GetByIdAsync(dto.AlunoId)).ReturnsAsync(aluno);
            _cursoRepositoryMock.Setup(r => r.GetByIdAsync(dto.CursoId)).ReturnsAsync(curso);
            _matriculaRepositoryMock.Setup(r => r.ExistsAsync(dto.AlunoId, dto.CursoId)).ReturnsAsync(false);

            _matriculaRepositoryMock.Setup(r => r.AddMatriculaAsync(It.IsAny<Matricula>()))
                .ReturnsAsync((Matricula m) =>
                {
                    m.Id = 10;
                    return m;
                });

            var resultado = await _matriculaService.CreateMatriculaAsync(dto);

            Assert.NotNull(resultado);
            Assert.Equal(10, resultado.Id);
            Assert.Equal("Lucas", resultado.AlunoNome);
            Assert.Equal("Física", resultado.CursoNome);
        }

        [Fact]
        public async Task DeleteMatriculaAsync_DeveRetornarTrue_QuandoMatriculaExistir()
        {
            var matricula = new Matricula { Id = 1, AlunoId = 2, CursoId = 1 };
            _matriculaRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(matricula);

            var resultado = await _matriculaService.DeleteMatriculaAsync(1);

            Assert.True(resultado);
            _matriculaRepositoryMock.Verify(r => r.DeleteMatriculaAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteMatriculaAsync_DeveRetornarFalse_QuandoNaoExistir()
        {
            // Arrange
            _matriculaRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Matricula)null);

            // Act
            var resultado = await _matriculaService.DeleteMatriculaAsync(1);

            Assert.False(resultado);
            _matriculaRepositoryMock.Verify(r => r.DeleteMatriculaAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task RemoveAlunoFromCursoAsync_DeveRemoverMatricula()
        {
            var matricula = new Matricula { Id = 1, AlunoId = 2, CursoId = 1 };
            _matriculaRepositoryMock.Setup(r => r.GetByAlunoAndCursoAsync(2, 1))
                .ReturnsAsync(matricula);

            var resultado = await _matriculaService.RemoveAlunoFromCursoAsync(2, 1);

            Assert.True(resultado);
            _matriculaRepositoryMock.Verify(r => r.DeleteMatriculaAsync(1), Times.Once);
        }

        [Fact]
        public async Task RemoveAlunoFromCursoAsync_DeveRetornarFalse_SeNaoExistirMatricula()
        {
            _matriculaRepositoryMock.Setup(r => r.GetByAlunoAndCursoAsync(2, 1))
                .ReturnsAsync((Matricula)null);

            var resultado = await _matriculaService.RemoveAlunoFromCursoAsync(2, 1);

            Assert.False(resultado);
        }

        [Fact]
        public async Task GetMatriculasByCursoAsync_DeveRetornarMatriculasDoCurso()
        {
            var matriculas = new List<Matricula>
        {
            new Matricula
            {
                Id = 1, AlunoId = 2, CursoId = 1, DataMatricula = DateTime.Now,
                Aluno = new Aluno { Nome = "Lucas" }, Curso = new Curso { Nome = "Física" }
            }
        };

            _matriculaRepositoryMock.Setup(r => r.GetByCursoIdAsync(1))
                .ReturnsAsync(matriculas);

            var resultado = await _matriculaService.GetMatriculasByCursoAsync(1);

            Assert.Single(resultado);
            Assert.Equal("Física", resultado.First().CursoNome);
        }

        [Fact]
        public async Task GetMatriculasByAlunoAsync_DeveRetornarMatriculasDoAluno()
        {
            var matriculas = new List<Matricula>
        {
            new Matricula
            {
                Id = 2, AlunoId = 2, CursoId = 2, DataMatricula = DateTime.Now,
                Aluno = new Aluno { Nome = "Lucas" }, Curso = new Curso { Nome = "Português" }
            }
        };

            _matriculaRepositoryMock.Setup(r => r.GetByAlunoIdAsync(2))
                .ReturnsAsync(matriculas);

            var resultado = await _matriculaService.GetMatriculasByAlunoAsync(2);

            Assert.Single(resultado);
            Assert.Equal("Português", resultado.First().CursoNome);
        }
    }
}
