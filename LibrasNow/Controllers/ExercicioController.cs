using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibrasNow.Data;
using Microsoft.EntityFrameworkCore;
using LibrasNow.ViewModels.Exercicio;
using LibrasNow.Models;

namespace LibrasNow.Controllers
{
    public class ExercicioController : Controller
    {
        private readonly LibrasNowDb dbContext;

        public ExercicioController(LibrasNowDb context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var exercicios = await dbContext.Exercicios.Where(e => e.Ativo == true)
                    .OrderBy(e => e.Descricao).ToListAsync();
                return View(exercicios);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar exercícios!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }
            return View();
        }

        public IActionResult Create()
        {
            try
            {
                ExercicioViewModel evm = new ExercicioViewModel();
                evm.Videos = VideoController.GetVideos(dbContext);

                if (evm.Videos.Count() == 0)
                {
                    TempData["Mensagem"] = "Não há vídeos cadastrados no momento!";
                    TempData["Sucesso"] = false;
                }
                else
                {   
                    return View(evm);
                }
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao cadastrar exercício!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;                
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExercicioViewModel evm)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if(ModelState.IsValid)
                    {
                        evm.Descricao = evm.Descricao.Trim();

                        Exercicio e = await dbContext.Exercicios.Where(ex => ex.Descricao == evm.Descricao
                        && ex.Ativo == true).SingleOrDefaultAsync();

                        if (e == null)
                        {
                            e = new Exercicio();
                            e.Descricao = evm.Descricao;
                            e.CodVideo = evm.CodVideo;
                            e.Resposta = evm.Resposta;
                            e.Ativo = true;
                            dbContext.Exercicios.Add(e);

                            await dbContext.SaveChangesAsync();


                            Alternativa a1 = new Alternativa();
                            a1.Descricao = evm.Alternativa1;
                            a1.CodAlternativa = 1;
                            a1.CodExercicio = e.CodExercicio;
                            a1.Ativo = true;
                            dbContext.Alternativas.Add(a1);

                            Alternativa a2 = new Alternativa();
                            a2.Descricao = evm.Alternativa2;
                            a2.CodAlternativa = 2;
                            a2.CodExercicio = e.CodExercicio;
                            a2.Ativo = true;                            
                            dbContext.Alternativas.Add(a2);

                            Alternativa a3 = new Alternativa();
                            a3.Descricao = evm.Alternativa3;
                            a3.CodAlternativa = 3;
                            a3.CodExercicio = e.CodExercicio;
                            a3.Ativo = true;
                            dbContext.Alternativas.Add(a3);

                            Alternativa a4 = new Alternativa();
                            a4.Descricao = evm.Alternativa4;
                            a4.CodAlternativa = 4;
                            a4.CodExercicio = e.CodExercicio;
                            a4.Ativo = true;
                            dbContext.Alternativas.Add(a4);


                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Exercício cadastrado com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe exercício com essa descrição!";
                        }
                    }

                    evm.Videos = VideoController.GetVideos(dbContext);

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao cadastrar exercício!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(evm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Exercicio exercicio = await dbContext.Exercicios.Where(e => e.CodExercicio == id.Value
                 && e.Ativo == true).SingleOrDefaultAsync();

                if(exercicio == null)
                {
                    return NotFound();
                }

                var alternativas = await dbContext.Alternativas.Where(a => a.CodExercicio ==
                exercicio.CodExercicio && a.Ativo == true).ToListAsync();

                if(alternativas.Count == 0)
                {
                    return NotFound();
                }                

                ExercicioViewModel evm = new ExercicioViewModel();

                evm.Videos = VideoController.GetVideos(dbContext);

                if(evm.Videos.Count() == 0)
                {
                    return NotFound();
                }

                evm.CodExercicio = exercicio.CodExercicio;
                evm.CodVideo = exercicio.CodVideo;                
                evm.DescVideo = evm.Videos.Where(v => v.CodVideo == evm.CodVideo).SingleOrDefault().Descricao;                
                evm.Descricao = exercicio.Descricao;                
                evm.Resposta = exercicio.Resposta;

                evm.Alternativa1 = alternativas.Where(a => a.CodAlternativa == 1).SingleOrDefault().Descricao;
                evm.Alternativa2 = alternativas.Where(a => a.CodAlternativa == 2).SingleOrDefault().Descricao;
                evm.Alternativa3 = alternativas.Where(a => a.CodAlternativa == 3).SingleOrDefault().Descricao;
                evm.Alternativa4 = alternativas.Where(a => a.CodAlternativa == 4).SingleOrDefault().Descricao;

                return View(evm);
                
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao alterar exercício!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int?id, ExercicioViewModel evm)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Exercicio ex = await dbContext.Exercicios.Where(e => e.CodExercicio == id.Value
                            && e.Ativo == true).SingleOrDefaultAsync();

                    if(ex == null)
                    {
                        return NotFound();
                    }

                    if (ModelState.IsValid)
                    {
                        ex.Descricao = evm.Descricao.Trim();

                        Exercicio existExercicio = await dbContext.Exercicios.Where(e => e.Descricao == ex.Descricao
                        && e.CodExercicio != id.Value && e.Ativo == true).SingleOrDefaultAsync();

                        if (existExercicio == null)
                        {
                            ex.CodVideo = evm.CodVideo;
                            ex.Resposta = evm.Resposta;
                            dbContext.Exercicios.Update(ex);

                            Alternativa a1 = await dbContext.Alternativas.Where(a => a.CodExercicio
                            == ex.CodExercicio && a.CodAlternativa == 1 && a.Ativo == true)
                            .SingleOrDefaultAsync();
                            a1.Descricao = evm.Alternativa1;
                            dbContext.Alternativas.Update(a1);

                            Alternativa a2 = await dbContext.Alternativas.Where(a => a.CodExercicio
                            == ex.CodExercicio && a.CodAlternativa == 2 && a.Ativo == true)
                            .SingleOrDefaultAsync();
                            a2.Descricao = evm.Alternativa2;
                            dbContext.Alternativas.Update(a2);

                            Alternativa a3 = await dbContext.Alternativas.Where(a => a.CodExercicio
                            == ex.CodExercicio && a.CodAlternativa == 3 && a.Ativo == true)
                            .SingleOrDefaultAsync();
                            a3.Descricao = evm.Alternativa3;
                            dbContext.Alternativas.Update(a3);

                            Alternativa a4 = await dbContext.Alternativas.Where(a => a.CodExercicio
                            == ex.CodExercicio && a.CodAlternativa == 4 && a.Ativo == true)
                            .SingleOrDefaultAsync();
                            a4.Descricao = evm.Alternativa4;
                            dbContext.Alternativas.Update(a4);
                            

                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Exercício alterado com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe exercício com essa descrição!";
                        }
                    }

                    evm.CodExercicio = ex.CodExercicio;
                    evm.CodVideo = ex.CodVideo;
                    evm.Videos = VideoController.GetVideos(dbContext);
                    evm.DescVideo = evm.Videos.Where(v => v.CodVideo == evm.CodVideo)
                        .SingleOrDefault().Descricao;

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao alterar exercício!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(evm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Exercicio exerc = await dbContext.Exercicios.Where(e => e.CodExercicio == id.Value
                && e.Ativo == true).SingleOrDefaultAsync();

                if (exerc == null)
                {
                    return NotFound();
                }

                var alternativas = await dbContext.Alternativas.Where(a => a.CodExercicio ==
                exerc.CodExercicio && a.Ativo == true).ToListAsync();

                if (alternativas.Count == 0)
                {
                    return NotFound();
                }

                ExercicioViewModel evm = new ExercicioViewModel();

                evm.CodExercicio = exerc.CodExercicio;
                evm.CodVideo = exerc.CodVideo;
                evm.Descricao = exerc.Descricao;
                evm.DescVideo = VideoController.GetVideos(dbContext).Where(v => v.CodVideo == evm.CodVideo)
                    .SingleOrDefault().Descricao;
                evm.Resposta = exerc.Resposta;

                evm.Alternativa1 = alternativas.Where(a => a.CodAlternativa == 1).SingleOrDefault().Descricao;
                evm.Alternativa2 = alternativas.Where(a => a.CodAlternativa == 2).SingleOrDefault().Descricao;
                evm.Alternativa3 = alternativas.Where(a => a.CodAlternativa == 3).SingleOrDefault().Descricao;
                evm.Alternativa4 = alternativas.Where(a => a.CodAlternativa == 4).SingleOrDefault().Descricao;
                
                return View(evm);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao excluir exercício!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            using (await dbContext.Database.BeginTransactionAsync())
            {

                try
                {
                    Exercicio exerc = await dbContext.Exercicios.Where(e => e.CodExercicio == id.Value
                    && e.Ativo == true).SingleOrDefaultAsync();

                    if (exerc == null)
                    {
                        return NotFound();
                    }

                    var alternativas = await dbContext.Alternativas.Where(a => a.Ativo == true)
                        .ToListAsync();

                    if(alternativas.Count == 0)
                    {
                        return NotFound();
                    }

                    ExercicioModulo exMod = await dbContext.ExerciciosModulo.Where(em => em.CodExercicio 
                    == exerc.CodExercicio && em.Ativo == true).FirstOrDefaultAsync();

                    if(exMod == null)
                    {
                        exerc.Ativo = false;
                        dbContext.Update(exerc);

                        foreach(Alternativa a in alternativas)
                        {
                            a.Ativo = false;
                            dbContext.Update(a);
                        }

                        await dbContext.SaveChangesAsync();
                        if (dbContext.Database.CurrentTransaction != null)
                                    {
                                        dbContext.Database.CommitTransaction();
                                    }
                        TempData["Mensagem"] = "Exercício excluído com sucesso!";
                        TempData["Sucesso"] = true;
                    }
                    else
                    {
                        TempData["Mensagem"] = "Não foi possível excluir o exercício porque há um módulo que o " +
                            "está utilizando!";
                        TempData["Sucesso"] = false;
                    }
                    
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao excluir exercício!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                }

            }

            return RedirectToAction("Index");
        }
    }
}