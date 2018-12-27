using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
    public interface ICategoriaService : IDisposable
    {
        //Listagem de todas categorias
        Task<IEnumerable<Categoria>> ListarCategorias(string idAdm, string idpj);

        //Cadastrar uma categoria
        Task<Categoria> CadastrarCategoria(string idAdm, string idPj, Categoria cat);

        //Pegar uma única categoria
        Task<Categoria> PegarCategoria(string idAdm, string id);

        //Atualizar uma categoria
        Task<Categoria> AtualizarCategoria(string idAdm, Categoria cat);

        //Deletar uma categoria
        Task DeletarCategoria(string idAdm, string id);
    }
}
