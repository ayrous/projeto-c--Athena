using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Athenas.JwtDomains;
using Athenas.Domain;
using Athenas.Repository;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Athenas.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AdministradorDTO loginModel)
        {
            //List<Administrador> administradores = (List<Administrador>)await Repository<Administrador>.ListarAdm();

            //Administrador admin = administradores.FirstOrDefault(x => x.Email == loginModel.Email);

            Administrador admin = await Repository<Administrador>.PegarAdmPorEmail(loginModel.Email);

            if (admin == null)
                return Unauthorized();

            loginModel.HashearSenha();

            if (admin.Senha != loginModel.Senha)
            {
                admin = null;
            } 
            
            if (admin == null)
                return Unauthorized();
            else
            {
                var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create("a-password-very-big-to-be-good"))
                                .AddIssuer("admin.com")
                                .AddAudience("admin.com")
                                .AddNameId(admin.Email)
                                .AddClaim("administrador", admin.Id)
                                .AddExpiry(1440)
                                .Build();
                

                JObject o = new JObject
                {
                    { "token", token }
                };

                return Ok(o);
            }
        }
    }
}