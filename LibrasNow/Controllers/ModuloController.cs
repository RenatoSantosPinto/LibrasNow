using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrasNow.Models;
using LibrasNow.Data;
using Microsoft.EntityFrameworkCore;
using LibrasNow.ViewModels.Modulo;
using System.Collections;

namespace LibrasNow.Controllers
{
    public class ModuloController : Controller
    {
        private readonly LibrasNowDb dbContext;

        public ModuloController(LibrasNowDb context)
        {
            dbContext = context;
        }

        // GET: Modulo
        public async Task <IActionResult> Index()
        {
            try
            {
                    var modulos = await dbContext.Modulos.Where(m => m.Ativo == true)
                    .OrderBy(m => m.Nivel).ToListAsync();

                    List<ModuloIndexViewModel> moduloIndexViewModels = new List<ModuloIndexViewModel>();

                    foreach (var m in modulos)
                    {
                        ModuloIndexViewModel mivm = new ModuloIndexViewModel();

                        mivm.CodModulo = m.CodModulo;
                        mivm.Imagem = m.Imagem;
                        mivm.Nivel = m.Nivel;
                        mivm.Titulo = m.Titulo;

                        moduloIndexViewModels.Add(mivm);
                    }

                    return View(moduloIndexViewModels);
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar módulos!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }
            return View();            
        }

        // GET: Modulo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Modulo/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var exercicios = await dbContext.Exercicios.Where(e => e.Ativo == true)
                    .OrderBy(e => e.Descricao).ToListAsync();

                if(exercicios.Count() != 0)
                {
                    ModuloCreateEditViewModel mcevm = new ModuloCreateEditViewModel();

                    mcevm.CodigosExerciciosModulo = new int[0];
                    mcevm.ExerciciosDisponiveis = exercicios;
                    
                    return View(mcevm);
                }
                else
                {
                    TempData["Mensagem"] = "Não há exercícios cadastrados no momento!";
                    TempData["Sucesso"] = false;
                }
                
            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao cadastrar módulo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
        }

        // POST: Modulo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModuloCreateEditViewModel mcevm, int[] CodigosExerciciosModulo)
        {
            try 
            {
                using (await dbContext.Database.BeginTransactionAsync())
                {
                    if (ModelState.IsValid)
                    {
                        mcevm.Titulo = mcevm.Titulo.Trim();

                        Modulo modulo = await dbContext.Modulos.Where(m => m.Titulo == mcevm.Titulo 
                        && m.Ativo == true).SingleOrDefaultAsync();

                        if(modulo == null)
                        {
                            IEnumerable<Modulo> modulosNivel = await dbContext.Modulos
                                .Where(m => m.Nivel == mcevm.Nivel).ToListAsync();

                            if (modulosNivel.Count() < 2)
                            {
                                if (CodigosExerciciosModulo.Count() > 0)
                                {
                                    modulo = new Modulo();
                                    modulo.Titulo = mcevm.Titulo;
                                    modulo.Explicacao = mcevm.Explicacao;
                                    modulo.QtdeExercicios = mcevm.QtdeExercicios;
                                    modulo.Nivel = mcevm.Nivel;
                                    modulo.Ativo = true;

                                    dbContext.Modulos.Add(modulo);
                                    await dbContext.SaveChangesAsync();

                                    foreach (var codigo in CodigosExerciciosModulo)
                                    {
                                        ExercicioModulo em = new ExercicioModulo();
                                        em.CodExercicio = codigo;
                                        em.CodModulo = modulo.CodModulo;
                                        em.Ativo = true;

                                        dbContext.ExerciciosModulo.Add(em);
                                    }

                                    await dbContext.SaveChangesAsync();

                                    if (dbContext.Database.CurrentTransaction != null)
                                    {
                                        dbContext.Database.CommitTransaction();
                                    }
                                    TempData["Mensagem"] = "Módulo cadastrado com sucesso!";
                                    TempData["Sucesso"] = true;
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Mensagem"] = "Pelo menos um exercício deve ser selecionado!";
                                }
                                
                            }
                            else
                            {
                                TempData["Mensagem"] = "Limite de módulos nesse nível já foi atingido!";
                            }
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe módulo com esse título!";
                        }
                        
                    }
                }

                mcevm.CodigosExerciciosModulo = CodigosExerciciosModulo;
                mcevm.ExerciciosDisponiveis = await dbContext.Exercicios.Where(e => e.Ativo == true)
                    .OrderBy(e => e.Descricao).ToListAsync();

            }
            catch(Exception ex)
            {
                if(dbContext.Database.CurrentTransaction != null)
                {
                    dbContext.Database.RollbackTransaction();
                }
                
                TempData["Mensagem"] = "Erro ao cadastrar módulo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
                return RedirectToAction("Index");
            }

            return View(mcevm);
        }

        // GET: Modulo/Create
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Modulo modulo = await dbContext.Modulos.Where(m => m.CodModulo == id.Value &&
                m.Ativo == true).SingleOrDefaultAsync();

                if(modulo == null)
                {
                    return NotFound();
                }

                var exerciciosDisponiveis = await dbContext.Exercicios.Where(e => e.Ativo == true)
                    .OrderBy(e => e.Descricao).ToListAsync();

                var exerciciosModulo = await dbContext.ExerciciosModulo.Where(em => 
                em.CodModulo == modulo.CodModulo && em.Ativo == true)
                .OrderBy(em => em.CodExercicio).ToListAsync();

                ModuloCreateEditViewModel mcevm = new ModuloCreateEditViewModel();

                mcevm.CodModulo = modulo.CodModulo;
                mcevm.Titulo = modulo.Titulo;
                mcevm.Nivel = modulo.Nivel;
                mcevm.Explicacao = modulo.Explicacao;
                mcevm.QtdeExercicios = modulo.QtdeExercicios;
                mcevm.ExerciciosDisponiveis = exerciciosDisponiveis;
                mcevm.CodigosExerciciosModulo = new int[exerciciosModulo.Count];

                int i;
                for(i = 0; i < exerciciosModulo.Count; i++)
                {
                    var em = exerciciosModulo.ElementAt(i);
                    mcevm.CodigosExerciciosModulo[i] = em.CodExercicio;
                }

                return View(mcevm);                

            }
            catch (Exception ex)
            {
                TempData["Mensagem"] = "Erro ao visualizar módulo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
        }

        // POST: Modulo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ModuloCreateEditViewModel mcevm, int[] CodigosExerciciosModulo)
        {
            try
            {
                using (await dbContext.Database.BeginTransactionAsync())
                { 
                    Modulo modulo = await dbContext.Modulos.Where(m => m.CodModulo == id.Value
                        && m.Ativo == true).SingleOrDefaultAsync();
                
                    if (ModelState.IsValid)
                    {
                        mcevm.Titulo = mcevm.Titulo.Trim();

                        Modulo existModulo = await dbContext.Modulos.Where(m => m.Titulo == mcevm.Titulo
                        && m.Ativo == true && m.CodModulo != modulo.CodModulo).SingleOrDefaultAsync();

                        if (existModulo == null)
                        {
                            IEnumerable<Modulo> modulosNivel = await dbContext.Modulos
                                .Where(m => m.Nivel == mcevm.Nivel).ToListAsync();

                            if (modulosNivel.Count() < 2)
                            {
                                if (CodigosExerciciosModulo.Count() > 0)
                                {
                                    modulo.Titulo = mcevm.Titulo;
                                    modulo.Explicacao = mcevm.Explicacao;
                                    modulo.QtdeExercicios = mcevm.QtdeExercicios;
                                    modulo.Nivel = mcevm.Nivel;

                                    dbContext.Modulos.Update(modulo);
                                    await dbContext.SaveChangesAsync();

                                    var exerciciosModulo = await dbContext.ExerciciosModulo
                                        .Where(em => em.CodModulo == mcevm.CodModulo && em.Ativo == true)
                                        .ToListAsync();

                                    foreach(var em in exerciciosModulo)
                                    {
                                        if(!CodigosExerciciosModulo.Contains(em.CodExercicio))
                                        {
                                            dbContext.ExerciciosModulo.Remove(em);                                            
                                        }

                                        await dbContext.SaveChangesAsync();
                                    }


                                    foreach (var codigo in CodigosExerciciosModulo)
                                    {
                                        ExercicioModulo em = exerciciosModulo
                                            .Where(exm => exm.CodExercicio != codigo).SingleOrDefault();

                                        if(em == null)
                                        {
                                            em = new ExercicioModulo();
                                            em.CodExercicio = codigo;
                                            em.CodModulo = modulo.CodModulo;
                                            em.Ativo = true;

                                            dbContext.ExerciciosModulo.Add(em);
                                            await dbContext.SaveChangesAsync();
                                        }

                                    }

                                    if (dbContext.Database.CurrentTransaction != null)
                                    {
                                        dbContext.Database.CommitTransaction();
                                    }
                                    TempData["Mensagem"] = "Módulo alterado com sucesso!";
                                    TempData["Sucesso"] = true;
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Mensagem"] = "Pelo menos um exercício deve ser selecionado!";
                                }

                            }
                            else
                            {
                                TempData["Mensagem"] = "Limite de módulos nesse nível já foi atingido!";
                            }
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe módulo com esse título!";
                        }

                    }
                }

                mcevm.CodigosExerciciosModulo = CodigosExerciciosModulo;
                mcevm.ExerciciosDisponiveis = await dbContext.Exercicios.Where(e => e.Ativo == true)
                    .OrderBy(e => e.Descricao).ToListAsync();

            }
            catch (Exception ex)
            {
                if (dbContext.Database.CurrentTransaction != null)
                {
                    dbContext.Database.RollbackTransaction();
                }

                TempData["Mensagem"] = "Erro ao alterar módulo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
                return RedirectToAction("Index");
            }

            return View(mcevm);
        }

        // GET: Modulo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Modulo/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}