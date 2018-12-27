using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public class Endereco 
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [JsonProperty(PropertyName = "cep")]
        public string Cep { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(4, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(31, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "bairro")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(4, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(128, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "localidade")]
        public string Localidade { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(2, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(2, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "uf")]
        public string Uf { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(3, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(31, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "logradouro")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [MinLength(1, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(7, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "numero")]
        public string Numero { get; set; }

        [MinLength(3, ErrorMessage = ErrorBase.erro_min)]
        [MaxLength(127, ErrorMessage = ErrorBase.erro_max)]
        [JsonProperty(PropertyName = "complemento")]
        public string Complemento { get; set; }
    }
}
