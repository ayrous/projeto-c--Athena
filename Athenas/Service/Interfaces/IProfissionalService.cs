using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;
namespace Athenas.Service.Interfaces
{
    public interface IProfissionalService : IDisposable
    {
        //Listagem de todos profissionais
        Task<IEnumerable<Profissional>> ListarProfissionais(string idAdm, string idSer);

        //Cadastrar um profissional
        Task<Profissional> CadastrarProfissional(string idAdm, string idServico, Profissional pro);

        //Pegar um único profissional
        Task<Profissional> PegarProfissional(string idAdm, string id);

        //Atualizar um profissional
        Task<Profissional> AtualizarProfissional(string idAdm, Profissional profissional);

        //Deletar um profissional
        Task DeletarProfissional(string idAdm, string id);

    }
}
