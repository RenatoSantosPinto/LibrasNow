using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class ExercicioModulo
    {
        [Key]
        public int CodExercicioModulo { get; set; }

        public int CodExercicio { get; set; }

        public int CodModulo { get; set; }

        public Boolean Ativo { get; set; }
    }
}
