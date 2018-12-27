using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Athenas.Service.Interfaces;
using Microsoft.Azure.Documents;
using Athenas.Domain;
using Athenas.Repository;

namespace Athenas.Service
{
    public class ClienteService : IClienteService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //Cadastrar um cliente
        public async Task<Document> CadastrarCliente(Clientes cliente)
        {
            return await Repository<Clientes>.CadastrarCliente(cliente);

        }

        //Pegar um único cliente
        public async Task<Clientes> PegarCliente(string cpf)
        {
            Clientes cliente = await Repository<Clientes>.PegarCliente(cpf);

            if (cliente == null)
            {
                return null;
            }
            else
            {
                return cliente;
            }
        }

        public async Task<IEnumerable<Clientes>> ListarClientes()
        {
            List<Clientes> clientes = (List<Clientes>)await Repository<Clientes>.ListarClientes();

            if (clientes == null)
            {
                throw new ArgumentException("O ID informado está incorreto ou ele não existe.", "adm");
            }
            else
            {
                return clientes;
            }
        }
    }
}
