using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Repository;
using Microsoft.AspNetCore.Cors;
using Athenas.Domain;
using Athenas.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Athenas.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
	[EnableCors("AllowAllHeaders")]
    [Authorize]
    public class ProfissionalController : Controller
    {
        private readonly IProfissionalService profissionalService;
        
        public ProfissionalController(IProfissionalService profissionalService_)
        {
            this.profissionalService = profissionalService_;
        }
        
        //Lista todos os profissionais
        //GET api/profissional
        [HttpGet("{idAdm}/{idServico}")]
        public async Task<ActionResult<IEnumerable<Profissional>>> ListarProfissionais(string idAdm, string idServico)
        {
            List<Profissional> profissionais = (List<Profissional>)await profissionalService.ListarProfissionais(idAdm, idServico);
            if (profissionais == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(profissionais);
            }
        }

        //Lista profissional específico
        // GET api/<controller>/5
        [HttpGet("specific/{idAdm}/{id}")]
        public async Task<Profissional> PegarProfissional(string idAdm, string id)
        {
            Profissional profissional = await profissionalService.PegarProfissional(idAdm, id);
            return profissional;
        }

        //Cadastra Profissional
        //POST api/profissional
        [HttpPost("{idAdm}/{idServico}")]
        public async Task<ActionResult<Profissional>> CadastrarProfissional(string idAdm, string idServico, [FromBody] Profissional pro)
        {
            pro = await profissionalService.CadastrarProfissional(idAdm, idServico, pro);
            if (pro == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pro);
            }
        }

        //Atualiza registro de determinado Profissionals
        // PUT api/profissional/2
        [HttpPut("{idAdm}/{id}")]
        public async Task<ActionResult<Profissional>>  AtualizarProfissional(string idAdm, string id, [FromBody]ProfissionalDTO profissionalDTO)
        {
            Profissional profissional = await profissionalService.PegarProfissional(idAdm, id);

            if (profissional != null)
            {
                if (profissionalDTO.NomeCompleto == null)
                {
                    profissionalDTO.NomeCompleto = profissional.NomeCompleto;
                }

                if (profissionalDTO.Email == null)
                {
                    profissionalDTO.Email = profissional.Email;
                }

                if (profissionalDTO.Agendamento == null)
                {
                    profissionalDTO.Agendamento = profissional.Agendamento;
                }

                if (profissionalDTO.IdServico == null)
                {
                    profissionalDTO.IdServico = profissional.IdServico;
                }

                if (profissionalDTO.Pin == null)
                {
                    profissionalDTO.Pin = profissionalDTO.Pin;
                }
                profissionalDTO.Id = id;
            }

            profissional.NomeCompleto = profissionalDTO.NomeCompleto;
            profissional.Email = profissionalDTO.Email;
            profissional.Pin = profissionalDTO.Pin;
            profissional.Agendamento = profissionalDTO.Agendamento;
            profissional.IdServico = profissionalDTO.IdServico;
            profissional.Id = id;

            var retorno = await profissionalService.AtualizarProfissional(idAdm, profissional);

            if (retorno == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(retorno);
            }
        }

        // DELETE api/profissional/5
        [HttpDelete("{idAdm}/{id}")]
        public async void Delete(string idAdm, string id)
        {
            await profissionalService.DeletarProfissional(idAdm, id);
        }
    }
}