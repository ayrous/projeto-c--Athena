using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Repository;
using Athenas.Domain;
using Microsoft.AspNetCore.Cors;
using Athenas.Service.Interfaces;
using Athenas.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Athenas.Controllers
{
    [Route("api/[controller]/")]
    [EnableCors("AllowAllHeaders")]
    [ApiController]
    [Authorize]
    public class PessoaJuridicaController : Controller
    {

        private readonly IPessoaJuridicaService pessoaJuridicaService;

        public PessoaJuridicaController(IPessoaJuridicaService pessoaJuridicaService_)
        {
            this.pessoaJuridicaService = pessoaJuridicaService_;
        }

        //Lista todas as empresas
        //GET api/pessoajuridica
        [HttpGet("{idAdm}")]
        public async Task<ActionResult<IEnumerable<PessoaJuridica>>> ListarPj(string idAdm)
        {
            List<PessoaJuridica> pessoasJuridicas = (List<PessoaJuridica>)await pessoaJuridicaService.ListarPj(idAdm);

            if (pessoasJuridicas == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pessoasJuridicas);
            }
        }

        //Lista todas as empresas do sistema
        //GET api/pessoajuridica
        [HttpGet("all/{idAdm}")]
        public async Task<ActionResult<IEnumerable<PessoaJuridica>>> ListarTodasPj(string idAdm)
        {
            List<PessoaJuridica> pessoasJuridicas = (List<PessoaJuridica>)await pessoaJuridicaService.ListarTodasPj(idAdm);

            if (pessoasJuridicas == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pessoasJuridicas);
            }
        }

        //Lista uma pj específica
        [HttpGet("{idAdm}/{id}")]
        public async Task<PessoaJuridica> PegarPj(string idAdm, string id)
        {
            return await pessoaJuridicaService.PegarPj(idAdm, id);
        }

        //Cadastra uma empresa
        //POST api/pessoaJuridica
        [HttpPost("{idAdm}")]
        public async Task<ActionResult<PessoaJuridica>> CadatrarPj([FromBody] PessoaJuridica pj, string idAdm)
        {
            pj = await pessoaJuridicaService.CadastrarPj(pj, idAdm);

            if (pj == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(pj);
            }
        }

        //Atualiza registro de determinada empresa
        // PUT api/pessoaJuridica/1/2
        [HttpPut("{idAdm}/{id}")]
        public async Task<ActionResult<PessoaJuridica>> AtualizarPj(string idAdm, string id, [FromBody]PessoaJuridicaDTO pessoaJuridicaDTO)
        {
            PessoaJuridica pj = await Repository<PessoaJuridica>.PegarPj(idAdm, id);

            if (pj != null)
            {
                if (pessoaJuridicaDTO.NomeFantasia == null)
                {
                    pessoaJuridicaDTO.NomeFantasia = pj.NomeFantasia;
                }

                if (pessoaJuridicaDTO.Cnpj == null)
                {
                    pessoaJuridicaDTO.Cnpj = pj.Cnpj;
                }

                if (pessoaJuridicaDTO.HorarioInicial == null)
                {
                    pessoaJuridicaDTO.HorarioInicial = pj.HorarioInicial;
                }

                if (pessoaJuridicaDTO.HorarioFinal == null)
                {
                    pessoaJuridicaDTO.HorarioFinal = pj.HorarioFinal;
                }

                if (pessoaJuridicaDTO.Endereco == null)
                {
                    pessoaJuridicaDTO.Endereco = pj.Endereco;
                }

                if (pessoaJuridicaDTO.IdAdministrador == null)
                {
                    pessoaJuridicaDTO.IdAdministrador = pj.IdAdministrador;
                }

                if (pessoaJuridicaDTO.Categoria == null)
                {
                    pessoaJuridicaDTO.Categoria = pj.Categoria;
                }

                pessoaJuridicaDTO.Id = id;
            }

            pj.NomeFantasia = pessoaJuridicaDTO.NomeFantasia;
            pj.Cnpj = pessoaJuridicaDTO.Cnpj;
            pj.HorarioInicial = pessoaJuridicaDTO.HorarioInicial;
            pj.HorarioFinal = pessoaJuridicaDTO.HorarioFinal;
            pj.Endereco = pessoaJuridicaDTO.Endereco;
            pj.IdAdministrador = pessoaJuridicaDTO.IdAdministrador;
            pj.Categoria = pessoaJuridicaDTO.Categoria;
            pj.Id = id;

            var retorno = await pessoaJuridicaService.AtualizarPj(idAdm, id, pj);

            if (retorno == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(retorno);
            }
        }

        // DELETE api/pessoaJuridica/5
        [HttpDelete("{idAdm}/{id}")]
        public async void DeletarPj(string idAdm, string id)
        {
            await pessoaJuridicaService.DeletarPj(idAdm, id);
        }
    }
}