using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class Termo
    {
        [Key]
        public int CodTermo { get; set; }

        public int CodVideo { get; set; }
        
        public String Descricao { get; set; }
        
        public String Explicacao { get; set; }

        public Boolean Ativo { get; set; }        

    }
}
