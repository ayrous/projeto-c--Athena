using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class ServicoDTO
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [MinLength(2, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(127, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [MinLength(4, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(255, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "descricao")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "idCategoria")]
        public string IdCategoria { get; set; }

        // pode ter mais de um profissional por serviço
        [JsonProperty(PropertyName = "profissional")]
        public virtual ICollection<Profissional> Profissional { get; set; }
    }
}
