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
    public class AgendamentoService : IAgendamentoService
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        //Listagem de agendamento
        public async Task<IEnumerable<Agendamento>> ListarAgendamentos(string idAdm, string idPro)
        {
            List<Agendamento> agendamentos = (List<Agendamento>)await Repository<Agendamento>.ListarAgendamentos(idAdm, idPro);

            if (agendamentos == null)
            {
                return null;
            }
            else
            {
                return agendamentos;
            }
        }


        //Cadastrar um agendamento
        public async Task<Agendamento> CadastrarAgendamento(string idAdm, string idProf, Agendamento agen)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            agen.IdProfissional = idProf;

            Agendamento vali = await Repository<Agendamento>.PegarAgendamentoPeloHorario2(adm, agen.Horario);

            if (vali == null)
            {

                var retorno = await Repository<Agendamento>.CadastrarItem(agen);

                agen = await Repository<Agendamento>.PegarAgendamentoPeloHorario(agen.Horario);

                agen.Cliente = new List<Clientes>();

                var retorno2 = await Repository<Administrador>.CadastrarAgendamento(adm, agen);

                await Repository<Servico>.DeletarItem(agen.Id);

                if (adm == null || agen == null || retorno == null || retorno2 == null)
                {
                    return null;
                }
                else
                {
                    return agen;
                }
            }
            else
            {
                Console.WriteLine("O horário inserido já está cadastrado!");
                return null;
            }
        }

        //Pegar um único agendamento
        public async Task<Agendamento> PegarAgendamento(string idAdm, string id)
        {
            Agendamento agendamento = await Repository<Agendamento>.PegarAgendamento(idAdm, id);

            if (agendamento == null)
            {
                return null;
            }
            else
            {
                return agendamento;
            }
        }


        //Atualizar um agendamento
        public async Task<Agendamento> AtualizarAgendamento(string idAdm, Agendamento agendamento)
        {
            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);
            var retorno = await Repository<Agendamento>.AtualizarAgendamento(adm, agendamento);

            if (agendamento == null || adm == null || retorno == null)
            {
                return null;
            }
            else
            {
                return agendamento;
            }
        }


        //Deletar um agendamento
        public async Task DeletarAgendamento(string idAdm, string id)
        {
            Agendamento agendamento = await Repository<Agendamento>.PegarAgendamento(idAdm, id);

            Administrador adm = await Repository<Administrador>.PegarAdm(idAdm);

            if (agendamento == null || adm == null)
            {
                throw null;
            }
            else
            {
                await Repository<Agendamento>.DeletarAgendamento(agendamento, adm);
            }
        }
    }
}
