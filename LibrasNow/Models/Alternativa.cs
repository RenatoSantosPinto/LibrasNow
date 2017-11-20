using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class Alternativa
    {
        [Key]
        public int CodAlternativa { get; set; }

        public int CodExercicio { get; set; }        

        [Required]
        [StringLength(40, ErrorMessage = "O campo Descrição deve ter no máximo 40 caracteres!")]
        public String Descricao { get; set; }
        
        public Boolean Ativo { get; set; }
    }
}
