using System;
using System.Collections.Generic;
using System.Linq;
using Athenas.JwtDomains;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Athenas.Domain;
using Athenas.Repository;
using Athenas.Service;
using Athenas.Service.Interfaces;

namespace Athenas
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
					.AddJwtBearer(options => {
						options.TokenValidationParameters = new TokenValidationParameters
						{
							ValidateIssuer = true,
							ValidateAudience = true,
							ValidateLifetime = true,
							ValidateIssuerSigningKey = true,
							ValidIssuer = "admin.com",
							ValidAudience = "admin.com",
							IssuerSigningKey = JwtSecurityKey.Create("a-password-very-big-to-be-good")
						};

						options.Events = new JwtBearerEvents
						{
							OnAuthenticationFailed = context =>
							{
								Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
								return Task.CompletedTask;
							},
							OnTokenValidated = context =>
							{
								Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
								return Task.CompletedTask;
							}
						};
					});

			services.RegisterServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

			app.UseAuthentication();
			app.UseCors("AllowAllHeaders");

            app.UseMvc();

            Repository<Administrador>.Initialize("Collection");
            Repository<PessoaJuridica>.Initialize("Collection");
            Repository<Categoria>.Initialize("Collection");
            Repository<Servico>.Initialize("Collection");
            Repository<Profissional>.Initialize("Collection");
            Repository<Agendamento>.Initialize("Collection");
            Repository<Clientes>.Initialize("Collection");	
        }
    }

    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterServices(
            this IServiceCollection services)
        {
            services.AddTransient<IAdministradorService, AdministradorService>();
			services.AddTransient<IPessoaJuridicaService, PessoaJuridicaService>();
            services.AddTransient<IServicoService, ServicoService>();
            services.AddTransient<IClienteService, ClienteService>();
            services.AddTransient<ICategoriaService, CategoriaService>();
            services.AddTransient<IProfissionalService, ProfissionalService>();
            services.AddTransient<IAgendamentoService, AgendamentoService>();
            return services;
        }
    }
}
