using System;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class Exercicio
    {
        [Key]
        public int CodExercicio { get; set; }

        public int CodVideo { get; set; }
        
        public String Descricao { get; set; }
                
        public int Resposta { get; set; }

        public Boolean Ativo { get; set; }
        
    }
}
