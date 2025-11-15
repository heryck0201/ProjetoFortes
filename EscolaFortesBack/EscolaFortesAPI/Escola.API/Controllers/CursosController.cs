using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly ICursoService _cursoService;

        public CursosController(ICursoService cursoService)
        {
            _cursoService = cursoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoDTO>>> GetCursos()
        {
            var cursos = await _cursoService.GetAllCursosAsync();
            return Ok(cursos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDTO>> GetCurso(int id)
        {
            var curso = await _cursoService.GetCursoByIdAsync(id);
            if (curso == null) return NotFound();
            return Ok(curso);
        }

        [HttpPost]
        public async Task<ActionResult<CursoDTO>> CreateCurso(CreateCursoDTO createCursoDto)
        {
            var curso = await _cursoService.CreateCursoAsync(createCursoDto);
            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCurso(int id, UpdateCursoDTO updateCursoDto)
        {
            var result = await _cursoService.UpdateCursoAsync(id, updateCursoDto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var result = await _cursoService.DeleteCursoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
