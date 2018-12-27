using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Athenas.Domain
{
    public abstract class Usuario
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
		
        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [JsonProperty(PropertyName = "nomeCompleto")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = ErrorBase.erro_nec)]
        [DataType(DataType.EmailAddress, ErrorMessage = ErrorBase.erro_for)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = ErrorBase.erro_for)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
    }
}