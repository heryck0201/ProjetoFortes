
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Escola.Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infraestrutura.Repositorio
{
    public class CursoRepository : ICursoRepository
    {
        private readonly ApplicationDbContext _context;

        public CursoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Curso> GetByIdAsync(int id)
        {
            return await _context.Cursos.FindAsync(id);
        }

        public async Task<IEnumerable<Curso>> GetAllCusosAsync()
        {
            return await _context.Cursos.ToListAsync();
        }

        public async Task<Curso> AddCursoAsync(Curso curso)
        {
            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();
            return curso;
        }

        public async Task UpdateCursoAsync(Curso curso)
        {
            _context.Cursos.Update(curso);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCursoAsync(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
                await _context.SaveChangesAsync();
            }
        }
    }
}
