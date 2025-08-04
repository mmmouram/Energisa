using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Config;
using MyApp.Models;
using MyApp.Repositories;
using NUnit.Framework;

namespace MyApp.Tests.Repositories
{
    [TestFixture]
    public class UnitOfWorkTests
    {
        private AppDbContext _context;
        private UnitOfWork _unitOfWork;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "UnitOfWorkTestDb")
                .Options;
            _context = new AppDbContext(options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Test]
        public async Task SalvarAlteracoesAsync_Retorna_Numero_De_Registros()
        {
            // Arrange
            _context.Clientes.Add(new Cliente { Nome = "Test", Email = "test@test.com", Telefone = "12345" });
            
            // Act
            var result = await _unitOfWork.SalvarAlteracoesAsync();
            
            // Assert
            Assert.AreEqual(1, result);
        }
    }
}
