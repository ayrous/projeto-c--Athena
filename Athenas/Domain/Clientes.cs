using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class Clientes
    {
        [Key]
        [JsonProperty("cpfCliente")]
        public string CpfCliente { get; set; }
        [JsonProperty("dia")]
        public string Dia { get; set; }
        [JsonProperty("horario")]
        public string Horario { get; set; }
        [JsonProperty("funcionario")]
        public string Funcionario { get; set; }
        //[JsonProperty("servico")]
        //public string Servico { get; set; }
        [JsonProperty("nomeCliente")]
        public string NomeCliente { get; set; }
    }
}
