using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MyApp.Models;
using MyApp.Repositories;
using MyApp.Services;
using NUnit.Framework;

namespace MyApp.Tests.Services
{
    [TestFixture]
    public class ClienteServiceTests
    {
        private Mock<ICustomerRepository> _mockRepo;
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private ClienteService _clienteService;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _clienteService = new ClienteService(_mockRepo.Object, _mockUnitOfWork.Object);
        }

        [Test]
        public async Task ObterTodosClientesAsync_Chama_Repositorio_ObterTodosAsync()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" }
            };
            _mockRepo.Setup(r => r.ObterTodosAsync()).ReturnsAsync(clientes);

            // Act
            var result = await _clienteService.ObterTodosClientesAsync();

            // Assert
            Assert.IsNotNull(result);
            _mockRepo.Verify(r => r.ObterTodosAsync(), Times.Once);
        }

        [Test]
        public async Task CriarClienteAsync_ClienteValido_Chama_AdicionarAsync_E_SalvarAlteracoesAsync()
        {
            // Arrange
            var cliente = new Cliente { Nome = "Valid", Email = "valid@test.com", Telefone = "12345" };

            // Act
            await _clienteService.CriarClienteAsync(cliente);

            // Assert
            _mockRepo.Verify(r => r.AdicionarAsync(cliente), Times.Once);
            _mockUnitOfWork.Verify(u => u.SalvarAlteracoesAsync(), Times.Once);
        }

        [Test]
        public void CriarClienteAsync_ClienteInvalido_Lanca_Excecao()
        {
            // Arrange
            var cliente = new Cliente { Nome = "", Email = "invalid", Telefone = "" };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _clienteService.CriarClienteAsync(cliente));
            Assert.AreEqual("Nome é obrigatório.", ex.Message);
        }

        [Test]
        public void AtualizarClienteAsync_ClienteComIdZero_Lanca_Excecao()
        {
            // Arrange
            var cliente = new Cliente { Id = 0, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _clienteService.AtualizarClienteAsync(cliente));
            Assert.AreEqual("Id do cliente é obrigatório para atualização.", ex.Message);
        }

        [Test]
        public async Task AtualizarClienteAsync_ClienteValido_Chama_Atualizar_E_SalvarAlteracoesAsync()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };

            // Act
            await _clienteService.AtualizarClienteAsync(cliente);

            // Assert
            _mockRepo.Verify(r => r.Atualizar(cliente), Times.Once);
            _mockUnitOfWork.Verify(u => u.SalvarAlteracoesAsync(), Times.Once);
        }

        [Test]
        public void ExcluirClienteAsync_ClienteNaoEncontrado_Lanca_Excecao()
        {
            // Arrange
            _mockRepo.Setup(r => r.ObterPorIdAsync(It.IsAny<int>())).ReturnsAsync((Cliente)null);

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _clienteService.ExcluirClienteAsync(1));
            Assert.AreEqual("Cliente não encontrado.", ex.Message);
        }

        [Test]
        public async Task ExcluirClienteAsync_ClienteEncontrado_Chama_Remover_E_SalvarAlteracoesAsync()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _mockRepo.Setup(r => r.ObterPorIdAsync(cliente.Id)).ReturnsAsync(cliente);

            // Act
            await _clienteService.ExcluirClienteAsync(cliente.Id);

            // Assert
            _mockRepo.Verify(r => r.Remover(cliente), Times.Once);
            _mockUnitOfWork.Verify(u => u.SalvarAlteracoesAsync(), Times.Once);
        }
    }
}
