using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class AgendamentoDTO
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DataType(DataType.Date, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "dia")]
        public DateTime Dia { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "horario")]
        public DateTime Horario { get; set; }

        [JsonProperty(PropertyName = "idProfissional")]
        public string IdProfissional { get; set; }

        // um horario possui apenas um cliente
        [JsonProperty(PropertyName = "cliente")]
        public virtual ICollection<Clientes> Cliente { get; set; }
    }
}
