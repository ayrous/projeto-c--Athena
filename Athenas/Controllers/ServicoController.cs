using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Repository;
using Microsoft.AspNetCore.Cors;
using Athenas.Domain;
using Athenas.Controllers;
using Microsoft.AspNetCore.Authorization;
using Athenas.Service.Interfaces;

namespace Athenas.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
	[EnableCors("AllowAllHeaders")]
	[Authorize]
    public class ServicoController : Controller
    {
		private readonly IServicoService servicoService;

		public ServicoController(IServicoService servicoService_)
		{
			this.servicoService = servicoService_;
		}

		//Lista todos os servicos
		//GET api/servico
		[HttpGet("{idAdm}/{idCat}")]
        public async Task<ActionResult<IEnumerable<Servico>>> ListarServicos(string idAdm, string idCat)
        {
            List<Servico> servicos = (List<Servico>)await servicoService.ListarServicos(idAdm, idCat);
            if (servicos == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(servicos);
            }
        }

        //Lista todos os servicos do sistema
        //GET api/servico
        [HttpGet("all/{idAdm}/{idCat}")]
        public async Task<ActionResult<IEnumerable<Servico>>> ListarTodosServicos(string idAdm)
        {
            List<Servico> servicos = (List<Servico>)await servicoService.ListarTodosServicos(idAdm);
            if (servicos == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(servicos);
            }
        }

        //Lista servico específico
        // GET api/<controller>/5
        [HttpGet("specific/{idAdm}/{id}")]
        public async Task<Servico> PegarServico(string idAdm, string id)
        {
            Servico servico = await servicoService.PegarServico(idAdm, id);
            return servico;
        }

        //Cadastra um servico
        //POST api/<controller>
        [HttpPost("{idAdm}/{idCat}")]
		public async Task<ActionResult<Servico>> CadastrarServico(string idAdm, string idCat, [FromBody] Servico servico)
		{
            servico = await servicoService.CadastrarServico(idAdm, idCat, servico);

            if (servico == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(servico);
            }            
		}

        //Atualiza registro de determinado servico
        // PUT api/<controller>/5
        [HttpPut("{idAdm}/{id}")]
        public async Task<ActionResult<Servico>> AtualizarServico(string idAdm,string id, [FromBody]ServicoDTO servicoDTO)
        {
            Servico serv = await servicoService.PegarServico(idAdm, id);

            if (serv != null)
            {
                if (servicoDTO.Nome == null)
                {
                    servicoDTO.Nome = serv.Nome;
                }

                if (servicoDTO.Descricao == null)
                {
                    servicoDTO.Descricao = serv.Descricao;
                }

                if (servicoDTO.Profissional == null)
                {
                    servicoDTO.Profissional = serv.Profissional;
                }

                if (servicoDTO.IdCategoria == null)
                {
                    servicoDTO.IdCategoria = serv.IdCategoria;
                }
                servicoDTO.Id = id;
            }

            serv.Nome = servicoDTO.Nome;
            serv.Descricao = servicoDTO.Descricao;
            serv.Profissional = servicoDTO.Profissional;
            serv.IdCategoria = servicoDTO.IdCategoria;
            serv.Id = id;

            var retorno = await servicoService.AtualizarServico(idAdm, serv);

            if (retorno == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(retorno);
            }
        }

        // DELETE api/<controller>/5
        [HttpDelete("{idAdm}/{id}")]
        public async void DeletarServico(string idAdm, string id)
        {
            await servicoService.DeletarServico(idAdm, id);
        }
    }
}