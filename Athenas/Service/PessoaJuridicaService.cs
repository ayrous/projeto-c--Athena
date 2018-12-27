using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Service.Interfaces;
using Athenas.Domain;
using Athenas.Repository;
using Microsoft.Azure.Documents;
using System.Net.Http;
using System.Net;

namespace Athenas.Service
{
    public class PessoaJuridicaService : IPessoaJuridicaService
    {

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public async Task<PessoaJuridica> PegarPjPorCnpj(string cnpj)
        {
            PessoaJuridica pj = await Repository<PessoaJuridica>.PegarPjPorCnpj(cnpj);

            if (pj == null)
            {
                return null;
            }
            else
            {
                return pj;
            }
        }

        public async Task<PessoaJuridica> PegarPj(string idAdm, string id)
        {
            PessoaJuridica pessoaJuridica = await Repository<PessoaJuridica>.PegarPj(idAdm, id);

            if (pessoaJuridica == null)
            {
                return null;
            }
            else
            {
                return pessoaJuridica;
            }
        }

        public async Task<PessoaJuridica> CadastrarPj(PessoaJuridica pj, string idAdm)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            pj.IdAdministrador = idAdm;

            PessoaJuridica vali = await Repository<PessoaJuridica>.PegarPjPorCnpj2(adm, pj.Cnpj);

            if (vali == null)
            {
                var retorno = await Repository<PessoaJuridica>.CadastrarItem(pj);
                pj = await Repository<PessoaJuridica>.PegarPjPorCnpj(pj.Cnpj);

                pj.Categoria = new List<Categoria>();

                var retorno2 = await Repository<Administrador>.CadastrarPj(adm, pj);

                await Repository<PessoaJuridica>.DeletarItem(pj.Id);

                if (adm == null || pj == null || retorno == null || retorno2 == null)
                {
                    return null;
                }
                else
                {
                    return pj;
                }

            }
            else
            {
                Console.WriteLine("O CNPJ inserido já está cadastrado!");
                return null;
            }
        }

        //Lista todas as empresas de determinado adm
        public async Task<IEnumerable<PessoaJuridica>> ListarPj(string idAdm)
        {
            List<PessoaJuridica> pessoasJuridicas = (List<PessoaJuridica>)await Repository<PessoaJuridica>.ListarPj(idAdm);

            if (pessoasJuridicas == null)
            {
                return null;
            }
            else
            {
                return pessoasJuridicas;
            }
        }

        //Lista todas as empresas do sistema
        public async Task<IEnumerable<PessoaJuridica>> ListarTodasPj(string idAdm)
        {
            List<PessoaJuridica> pessoasJuridicas = (List<PessoaJuridica>)await Repository<PessoaJuridica>.ListarTodasPj(idAdm);

            if (pessoasJuridicas == null)
            {
                return null;
            }
            else
            {
                return pessoasJuridicas;
            }
        }

        //Atualizar uma empresa
        public async Task<PessoaJuridica> AtualizarPj(string idAdm, string id, PessoaJuridica pj)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            var retorno = await Repository<PessoaJuridica>.AtualizarPj(pj, adm);

            if (adm == null || retorno == null)
            {
                return null;
            }
            else
            {
                return pj;
            }
        }

        //Deletar uma empresa
        public async Task DeletarPj(string idAdm, string id)
        {
            PessoaJuridica pessoaJuridica = await Repository<PessoaJuridica>.PegarPj(idAdm, id);
            if (pessoaJuridica == null)
            {
                throw null;
            }
            else
            {
                Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

                await Repository<Administrador>.DeletarPj(pessoaJuridica, adm);
            }
        }
    }
}
