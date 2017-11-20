using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibrasNow.Models;
using System.ComponentModel.DataAnnotations;

namespace LibrasNow.ViewModels.Dicionario
{
    public class TermoViewModel
    {    
        public int CodTermo{ get; set; }   

        public int CodVideo{ get; set; }

        public String DescVideo{ get; set; }

        [Required(ErrorMessage = "O campo Descrição não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "O campo Descrição deve ter no máximo 40 caracteres!")]
        public String Descricao { get; set; }

        [StringLength(1000, ErrorMessage = "O campo Explicação deve ter no máximo 1000 caracteres!")]
        public String Explicacao { get; set; }

        public IEnumerable<Video> Videos;
              
    }
}
