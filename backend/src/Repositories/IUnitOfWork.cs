using System.Threading.Tasks;

namespace MyApp.Repositories
{
    /// <summary>
    /// Interface para o Unit Of Work.
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Salva as alterações realizadas no contexto.
        /// </summary>
        Task<int> SalvarAlteracoesAsync();
    }
}
