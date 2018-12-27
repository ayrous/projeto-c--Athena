using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Repository;
using Microsoft.AspNetCore.Cors;
using Athenas.Service.Interfaces;
using Athenas.Domain;

namespace Athenas.Controllers
{
    [Route("api/[controller]/")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    public class AgendamentoController : Controller
    {
        private readonly IAgendamentoService agendamentoService;

        public AgendamentoController(IAgendamentoService agendamentoService_)
        {
            this.agendamentoService = agendamentoService_;
        }

        //Lista todos os agendamentos
        //GET api/agendamento
        [HttpGet("{idAdm}/{idProf}")]
        public async Task<ActionResult<IEnumerable<Agendamento>>> ListarAgendamentos(string idAdm, string idProf)
        {
            List<Agendamento> agendamentos = (List<Agendamento>)await agendamentoService.ListarAgendamentos(idAdm, idProf);
            if (agendamentos == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(agendamentos);
            }
        }

        //Cadastra um agendamento
        //POST api/<controller>
        [HttpPost("{idAdm}/{idProf}")]
        public async Task<ActionResult<Agendamento>> CadastrarAgendamento(string idAdm, string idProf, [FromBody] Agendamento agen)
        {
            agen = await agendamentoService.CadastrarAgendamento(idAdm, idProf, agen);
            if (agen == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(agen);
            }
        }

        //Lista agendamento específico
        // GET api/<controller>/5
        [HttpGet("specific/{idAdm}/{id}")]
        public async Task<Agendamento> PegarAgendamento(string idAdm, string id)
        {
            Agendamento agendamento = await agendamentoService.PegarAgendamento(idAdm, id);
            return agendamento;
        }

        //Atualiza registro de determinado profissional
        // PUT api/<controller>/5
        [HttpPut("{idAdm}/{id}")]
        public async Task<ActionResult<Agendamento>> AtualizarAgendamento(string idAdm, string id, [FromBody]AgendamentoDTO agendamentoDTO)
        {
            Agendamento agendamento = await agendamentoService.PegarAgendamento(idAdm, id);

            if (agendamento != null)
            {
                if (agendamentoDTO.Dia == null)
                {
                    agendamentoDTO.Dia = agendamento.Dia;
                }

                if (agendamentoDTO.Horario == null)
                {
                    agendamentoDTO.Horario = agendamento.Horario;
                }

                if (agendamentoDTO.Cliente == null)
                {
                    agendamentoDTO.Cliente = agendamento.Cliente;
                }

                if (agendamentoDTO.IdProfissional == null)
                {
                    agendamentoDTO.IdProfissional = agendamento.IdProfissional;
                }
                agendamentoDTO.Id = id;
            }

            agendamento.Dia = agendamentoDTO.Dia;
            agendamento.Horario = agendamentoDTO.Horario;
            agendamento.Cliente = agendamentoDTO.Cliente;
            agendamento.IdProfissional = agendamentoDTO.IdProfissional;
            agendamento.Id = id;

            var retorno = await agendamentoService.AtualizarAgendamento(idAdm, agendamento);

            if (retorno == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(retorno);
            }
        }

        //// DELETE api/<controller>/5
        [HttpDelete("{idAdm}/{id}")]
        public async void DeletarAgendamento(string idAdm, string id)
        {
            await agendamentoService.DeletarAgendamento(idAdm, id);
        }
    }
}