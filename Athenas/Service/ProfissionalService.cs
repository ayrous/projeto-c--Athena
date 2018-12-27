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
    public class ProfissionalService : IProfissionalService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        //Listagem de profissional
        public async Task<IEnumerable<Profissional>> ListarProfissionais(string idAdm, string idSer)
        {
            List<Profissional> profissionais = (List<Profissional>)await Repository<Profissional>.ListarProfissionais(idAdm, idSer);
            if (profissionais == null)
            {
                return null;
            }
            else
            {
                return profissionais;
            }
        }

        //Cadastrar um profissional
        public async Task<Profissional> CadastrarProfissional(string idAdm, string idServico, Profissional pro)
        {

            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            pro.IdServico = idServico;

            Profissional vali = await Repository<Profissional>.PegarProfissionalPorEmail2(adm, pro.Email);

            if (vali == null)
            {
                var retorno = await Repository<Profissional>.CadastrarItem(pro);

                pro = await Repository<Profissional>.PegarProfissionalPorEmail(pro.Email);

                pro.Agendamento = new List<Agendamento>();

                var retorno2 = await Repository<Administrador>.CadastrarProfissional(adm, pro);

                await Repository<Servico>.DeletarItem(pro.Id);

                if (adm == null || pro == null || retorno == null || retorno2 == null)
                {
                    return null;
                }
                else
                {
                    return pro;
                }

            }
            else
            {
                Console.WriteLine("O email inserido já está cadastrado!");
                return null;
            }
        }

        //Pegar um único profissional
        public async Task<Profissional> PegarProfissional(string idAdm, string id)
        {
            Profissional profissional = await Repository<Profissional>.PegarProfissional(idAdm, id);

            if (profissional == null)
            {
                throw null;
            }
            else
            {
                return profissional;
            }
        }

        //Atualizar um profissional
        public async Task<Profissional> AtualizarProfissional(string idAdm, Profissional profissional)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            var retorno = await Repository<Profissional>.AtualizarProfissional(adm, profissional);

            if (profissional == null || adm == null || retorno == null)
            {
                return null;
            }
            else
            {
                return profissional;
            }
        }

        //Deletar um profissional
        public async Task DeletarProfissional(string idAdm, string id)
        {
            Profissional profissional = await Repository<Profissional>.PegarProfissional(idAdm, id);
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

            if (profissional == null || adm == null)
            {
                throw null;
            }
            else
            {
                await Repository<Profissional>.DeletarProfissional(profissional, adm);
            }
        }
    }
}
