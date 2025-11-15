using Escola.Application.DTOs;
using Escola.Application.Servicos.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Escola.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService alunoService)
        {
            _alunoService = alunoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunos()
        {
            var alunos = await _alunoService.GetAllAlunosAsync();
            return Ok(alunos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoDTO>> GetAluno(int id)
        {
            var aluno = await _alunoService.GetAlunoByIdAsync(id);
            if (aluno == null) return NotFound();
            return Ok(aluno);
        }

        [HttpPost]
        public async Task<ActionResult<AlunoDTO>> CreateAluno(CreateAlunoDTO createAlunoDto)
        {
            try
            {
                var aluno = await _alunoService.CreateAlunoAsync(createAlunoDto);
                return CreatedAtAction(nameof(GetAluno), new { id = aluno.Id }, aluno);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAluno(int id, UpdateAlunoDTO updateAlunoDto)
        {
            try
            {
                var result = await _alunoService.UpdateAlunoAsync(id, updateAlunoDto);
                if (!result) return NotFound();
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(int id)
        {
            var result = await _alunoService.DeleteAlunoAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
