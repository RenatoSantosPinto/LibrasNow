using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class MaterialEstudo
    {
        [Key]
        public int CodMatEst { get; set; }

        [Required(ErrorMessage = "O campo Título não pode ser deixado em branco!")]
        [StringLength(100, ErrorMessage = "O campo Título deve ter no máximo 100 caracteres!")]
        public String Titulo { get; set; }

        [Required(ErrorMessage = "O campo Descrição não pode ser deixado em branco!")]
        [StringLength(1000, ErrorMessage = "O campo Descrição deve ter no máximo 1000 caracteres!")]
        public String Descricao { get; set; }

        [StringLength(100, ErrorMessage = "O campo Link deve ter no máximo 100 caracteres!")]
        public String Link { get; set; }

        public Boolean Ativo { get; set; }
        
        
    }
}
