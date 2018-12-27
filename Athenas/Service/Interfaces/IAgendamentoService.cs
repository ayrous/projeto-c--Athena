using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Domain;
using Microsoft.Azure.Documents;

namespace Athenas.Service.Interfaces
{
    public interface IAgendamentoService : IDisposable
    {
        //Listagem de todos agendamentos
        Task<IEnumerable<Agendamento>> ListarAgendamentos(string idAdm, string idPro);

        //Cadastrar um agendamento
        Task<Agendamento> CadastrarAgendamento(string idAdm, string idPro, Agendamento agendamento);

        //Pegar um único agendamento
        Task<Agendamento> PegarAgendamento(string idAdm, string id);

        //Atualizar um agendamento
        Task<Agendamento> AtualizarAgendamento(string idAdm, Agendamento agendamento);

        //Deletar um agendamento
        Task DeletarAgendamento(string idAdm, string id);
    }
}
