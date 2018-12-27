using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class Profissional : Usuario
    {
        [JsonProperty(PropertyName = "pin")]
        public string Pin { get; set; }

        [JsonProperty(PropertyName = "idServico")]
        public string IdServico { get; set; }

        // o profissional possui uma lista de agendamentos
        [JsonProperty(PropertyName = "agendamento")]
        public virtual ICollection<Agendamento> Agendamento { get; set; }
    }
}
