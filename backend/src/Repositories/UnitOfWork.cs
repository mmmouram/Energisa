using System.Threading.Tasks;
using MyApp.Config;

namespace MyApp.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> SalvarAlteracoesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
