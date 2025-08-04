using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using backend.src.Services;
using backend.src.Models;
using backend.src.Models.Requests;
using backend.src.Models.Responses;

namespace backend.src.Controllers
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
        
        [HttpPost]
        public async Task<IActionResult> CriarCliente([FromBody] ClienteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var cliente = new Cliente
            {
                Nome = request.Nome,
                Email = request.Email,
                Telefone = request.Telefone
            };
            
            var resultado = await _clienteService.CriarClienteAsync(cliente);
            if (resultado.sucesso)
                return Ok(new { mensagem = resultado.mensagem });
            else
                return BadRequest(new { mensagem = resultado.mensagem });
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarCliente(int id, [FromBody] ClienteRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var clienteExistente = await _clienteService.ObterClientePorIdAsync(id);
            if (clienteExistente == null)
                return NotFound(new { mensagem = "Cliente não encontrado" });
            
            clienteExistente.Nome = request.Nome;
            clienteExistente.Email = request.Email;
            clienteExistente.Telefone = request.Telefone;
            
            var resultado = await _clienteService.AtualizarClienteAsync(clienteExistente);
            if (resultado.sucesso)
                return Ok(new { mensagem = resultado.mensagem });
            else
                return BadRequest(new { mensagem = resultado.mensagem });
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirCliente(int id)
        {
            var resultado = await _clienteService.ExcluirClienteAsync(id);
            if (resultado.sucesso)
                return Ok(new { mensagem = resultado.mensagem });
            else
                return BadRequest(new { mensagem = resultado.mensagem });
        }
        
        [HttpGet]
        public async Task<IActionResult> ListarClientes([FromQuery] string filtroNome)
        {
            var clientes = await _clienteService.ObterTodosClientesAsync(filtroNome);
            if (clientes == null || clientes.Count == 0)
                return Ok(new { mensagem = "Nenhum cliente encontrado" });
            
            var resposta = clientes.Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone
            }).ToList();
            return Ok(resposta);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterClientePorId(int id)
        {
            var cliente = await _clienteService.ObterClientePorIdAsync(id);
            if (cliente == null)
                return NotFound(new { mensagem = "Cliente não encontrado" });
            
            var resposta = new ClienteResponse
            {
                Id = cliente.Id,
                Nome = cliente.Nome,
                Email = cliente.Email,
                Telefone = cliente.Telefone
            };
            return Ok(resposta);
        }
        
        [HttpGet("relatorio")]
        public async Task<IActionResult> GerarRelatorioClientes()
        {
            var clientes = await _clienteService.GerarRelatorioClientesAsync();
            if (clientes == null || clientes.Count == 0)
                return Ok(new { mensagem = "Nenhum cliente encontrado" });
            
            // Para simplificação, retornamos os dados para um print preview
            var resposta = clientes.Select(c => new ClienteResponse
            {
                Id = c.Id,
                Nome = c.Nome,
                Email = c.Email,
                Telefone = c.Telefone
            }).ToList();
            return Ok(resposta);
        }
    }
}
