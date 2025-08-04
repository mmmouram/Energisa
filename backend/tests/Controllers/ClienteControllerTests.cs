using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MyApp.Controllers;
using MyApp.Models;
using MyApp.Services;
using NUnit.Framework;

namespace MyApp.Tests.Controllers
{
    [TestFixture]
    public class ClienteControllerTests
    {
        private Mock<ClienteService> _mockService;
        private ClienteController _controller;

        [SetUp]
        public void Setup()
        {
            // Como o ClienteService não é uma interface, usamos o Mock com parâmetros nulos
            _mockService = new Mock<ClienteService>(MockBehavior.Strict, null, null);
            _controller = new ClienteController(_mockService.Object);
        }

        [Test]
        public async Task ObterTodos_Retorna_NotFound_Quando_Nenhum_Cliente()
        {
            // Arrange
            _mockService.Setup(s => s.ObterTodosClientesAsync())
                .ReturnsAsync(new List<Cliente>());

            // Act
            var result = await _controller.ObterTodos();

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Nenhum cliente encontrado.", notFoundResult.Value);
            _mockService.Verify(s => s.ObterTodosClientesAsync(), Times.Once);
        }

        [Test]
        public async Task ObterTodos_Retorna_Ok_Quando_Existem_Clientes()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" }
            };
            _mockService.Setup(s => s.ObterTodosClientesAsync())
                .ReturnsAsync(clientes);

            // Act
            var result = await _controller.ObterTodos();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(clientes, okResult.Value);
            _mockService.Verify(s => s.ObterTodosClientesAsync(), Times.Once);
        }

        [Test]
        public async Task ObterPorId_Retorna_NotFound_Quando_Cliente_Nao_Existe()
        {
            // Arrange
            _mockService.Setup(s => s.ObterClientePorIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Cliente)null);

            // Act
            var result = await _controller.ObterPorId(1);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFound = result as NotFoundObjectResult;
            Assert.AreEqual("Cliente não encontrado.", notFound.Value);
        }

        [Test]
        public async Task ObterPorId_Retorna_Ok_Quando_Cliente_Existe()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _mockService.Setup(s => s.ObterClientePorIdAsync(cliente.Id))
                .ReturnsAsync(cliente);

            // Act
            var result = await _controller.ObterPorId(cliente.Id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(cliente, okResult.Value);
        }

        [Test]
        public async Task Criar_Retorna_Ok_Quando_Criacao_For_Sucesso()
        {
            // Arrange
            var cliente = new Cliente { Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _mockService.Setup(s => s.CriarClienteAsync(cliente))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Criar(cliente);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Cliente criado com sucesso.", okResult.Value);
        }

        [Test]
        public async Task Criar_Retorna_BadRequest_Quando_Excecao()
        {
            // Arrange
            var cliente = new Cliente { Nome = "", Email = "invalid", Telefone = "" };
            _mockService.Setup(s => s.CriarClienteAsync(cliente))
                .ThrowsAsync(new Exception("Erro de validação"));

            // Act
            var result = await _controller.Criar(cliente);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Erro de validação", badResult.Value);
        }

        [Test]
        public async Task Atualizar_Retorna_BadRequest_Quando_Id_Nao_Conferem()
        {
            // Arrange
            var cliente = new Cliente { Id = 2, Nome = "Test", Email = "test@test.com", Telefone = "12345" };

            // Act
            var result = await _controller.Atualizar(1, cliente);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("O ID do cliente não confere.", badResult.Value);
        }

        [Test]
        public async Task Atualizar_Retorna_Ok_Quando_Sucesso()
        {
            // Arrange
            var cliente = new Cliente { Id = 1, Nome = "Test", Email = "test@test.com", Telefone = "12345" };
            _mockService.Setup(s => s.AtualizarClienteAsync(cliente))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Atualizar(cliente.Id, cliente);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Cliente atualizado com sucesso.", okResult.Value);
        }

        [Test]
        public async Task Excluir_Retorna_Ok_Quando_Sucesso()
        {
            // Arrange
            _mockService.Setup(s => s.ExcluirClienteAsync(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Excluir(1);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual("Cliente excluído com sucesso.", okResult.Value);
        }

        [Test]
        public async Task Excluir_Retorna_BadRequest_Quando_Excecao()
        {
            // Arrange
            _mockService.Setup(s => s.ExcluirClienteAsync(It.IsAny<int>()))
                .ThrowsAsync(new Exception("Erro ao excluir"));

            // Act
            var result = await _controller.Excluir(1);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badResult = result as BadRequestObjectResult;
            Assert.AreEqual("Erro ao excluir", badResult.Value);
        }
    }
}
