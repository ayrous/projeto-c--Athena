using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athenas.Domain;
using Athenas.Repository;
using Microsoft.AspNetCore.Cors;
using Athenas.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Athenas.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
	[EnableCors("AllowAllHeaders")]
	[Authorize]
    public class ClienteController : Controller
    {

        private readonly IClienteService clienteService;

        public ClienteController(IClienteService clienteService_)
        {
            this.clienteService = clienteService_;
        }

        [HttpPost]
        public async void CadastrarCliente([FromBody] Clientes cliente)
        {
            await clienteService.CadastrarCliente(cliente);
        }

        //Lista agendamento específico
        // GET api/<controller>/5
        [HttpGet("{cpf}")]
        public async Task<Clientes> PegarCliente(string cpf)
        {
            Clientes cliente = await clienteService.PegarCliente(cpf);
            return cliente;
        }

        //Lista todos os clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clientes>>> ListarClientes()
        {
            List<Clientes> clientes = (List<Clientes>)await clienteService.ListarClientes();

            if (clientes == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(clientes);
            }
        }

    }
}