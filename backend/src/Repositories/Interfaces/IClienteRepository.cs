using backend.src.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.src.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> ObterPorIdAsync(int id);
        Task<List<Cliente>> ObterTodosAsync();
        Task CriarAsync(Cliente cliente);
        Task AtualizarAsync(Cliente cliente);
        Task ExcluirAsync(Cliente cliente);
        Task<bool> ExisteEmailAsync(string email, int? clienteId = null);
    }
}
