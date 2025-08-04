using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Config;

namespace MyApp.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Cliente?> ObterPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task AdicionarAsync(Cliente entidade)
        {
            await _context.Clientes.AddAsync(entidade);
        }

        public void Atualizar(Cliente entidade)
        {
            _context.Clientes.Update(entidade);
        }

        public void Remover(Cliente entidade)
        {
            _context.Clientes.Remove(entidade);
        }
    }
}
