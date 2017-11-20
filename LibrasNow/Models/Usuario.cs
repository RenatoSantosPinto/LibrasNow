using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LibrasNow.Models
{
    public class Usuario
    {
        [Key]
        public int CodUsuario { get; set; }

        [Required]
        [StringLength(50)]
        public String Nome { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "Email inválido!")]
        public String Email { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        public String Login { get; set; }

        [Required]
        [StringLength(25, MinimumLength = 5)]
        public String Senha { get; set; }

        public Boolean Ativo { get; set; }

        public int Nivel { get; set; }

        public int Tipo { get; set; }
    }
}
