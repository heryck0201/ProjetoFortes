
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Escola.Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infraestrutura.Repositorio
{
    public class MatriculaRepository : IMatriculaRepository
    {
        private readonly ApplicationDbContext _context;

        public MatriculaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Matricula> GetByIdAsync(int id)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Matricula>> GetAllMatriculasAsync()
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .ToListAsync();
        }

        public async Task<Matricula> AddMatriculaAsync(Matricula matricula)
        {
            _context.Matriculas.Add(matricula);
            await _context.SaveChangesAsync();
            return matricula;
        }

        public async Task DeleteMatriculaAsync(int id)
        {
            var matricula = await _context.Matriculas.FindAsync(id);
            if (matricula != null)
            {
                _context.Matriculas.Remove(matricula);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int alunoId, int cursoId)
        {
            return await _context.Matriculas
                .AnyAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
        }

        public async Task<IEnumerable<Matricula>> GetByCursoIdAsync(int cursoId)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .Where(m => m.CursoId == cursoId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Matricula>> GetByAlunoIdAsync(int alunoId)
        {
            return await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .Where(m => m.AlunoId == alunoId)
                .ToListAsync();
        }

        public async Task<Matricula> GetByAlunoAndCursoAsync(int alunoId, int cursoId)
        {
            return await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
        }
    }
}
