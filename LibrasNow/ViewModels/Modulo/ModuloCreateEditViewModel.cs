using LibrasNow.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibrasNow.ViewModels.Modulo
{
    public class ModuloCreateEditViewModel
    {
        public int CodModulo { get; set; }

        [Required(ErrorMessage = "O campo Título não pode ser deixado em branco!")]
        [StringLength(25, ErrorMessage = "O campo Título deve ter no máximo 25 caracteres!")]
        public String Titulo { get; set; }

        public byte[] Imagem { get; set; }

        [Required(ErrorMessage = "O campo Nível não pode ser deixado em branco!")]
        [RegularExpression("^[1-9]+[0-9]*$", ErrorMessage = "O campo Nível deve conter apenas números " +
            "inteiros maiores que 0!")]
        public int Nivel { get; set; }

        [StringLength(1000, ErrorMessage = "O campo Explicação deve ter no máximo 1000 caracteres!")]
        public String Explicacao { get; set; }
        
        [Required(ErrorMessage = "O campo Quantidade de Exercícios não pode ser deixado em branco!")]
        [RegularExpression("^[1-9]+[0-9]*$", ErrorMessage = "O campo Quantidade de Exercícios deve " +
            "conter apenas números inteiros maiores que 0!")]
        public int QtdeExercicios { get; set; }  
        
        public int[] CodigosExerciciosModulo { get; set; }

        public IEnumerable<LibrasNow.Models.Exercicio> ExerciciosDisponiveis { get; set; }
    }
}
