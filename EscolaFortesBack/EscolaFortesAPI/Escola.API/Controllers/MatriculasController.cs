using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatriculasController : ControllerBase
    {
        private readonly IMatriculaService _matriculaService;

        public MatriculasController(IMatriculaService matriculaService)
        {
            _matriculaService = matriculaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculas()
        {
            var matriculas = await _matriculaService.GetAllMatriculasAsync();
            return Ok(matriculas);
        }

        [HttpGet("curso/{cursoId}")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculasPorCurso(int cursoId)
        {
            var matriculas = await _matriculaService.GetMatriculasByCursoAsync(cursoId);
            return Ok(matriculas);
        }

        [HttpGet("aluno/{alunoId}")]
        public async Task<ActionResult<IEnumerable<MatriculaDTO>>> GetMatriculasPorAluno(int alunoId)
        {
            var matriculas = await _matriculaService.GetMatriculasByAlunoAsync(alunoId);
            return Ok(matriculas);
        }

        [HttpPost]
        public async Task<ActionResult<MatriculaDTO>> CreateMatricula(CreateMatriculaDTO createMatriculaDto)
        {
            try
            {
                var matricula = await _matriculaService.CreateMatriculaAsync(createMatriculaDto);
                return CreatedAtAction(nameof(GetMatriculas), new { id = matricula.Id }, matricula);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatricula(int id)
        {
            var result = await _matriculaService.DeleteMatriculaAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("aluno/{alunoId}/curso/{cursoId}")]
        public async Task<IActionResult> RemoveAlunoFromCurso(int alunoId, int cursoId)
        {
            var result = await _matriculaService.RemoveAlunoFromCursoAsync(alunoId, cursoId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
