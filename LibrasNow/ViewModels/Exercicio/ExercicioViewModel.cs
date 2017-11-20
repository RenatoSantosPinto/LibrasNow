using LibrasNow.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.ViewModels.Exercicio
{
    public class ExercicioViewModel
    {
        public int CodExercicio { get; set; }
        
        public int CodVideo { get; set; }

        public String DescVideo { get; set; }

        [Required(ErrorMessage = "O campo Descrição não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "O campo Descrição deve ter no máximo 40 caracteres!")]
        public String Descricao { get; set; }

        [Required]
        public int Resposta { get; set; }

        public IEnumerable<Video> Videos;

        [Required(ErrorMessage = "O campo Alternativa 1 não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "As alternativas devem ter no máximo 40 caracteres!")]
        public String Alternativa1 { get; set; }

        [Required(ErrorMessage = "O campo Alternativa 2 não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "As alternativas devem ter no máximo 40 caracteres!")]
        public String Alternativa2 { get; set; }

        [Required(ErrorMessage = "O campo Alternativa 3 não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "As alternativas devem ter no máximo 40 caracteres!")]
        public String Alternativa3 { get; set; }

        [Required(ErrorMessage = "O campo Alternativa 4 não pode ser deixado em branco!")]
        [StringLength(40, ErrorMessage = "As alternativas devem ter no máximo 40 caracteres!")]
        public String Alternativa4 { get; set; }
    }
}
