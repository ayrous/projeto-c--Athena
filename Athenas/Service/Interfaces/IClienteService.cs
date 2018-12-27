using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
    public interface IClienteService : IDisposable
    {
        //Cadastrar um cliente
        Task<Document> CadastrarCliente(Clientes cliente);

        //Pegar um único cliente
        Task<Clientes> PegarCliente(string cpf);

        Task<IEnumerable<Clientes>> ListarClientes();
    }
}
