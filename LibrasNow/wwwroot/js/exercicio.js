var exerciciosModuloList;
var exerciciosModuloArray;

window.onload = iniciaExerciciosModulo();

function iniciaExerciciosModulo()
{
    exerciciosModuloList = document.getElementById("exerciciosModuloList");
    exerciciosModuloArray = new Array();
    
    var checkboxes = document.getElementsByName("CodigosExerciciosModulo");

    var i;
    for (i = 0; i < checkboxes.length; i++)
    {
        var checkbox = checkboxes[i];

        if (checkbox.checked == true)
        {
            exerciciosModuloArray.push(checkbox.id);
        }
    }

    insereExerciciosModulo();
}

function adicionaExercicioModulo(id)
{
    alteraListaExerciciosModulo();

    var checkbox = document.getElementById(id);
    if (checkbox.checked == true)
    {
        exerciciosModuloArray.push(checkbox.id);
    }
    else
    {
        exerciciosModuloArray.splice(exerciciosModuloArray.indexOf(checkbox.id), 1);
    }

    insereExerciciosModulo();
}

function insereExerciciosModulo()
{
    exerciciosModuloArray.sort();
    
    var i;
    for (i = 0; i < exerciciosModuloArray.length; i++)
    {
        var exercicio = document.createElement("li");
        exercicio.innerHTML = exerciciosModuloArray[i];
        exerciciosModuloList.appendChild(exercicio);
    }
}

function alteraListaExerciciosModulo()
{
    while (exerciciosModuloList.firstChild)
    {
        exerciciosModuloList.removeChild(exerciciosModuloList.firstChild);
    }
}

