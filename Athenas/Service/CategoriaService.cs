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
    public class CategoriaService : ICategoriaService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //Listagem de categoria
        public async Task<IEnumerable<Categoria>> ListarCategorias(string idAdm, string idPj)
        {
            List<Categoria> categorias = (List<Categoria>)await Repository<Categoria>.ListarCategorias(idAdm, idPj);

            if (categorias == null)
            {
                return null;
            }
            else
            {
                return categorias;
            }
        }


        //Cadastrar uma categoria
        public async Task<Categoria> CadastrarCategoria(string idAdm, string idPj, Categoria cat)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            cat.IdPessoaJuridica = idPj;

            Categoria vali = await Repository<Categoria>.PegarCategoriaPorNome2(adm, cat.Nome);

            if (vali == null)
            {

                var retorno = await Repository<Categoria>.CadastrarItem(cat);
                cat = await Repository<Categoria>.PegarCategoriaPorNome(cat.Nome);

                cat.Servico = new List<Servico>();

                var retorno2 = await Repository<Administrador>.CadastrarCategoria(adm, cat);

                await Repository<Categoria>.DeletarItem(cat.Id);

                if (adm == null || cat == null || retorno == null || retorno2 == null)
                {
                    return null;
                }
                else
                {
                    return cat;
                }
            }
            else
            {
                Console.WriteLine("O nome inserido já está cadastrado!");
                return null;
            }
        }

        //Pegar uma única categoria
        public async Task<Categoria> PegarCategoria(string idAdm, string id)
        {
            Categoria categoria = await Repository<Categoria>.PegarCategoria(idAdm, id);

            if (categoria == null)
            {
                return null;
            }
            else
            {
                return categoria;
            }
        }


        //Atualizar uma categoria
        public async Task<Categoria> AtualizarCategoria(string idAdm, Categoria cat)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            var retorno = await Repository<Categoria>.AtualizarCategoria(adm, cat);

            if (adm == null || retorno == null)
            {
                return null;
            }
            else
            {
                return cat;
            }
        }


        //Deletar uma categoria
        public async Task DeletarCategoria(string idAdm, string id)
        {
            Categoria categoria = await Repository<Categoria>.PegarCategoria(idAdm, id);
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

            if (categoria == null || adm == null)
            {
                throw null;
            }
            else
            {
                await Repository<PessoaJuridica>.DeletarCategoria(categoria, adm);
            }
        }
    }
}
