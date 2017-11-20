using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class Modulo
    {
        [Key]
        public int CodModulo { get; set; }
        
        public String Titulo { get; set; }
        
        public byte[] Imagem { get; set; }
        
        public int Nivel { get; set; }
        
        public String Explicacao { get; set; } 
        
        public int QtdeExercicios { get; set; }
        
        public Boolean Ativo { get; set; }

        
    }
}
