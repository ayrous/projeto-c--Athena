using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
    public interface IServicoService : IDisposable
    {
        //Listagem de todos servicos
        Task<IEnumerable<Servico>> ListarServicos(string idAdm, string idCat);

        //Listagem de todos servicos do sistema
        Task<IEnumerable<Servico>> ListarTodosServicos(string idAdm);

        //Cadastrar um servico
        Task<Servico> CadastrarServico(string idAdm, string idCat, Servico servico);

        //Pegar um único serviço
        Task<Servico> PegarServico(string idAdm, string id);

        //Atualizar um servico
        Task<Servico> AtualizarServico(string idAdm, Servico servico);

        //Deletar um servico
        Task DeletarServico(string idAdm, string id);
    }
}
