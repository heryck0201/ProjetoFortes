
using Escola.Application.DTOs;
using Escola.Application.Servicos;
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Moq;

namespace Escola.Teste.Application.Servicos
{
    public class CursoServiceTests
    {
        private readonly Mock<ICursoRepository> _cursoRepositoryMock;
        private readonly CursoService _cursoService;

        public CursoServiceTests()
        {
            _cursoRepositoryMock = new Mock<ICursoRepository>();
            _cursoService = new CursoService(_cursoRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllCursosAsync_DeveRetornarListaDeCursos()
        {
            var cursos = new List<Curso>
        {
            new Curso { Id = 1, Nome = "física", Descricao = "cáculo 1" },
            new Curso { Id = 2, Nome = "portugês", Descricao = "ler" }
        };

            _cursoRepositoryMock.Setup(repo => repo.GetAllCusosAsync()).ReturnsAsync(cursos);

            var resultado = await _cursoService.GetAllCursosAsync();

            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count());
            Assert.Equal("física", resultado.First().Nome);
        }

        [Fact]
        public async Task GetCursoByIdAsync_DeveRetornarCurso_QuandoExistir()
        {
            var curso = new Curso { Id = 1, Nome = "física", Descricao = "cáculo 1" };
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curso);

            var resultado = await _cursoService.GetCursoByIdAsync(1);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("física", resultado.Nome);
        }

        [Fact]
        public async Task GetCursoByIdAsync_DeveRetornarNull_QuandoNaoExistir()
        {
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Curso)null);

            var resultado = await _cursoService.GetCursoByIdAsync(99);

            Assert.Null(resultado);
        }

        [Fact]
        public async Task CreateCursoAsync_DeveCriarCursoComSucesso()
        {
            var dto = new CreateCursoDTO { Nome = "física", Descricao = "cáculo 1" };
            var cursoCriado = new Curso { Id = 1, Nome = "física", Descricao = "cáculo 1" };

            _cursoRepositoryMock.Setup(repo => repo.AddCursoAsync(It.IsAny<Curso>())).ReturnsAsync(cursoCriado);

            var resultado = await _cursoService.CreateCursoAsync(dto);

            Assert.NotNull(resultado);
            Assert.Equal(1, resultado.Id);
            Assert.Equal("física", resultado.Nome);
        }

        [Fact]
        public async Task UpdateCursoAsync_DeveRetornarTrue_QuandoCursoExistir()
        {
            var curso = new Curso { Id = 1, Nome = "física", Descricao = "antigo" };
            var dto = new UpdateCursoDTO { Nome = "física avançada", Descricao = "novo conteúdo" };

            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curso);

            var resultado = await _cursoService.UpdateCursoAsync(1, dto);

            Assert.True(resultado);
            _cursoRepositoryMock.Verify(repo => repo.UpdateCursoAsync(It.Is<Curso>(
                c => c.Nome == dto.Nome && c.Descricao == dto.Descricao
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateCursoAsync_DeveRetornarFalse_QuandoCursoNaoExistir()
        {
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Curso)null);
            var dto = new UpdateCursoDTO { Nome = "novo", Descricao = "desc" };

            var resultado = await _cursoService.UpdateCursoAsync(99, dto);

            Assert.False(resultado);
            _cursoRepositoryMock.Verify(repo => repo.UpdateCursoAsync(It.IsAny<Curso>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCursoAsync_DeveRetornarTrue_QuandoCursoExistir()
        {
            var curso = new Curso { Id = 1, Nome = "física", Descricao = "cáculo 1" };
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(curso);

            var resultado = await _cursoService.DeleteCursoAsync(1);

            Assert.True(resultado);
            _cursoRepositoryMock.Verify(repo => repo.DeleteCursoAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteCursoAsync_DeveRetornarFalse_QuandoCursoNaoExistir()
        {
            _cursoRepositoryMock.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Curso)null);

            var resultado = await _cursoService.DeleteCursoAsync(99);

            Assert.False(resultado);
            _cursoRepositoryMock.Verify(repo => repo.DeleteCursoAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
