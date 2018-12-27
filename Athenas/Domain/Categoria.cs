using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Athenas.Domain
{
    public class Categoria
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(3, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(63, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(4, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(255, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "descricao")]
        public string Descricao { get; set; }

        [JsonProperty(PropertyName = "idPessoaJuridica")]
        public string IdPessoaJuridica { get; set; }

        // uma categoria tem uma lista de serviços - cabelereiro, manicure, etc
        [JsonProperty(PropertyName = "servico")]
        public virtual ICollection<Servico> Servico { get; set; }
    }
}
