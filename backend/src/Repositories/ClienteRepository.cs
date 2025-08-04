using backend.src.Models;
using backend.src.Data;
using backend.src.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.src.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;
        
        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<Cliente> ObterPorIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }
        
        public async Task<List<Cliente>> ObterTodosAsync()
        {
            return await _context.Clientes.ToListAsync();
        }
        
        public async Task CriarAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }
        
        public async Task AtualizarAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
        }
        
        public async Task ExcluirAsync(Cliente cliente)
        {
            _context.Clientes.Remove(cliente);
        }
        
        public async Task<bool> ExisteEmailAsync(string email, int? clienteId = null)
        {
            if (clienteId.HasValue)
            {
                return await _context.Clientes.AnyAsync(c => c.Email == email && c.Id != clienteId.Value);
            }
            else
            {
                return await _context.Clientes.AnyAsync(c => c.Email == email);
            }
        }
    }
}
