using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json; 

namespace BotAthenas.Models
{ 
    public class Administrador
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nomeCompleto")]
        public string NomeCompleto { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("senha")]
        public string Senha { get; set; }
        [JsonProperty("pessoaJuridica")]
        public Pessoajuridica[] PessoaJuridica { get; set; }
    }

    public class Pessoajuridica
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("cnpj")]
        public string Cnpj { get; set; }
        [JsonProperty("nomeFantasia")]
        public string NomeFantasia { get; set; }
        [JsonProperty("horarioInicial")]
        public DateTime HorarioInicial { get; set; }
        [JsonProperty("horarioFinal")]
        public DateTime HorarioFinal { get; set; }
        [JsonProperty("endereco")]
        public object[] Endereco { get; set; }
        [JsonProperty("tiposJuridico")]
        public int TiposJuridico { get; set; }
        [JsonProperty("idAdministrador")]
        public string IdAdministrador { get; set; }
        [JsonProperty("categoria")]
        public Categoria[] Categoria { get; set; }
    }

    public class Categoria
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nome")]
        public string Nome { get; set; }
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("idPessoaJuridica")]
        public string IdPessoaJuridica { get; set; }
        [JsonProperty("servico")]
        public Servico[] Servico { get; set; }
    }

    public class Servico
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nome")]
        public string Nome { get; set; }
        [JsonProperty("descricao")]
        public string Descricao { get; set; }
        [JsonProperty("idCategoria")]
        public string IdCategoria { get; set; }
        [JsonProperty("profissional")]
        public Profissional[] Profissional { get; set; }
    }

    public class Profissional
    {

		[JsonProperty("pin")]
        public object Pin { get; set; }
        [JsonProperty("idServico")]
        public string IdServico { get; set; }
        [JsonProperty("agendamento")]
        public Agendamento[] Agendamento { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nomeCompleto")]
        public string NomeCompleto { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class Agendamento
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("dia")]
        public string Dia { get; set; }
        [JsonProperty("horario")]
        public DateTime Horario { get; set; }
        [JsonProperty("idProfissional")]
        public string IdProfissional { get; set; }
        [JsonProperty("cliente")]
        public Cliente[] Cliente { get; set; }
    }

    public class Cliente
    {
        [JsonProperty("primeiroAcesso")]
        public DateTime PrimeiroAcesso { get; set; }
        [JsonProperty("idAgendamento")]
        public string IdAgendamento { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("nomeCompleto")]
        public string NomeCompleto { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }

}