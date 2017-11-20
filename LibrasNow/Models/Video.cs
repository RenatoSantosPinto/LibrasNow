using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.Models
{
    public class Video
    {
        [Key]
        public int CodVideo { get; set; }

        [Required(ErrorMessage = "O campo Descrição não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "O campo Descrição deve ter no máximo 40 caracteres!")]
        public String Descricao { get; set; }
       
        public byte[] Arquivo { get; set; }
        
        public String Tipo { get; set; }

        public Boolean Ativo { get; set; }
    }
}
