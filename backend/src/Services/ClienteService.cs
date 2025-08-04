using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.src.Models;
using backend.src.Repositories.Interfaces;
using backend.src.Data;

namespace backend.src.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly AppDbContext _context;
        
        public ClienteService(IClienteRepository clienteRepository, AppDbContext context)
        {
            _clienteRepository = clienteRepository;
            _context = context;
        }
        
        public async Task<(bool sucesso, string mensagem)> CriarClienteAsync(Cliente cliente)
        {
            if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Email) || string.IsNullOrEmpty(cliente.Telefone))
                return (false, "Nome, e-mail e telefone são obrigatórios");
            
            if (!cliente.Email.Contains("@"))
                return (false, "E-mail inválido");
            
            // Verifica se o e-mail já existe
            if (await _clienteRepository.ExisteEmailAsync(cliente.Email))
                return (false, "Email já existente");
            
            using (var transacao = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _clienteRepository.CriarAsync(cliente);
                    await _context.SaveChangesAsync();
                    await transacao.CommitAsync();
                    return (true, "Cliente criado com sucesso");
                }
                catch(Exception ex)
                {
                    await transacao.RollbackAsync();
                    return (false, $"Erro: {ex.Message}");
                }
            }
        }
        
        public async Task<(bool sucesso, string mensagem)> AtualizarClienteAsync(Cliente cliente)
        {
            if (cliente.Id <= 0)
                return (false, "Cliente não encontrado");
            
            if (string.IsNullOrEmpty(cliente.Nome) || string.IsNullOrEmpty(cliente.Email) || string.IsNullOrEmpty(cliente.Telefone))
                return (false, "Nome, e-mail e telefone são obrigatórios");
            
            if (!cliente.Email.Contains("@"))
                return (false, "E-mail inválido");
            
            // Verifica se o e-mail já está em uso por outro cliente
            if (await _clienteRepository.ExisteEmailAsync(cliente.Email, cliente.Id))
                return (false, "Email já existente em outro registro");
            
            using (var transacao = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _clienteRepository.AtualizarAsync(cliente);
                    await _context.SaveChangesAsync();
                    await transacao.CommitAsync();
                    return (true, "Cliente atualizado com sucesso");
                }
                catch(Exception ex)
                {
                    await transacao.RollbackAsync();
                    return (false, $"Erro: {ex.Message}");
                }
            }
        }
        
        public async Task<(bool sucesso, string mensagem)> ExcluirClienteAsync(int id)
        {
            var cliente = await _clienteRepository.ObterPorIdAsync(id);
            if (cliente == null)
                return (false, "Cliente não encontrado");
            
            using (var transacao = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    await _clienteRepository.ExcluirAsync(cliente);
                    await _context.SaveChangesAsync();
                    await transacao.CommitAsync();
                    return (true, "Cliente excluído com sucesso");
                }
                catch(Exception ex)
                {
                    await transacao.RollbackAsync();
                    return (false, $"Erro: {ex.Message}");
                }
            }
        }
        
        public async Task<Cliente> ObterClientePorIdAsync(int id)
        {
            return await _clienteRepository.ObterPorIdAsync(id);
        }
        
        public async Task<List<Cliente>> ObterTodosClientesAsync(string filtroNome = null)
        {
            var clientes = await _clienteRepository.ObterTodosAsync();
            if (!string.IsNullOrEmpty(filtroNome))
            {
                clientes = clientes.FindAll(c => c.Nome.IndexOf(filtroNome, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return clientes;
        }
        
        public async Task<List<Cliente>> GerarRelatorioClientesAsync()
        {
            // Aqui podem ser aplicadas lógicas adicionais para formatação do relatório
            return await _clienteRepository.ObterTodosAsync();
        }
    }
}
