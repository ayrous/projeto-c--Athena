using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Athenas.Repository;
using Athenas.Domain;
using Athenas.Service.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;

namespace Athenas.Controllers
{

	[Route("api/[controller]/")]
    [ApiController]
    public class AdministradorController : Controller
	{
        private readonly IAdministradorService administradorService;

        public AdministradorController(IAdministradorService administradorService)
        {
            this.administradorService = administradorService;
        }

        //Lista todos os administradores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrador>>> Get()
        {
            List<Administrador> administradores = (List<Administrador>)await administradorService.ListarAdministradores();

            if (administradores == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(administradores);
            }
        }

        //Lista adm específico
        [HttpGet("{id}")]
        public async Task<Administrador> Get(string id)
        {
			return await administradorService.PegarAdm(id);
        }

		[AllowAnonymous]
		[EnableCors("AllowAllHeaders")]
        //Cadastra um adm
        [HttpPost]
        //public async Task<ActionResult<Administrador>> Post([FromBody] Administrador administrador)
        public async Task<IActionResult> Post([FromBody] Administrador administrador)
        {
            Administrador admin = await administradorService.CadastrarAdm(administrador);

            if (admin == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(admin);
            }
        }

        //Atualiza registro de determinado adm
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody]Administrador adm)
        {
              await administradorService.AtualizarAdm(id, adm);
        }

        // Deleta um adm especifico
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await administradorService.DeletarAdm(id);
        }
    }
}
