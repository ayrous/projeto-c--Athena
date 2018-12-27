using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athenas.Domain;
using Microsoft.AspNetCore.Cors;
using Athenas.Repository;
using Athenas.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Athenas.Controllers;

namespace Athenas.Controllers
{
	[Route("api/[controller]/")]
	[ApiController]
	[EnableCors("AllowAllHeaders")]
	[Authorize]
    public class CategoriaController : Controller
    {
		private readonly ICategoriaService categoriaService;

	    public CategoriaController(ICategoriaService categoriaService_)
		{
			this.categoriaService = categoriaService_;
		}
        
		//Lista todos os categorias
		//GET api/categoria
		[HttpGet("{idAdm}/{idPj}")]
        public async Task<ActionResult<IEnumerable<Categoria>>> ListarCategorias(string idAdm, string idPj)
        {
            List<Categoria> categorias = (List<Categoria>)await categoriaService.ListarCategorias(idAdm, idPj);

            if (categorias == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(categorias);
            }
		}

        //Lista categoria específico
        // GET api/<controller>/5
        [HttpGet("specific/{idAdm}/{id}")]
        public async Task<Categoria> PegarCategoria(string idAdm, string id)
        {
            return await categoriaService.PegarCategoria(idAdm, id);
        }

		//Cadastra um categoria
		//POST api/<controller>
		[HttpPost("{idAdm}/{idPj}")]
        public async Task<ActionResult<Categoria>> CadastrarCategoria(string idAdm, string idPj, [FromBody] Categoria cat)
		{
            cat = await categoriaService.CadastrarCategoria(idAdm, idPj, cat);
            if (cat == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cat);
            }
        }
        
		//Atualiza registro de determinado categoria
		// PUT api/<controller>/5
		[HttpPut("{idAdm}/{id}")]
        public async Task<ActionResult<Categoria>> AtualizarCategoria(string idAdm, string id, [FromBody]CategoriaDTO categoriaDTO)
        {
            Categoria cat = await categoriaService.PegarCategoria(idAdm, id);

            if (cat != null)
            {
                if (categoriaDTO.Nome == null)
                {
                    categoriaDTO.Nome = cat.Nome;
                }

                if (categoriaDTO.Descricao == null)
                {
                    categoriaDTO.Descricao = cat.Descricao;
                }

                if (categoriaDTO.Servico == null)
                {
                    categoriaDTO.Servico = cat.Servico;
                }

                if (categoriaDTO.IdPessoaJuridica == null)
                {
                    categoriaDTO.IdPessoaJuridica = cat.IdPessoaJuridica;
                }
                categoriaDTO.Id = id;
            }

            cat.Nome = categoriaDTO.Nome;
            cat.Descricao = categoriaDTO.Descricao;
            cat.Servico = categoriaDTO.Servico;
            cat.IdPessoaJuridica = categoriaDTO.IdPessoaJuridica;
            cat.Id = id;

            var retorno = await categoriaService.AtualizarCategoria(idAdm, cat);

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
        public async void DeletarCategoria(string idAdm, string id)
        {
            await categoriaService.DeletarCategoria(idAdm, id);
        }
    }
}