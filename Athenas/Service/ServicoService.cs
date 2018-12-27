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
    public class ServicoService : IServicoService
    {

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //Listagem de servico
        public async Task<IEnumerable<Servico>> ListarServicos(string idAdm, string idCat)
        {
            List<Servico> servicos = (List<Servico>)await Repository<Servico>.ListarServicos(idAdm, idCat);

            if (servicos == null)
            {
                return null;
            }
            else
            {
                return servicos;
            }
        }

        //Listagem de servico
        public async Task<IEnumerable<Servico>> ListarTodosServicos(string idAdm)
        {
            List<Servico> servicos = (List<Servico>)await Repository<Servico>.ListarTodosServicos(idAdm);

            if (servicos == null)
            {
                return null;
            }
            else
            {
                return servicos;
            }
        }

        //Cadastrar um servico
        public async Task<Servico> CadastrarServico(string idAdm, string idCat, Servico servico)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            servico.IdCategoria = idCat;

            Servico vali = await Repository<Servico>.PegarServicoPorNome2(adm, servico.Nome);

            if (vali == null)
            {

                var retorno = await Repository<Servico>.CadastrarItem(servico);
                servico = await Repository<PessoaJuridica>.PegarServicoPorNome(servico.Nome);

                servico.Profissional = new List<Profissional>();

                var retorno2 = await Repository<Administrador>.CadastrarServico(adm, servico);

                await Repository<Servico>.DeletarItem(servico.Id);

                if (adm == null || servico == null || retorno == null || retorno2 == null)
                {
                    return null;
                }
                else
                {
                    return servico;
                }

            }
            else
            {
                Console.WriteLine("O nome inserido já está cadastrado!");
                return null;
            }
        }

        //Pegar um único servico
        public async Task<Servico> PegarServico(string idAdm, string id)
        {
            Servico servico = await Repository<Servico>.PegarServico(idAdm, id);

            if (servico == null)
            {
                return null;
            }
            else
            {
                return servico;
            }
        }

        //Atualizar um servico
        public async Task<Servico> AtualizarServico(string idAdm, Servico servico)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            var retorno = await Repository<Servico>.AtualizarServico(adm, servico);

            if (servico == null || adm == null || retorno == null)
            {
                return null;
            }
            else
            {
                return servico;
            }
        }

        //Deletar um servico
        public async Task DeletarServico(string idAdm, string id)
        {
            Servico servico = await Repository<Servico>.PegarServico(idAdm, id);

            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

            if (servico == null || adm == null)
            {
                throw null;
            }
            else
            {
                await Repository<Servico>.DeletarServico(servico, adm);
            }
        }
    }
}
