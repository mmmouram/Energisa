using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyApp.Models;
using MyApp.Config;
using MyApp.Repositories;
using NUnit.Framework;

namespace MyApp.Tests.Repositories
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        private AppDbContext _context;
        private CustomerRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CustomerRepoTestDb")
                .Options;
            _context = new AppDbContext(options);
            _repository = new CustomerRepository(_context);
            
            // Limpa os dados existentes
            _context.Clientes.RemoveRange(_context.Clientes);
            _context.SaveChanges();
        }

        [Test]
        public async Task ObterTodosAsync_Retorna_Clientes()
        {
            // Arrange
            _context.Clientes.Add(new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" });
            _context.Clientes.Add(new Cliente { Id = 2, Nome = "Test2", Email = "test2@test.com", Telefone = "67890" });
            await _context.SaveChangesAsync();

            // Act
            var clientes = await _repository.ObterTodosAsync();

            // Assert
            Assert.IsNotNull(clientes);
            Assert.AreEqual(2, ((List<Cliente>)clientes).Count);
        }

        [Test]
        public async Task ObterPorIdAsync_Retorna_Cliente_Correto()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ObterPorIdAsync(cliente.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(cliente.Id, result.Id);
        }

        [Test]
        public async Task AdicionarAsync_Adiciona_Cliente()
        {
            // Arrange
            var cliente = new Cliente { Nome = "Novo", Email = "novo@test.com", Telefone = "55555" };

            // Act
            await _repository.AdicionarAsync(cliente);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.ObterTodosAsync();
            Assert.AreEqual(1, ((List<Cliente>)result).Count);
        }

        [Test]
        public async Task Atualizar_Atualiza_Cliente()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            cliente.Nome = "Atualizado";
            _repository.Atualizar(cliente);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.ObterPorIdAsync(cliente.Id);
            Assert.AreEqual("Atualizado", result.Nome);
        }

        [Test]
        public async Task Remover_Remove_Cliente()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            _repository.Remover(cliente);
            await _context.SaveChangesAsync();

            // Assert
            var result = await _repository.ObterPorIdAsync(cliente.Id);
            Assert.IsNull(result);
        }
    }
}
