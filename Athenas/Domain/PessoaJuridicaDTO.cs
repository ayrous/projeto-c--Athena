using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class PessoaJuridicaDTO
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        
        [JsonProperty(PropertyName = "cnpj")]
        public string Cnpj { get; set; }
        
        [JsonProperty(PropertyName = "nomeFantasia")]
        public string NomeFantasia { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "horarioInicial")]
        public DateTime HorarioInicial { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "horarioFinal")]
        public DateTime HorarioFinal { get; set; }

        [JsonProperty(PropertyName = "endereco")]
        public Endereco Endereco { get; set; }

        [JsonProperty(PropertyName = "tiposJuridico")]
        public TiposJuridico TiposJuridico { get; set; }

        [JsonProperty(PropertyName = "idAdministrador")]
        public string IdAdministrador { get; set; }

        [JsonProperty(PropertyName = "categoria")]
        public virtual ICollection<Categoria> Categoria { get; set; }
    }
}
