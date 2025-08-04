using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using MyApp.Services;
using System.Collections.Generic;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        /// <summary>
        /// Carrega todos os clientes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var clientes = await _clienteService.ObterTodosClientesAsync();

            // Se não houver clientes, retorna mensagem personalizada
            if (clientes == null || ((List<Cliente>)clientes).Count == 0)
            {
                return NotFound("Nenhum cliente encontrado.");
            }

            return Ok(clientes);
        }

        /// <summary>
        /// Obtém um cliente pelo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var cliente = await _clienteService.ObterClientePorIdAsync(id);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            return Ok(cliente);
        }

        /// <summary>
        /// Cria um novo cliente.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Cliente cliente)
        {
            try
            {
                await _clienteService.CriarClienteAsync(cliente);
                return Ok("Cliente criado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza um cliente existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest("O ID do cliente não confere.");
            try
            {
                await _clienteService.AtualizarClienteAsync(cliente);
                return Ok("Cliente atualizado com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Exclui um cliente.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Excluir(int id)
        {
            try
            {
                await _clienteService.ExcluirClienteAsync(id);
                return Ok("Cliente excluído com sucesso.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
