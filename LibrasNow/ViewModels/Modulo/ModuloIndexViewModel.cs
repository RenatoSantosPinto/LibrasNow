using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.ViewModels.Modulo
{
    public class ModuloIndexViewModel
    {
        public int CodModulo { get; set; }
        
        public String Titulo { get; set; }

        public byte[] Imagem { get; set; }
        
        public int Nivel { get; set; }

        public int Porcentagem { get; set; }
    }
}
