
using Escola.Domain.Entidades;
using Escola.Domain.IRepositorio;
using Escola.Infraestrutura.Context;
using Microsoft.EntityFrameworkCore;

namespace Escola.Infraestrutura.Repositorio
{
    public class AlunoRepository : IAlunoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlunoRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Aluno> GetByIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<IEnumerable<Aluno>> GetAllAlunosAsync()
        {
            return await _context.Alunos.ToListAsync();
        }

        public async Task<Aluno> AddAlunoAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }

        public async Task UpdateAlunoAsync(Aluno aluno)
        {
            _context.Alunos.Update(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAlunoAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            return await _context.Alunos
                .AnyAsync(a => a.Email == email && (!excludeId.HasValue || a.Id != excludeId.Value));
        }
    }
}
