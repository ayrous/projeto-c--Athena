using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
	public interface IPessoaJuridicaService : IDisposable
	{
        //Cadastrar uma pj
        Task<PessoaJuridica> CadastrarPj(PessoaJuridica pj, string idAdm);

        //Pegar uma única empresa pelo cnpj
        Task<PessoaJuridica> PegarPjPorCnpj(string cnpj);

        //Pegar uma única empresa
        Task<PessoaJuridica> PegarPj(string idAdm, string id);

        //Listagem de todos as empresas
        Task<IEnumerable<PessoaJuridica>> ListarPj(string idAdm);

        //Listagem de todos as empresas do sistema
        Task<IEnumerable<PessoaJuridica>> ListarTodasPj(string idAdm);

        //Atualizar uma empresa
        Task<PessoaJuridica> AtualizarPj(string idAdm, string id, PessoaJuridica emp);

        //Deletar uma empresa
        Task DeletarPj(string idAdm, string id);
    }
}
