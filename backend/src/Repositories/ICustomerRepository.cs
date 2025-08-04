using System.Collections.Generic;
using System.Threading.Tasks;
using MyApp.Models;

namespace MyApp.Repositories
{
    /// <summary>
    /// Interface para o repositório de clientes.
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Obtém todos os clientes.
        /// </summary>
        Task<IEnumerable<Cliente>> ObterTodosAsync();

        /// <summary>
        /// Obtém um cliente pelo identificador.
        /// </summary>
        Task<Cliente?> ObterPorIdAsync(int id);

        /// <summary>
        /// Adiciona um novo cliente.
        /// </summary>
        Task AdicionarAsync(Cliente entidade);

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        void Atualizar(Cliente entidade);

        /// <summary>
        /// Remove um cliente.
        /// </summary>
        void Remover(Cliente entidade);
    }
}
