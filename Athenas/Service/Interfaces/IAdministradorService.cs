using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
    public interface IAdministradorService : IDisposable
    {

		//Listagem de todos os adm
        Task<IEnumerable<Administrador>> ListarAdministradores();

		//Cadastrar um adm
		Task<Administrador> CadastrarAdm(Administrador adm);

		//Pegar um único adm por seu id
		Task<Administrador> PegarAdm(string id);

		//Atualizar um adm
		Task<Document> AtualizarAdm(string id, Administrador adm);

		//Deletar um adm
		Task DeletarAdm(string id);
		

	}
}
