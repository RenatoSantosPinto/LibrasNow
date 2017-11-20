using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibrasNow.Models
{
    public class ModuloResolvido
    {
        [Key]
        public int CodModuloResolvido { get; set; }

        public int CodModulo { get; set; }
        
        public int CodUsuario { get; set; }
        
        public int Porcentagem { get; set; }

        public Boolean Ativo { get; set; }
    }
}
