using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class PessoaJuridica
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        [JsonProperty(PropertyName = "cnpj")]
        public string Cnpj { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        //[MinLength(4, ErrorMessage = ErrorBase.erro_min)]
        //[MaxLength(31, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "nomeFantasia")]
        public string NomeFantasia { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        //[DataType(DataType.DateTime, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "horarioInicial")]
        public DateTime HorarioInicial { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        //[DataType(DataType.DateTime, ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "horarioFinal")]
        public DateTime HorarioFinal { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        [JsonProperty(PropertyName = "endereco")]
        public Endereco Endereco { get; set; }

        //[Required(ErrorMessage = ErrorBase.erro_nec)]
        [JsonProperty(PropertyName = "tiposJuridico")]
        public TiposJuridico TiposJuridico { get; set; }
        
        [JsonProperty(PropertyName = "idAdministrador")]
        public string IdAdministrador { get; set; }
        
        [JsonProperty(PropertyName = "categoria")]
        public virtual ICollection<Categoria> Categoria { get; set; }
    }
}
