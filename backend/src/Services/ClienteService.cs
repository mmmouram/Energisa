using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyApp.Models;
using MyApp.Repositories;

namespace MyApp.Services
{
    public class ClienteService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Cliente>> ObterTodosClientesAsync()
        {
            return await _customerRepository.ObterTodosAsync();
        }

        public async Task<Cliente?> ObterClientePorIdAsync(int id)
        {
            return await _customerRepository.ObterPorIdAsync(id);
        }

        public async Task CriarClienteAsync(Cliente cliente)
        {
            ValidarCliente(cliente);
            await _customerRepository.AdicionarAsync(cliente);
            await _unitOfWork.SalvarAlteracoesAsync();
        }

        public async Task AtualizarClienteAsync(Cliente cliente)
        {
            if (cliente.Id == 0)
                throw new ArgumentException("Id do cliente é obrigatório para atualização.");

            ValidarCliente(cliente);
            _customerRepository.Atualizar(cliente);
            await _unitOfWork.SalvarAlteracoesAsync();
        }

        public async Task ExcluirClienteAsync(int id)
        {
            var cliente = await _customerRepository.ObterPorIdAsync(id);
            if (cliente == null)
                throw new ArgumentException("Cliente não encontrado.");

            _customerRepository.Remover(cliente);
            await _unitOfWork.SalvarAlteracoesAsync();
        }

        private void ValidarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(cliente.Email) || !cliente.Email.Contains("@"))
                throw new ArgumentException("E-mail inválido.");

            if (string.IsNullOrWhiteSpace(cliente.Telefone))
                throw new ArgumentException("Telefone é obrigatório.");
        }
    }
}
