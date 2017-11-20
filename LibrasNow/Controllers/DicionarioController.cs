using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrasNow.Data;
using Microsoft.EntityFrameworkCore;
using LibrasNow.Models;
using LibrasNow.ViewModels.Dicionario;

namespace LibrasNow.Controllers
{
    public class DicionarioController : Controller
    {
        private readonly LibrasNowDb dbContext;

        public DicionarioController(LibrasNowDb context)
        {
            dbContext = context;
        }

        // GET: Dicionario
        public async Task<IActionResult> Index()
        {
            try
            {
                var dicionario = await dbContext.Dicionario.Where(t => t.Ativo == true)
                    .OrderBy(t => t.Descricao).ToListAsync();
                return View(dicionario);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar termos do dicionário!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }
            return View();
        }

        // GET: Dicionario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dicionario/Create
        public ActionResult Create()
        {          
            try
            {                
                TermoViewModel termoVM = new TermoViewModel();                              

                termoVM.Videos = VideoController.GetVideos(dbContext);

                if(termoVM.Videos.Count() == 0)
                {
                    TempData["Mensagem"] = "Não há vídeos cadastrados no momento!";
                    TempData["Sucesso"] = false;
                }
                else
                {
                    return View(termoVM);
                }
                
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar vídeos!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;                
            }

            return RedirectToAction("Index");

        }

        // POST: Dicionario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TermoViewModel termoVM)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Termo termo = new Termo();

                        termo.Descricao = termoVM.Descricao.Trim();

                        Termo existTermo = await dbContext.Dicionario.Where(t => t.Descricao == termo.Descricao
                        && t.Ativo == true).SingleOrDefaultAsync();

                        if (existTermo == null)
                        {
                            termo.Explicacao = termoVM.Explicacao;
                            termo.CodVideo = termoVM.CodVideo;

                            termo.Ativo = true;
                            dbContext.Dicionario.Add(termo);
                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Termo cadastrado no dicionário com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe termo com essa descrição!";
                        }

                    }

                    termoVM.Videos = VideoController.GetVideos(dbContext);

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao cadastrar termo no dicionário!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(termoVM);
        }

        // GET: Dicionario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Termo termo = await dbContext.Dicionario.Where(t => t.CodTermo == id
                && t.Ativo == true).SingleOrDefaultAsync();

                if(termo == null)
                {
                    return NotFound();
                }

                TermoViewModel termoVM = new TermoViewModel();

                termoVM.CodTermo = termo.CodTermo;
                termoVM.Descricao = termo.Descricao;
                termoVM.Explicacao = termo.Explicacao;
                termoVM.CodVideo = termo.CodVideo;
                termoVM.Videos = VideoController.GetVideos(dbContext);
                termoVM.DescVideo = termoVM.Videos.Where(v => v.CodVideo == termoVM.CodVideo)
                    .FirstOrDefault().Descricao;

                return View(termoVM);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao visualizar termo do dicionário!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
        }

        // POST: Dicionario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, TermoViewModel termoVM)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Termo termo = await dbContext.Dicionario.Where(t => t.CodTermo == id.Value
                        && t.Ativo == true).SingleOrDefaultAsync();

                    if (ModelState.IsValid)
                    {
                        termo.Descricao = termoVM.Descricao.Trim();

                        Termo existTermo = await dbContext.Dicionario.Where(t => t.Descricao == termo.Descricao
                        && t.Ativo == true && t.CodTermo != termo.CodTermo).SingleOrDefaultAsync();

                        if (existTermo == null)
                        {
                            termo.CodVideo = termoVM.CodVideo;
                            termo.Explicacao = termoVM.Explicacao;
                            dbContext.Dicionario.Update(termo);
                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Termo alterado no dicionário com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe termo com essa descrição!";
                        }
                    }

                    termoVM.CodTermo = termo.CodTermo;
                    termoVM.CodVideo = termo.CodVideo;
                    termoVM.Videos = VideoController.GetVideos(dbContext);
                    termoVM.DescVideo = termoVM.Videos.Where(v => v.CodVideo == termoVM.CodVideo)
                        .SingleOrDefault().Descricao;
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao alterar termo no dicionário!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(termoVM);
        }

        // GET: Dicionario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Termo termo = await dbContext.Dicionario.Where(t => t.CodTermo == id.Value
                && t.Ativo == true).SingleOrDefaultAsync();

                if(termo == null)
                {
                    return NotFound();
                }

                return View(termo);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao excluir termo do dicionário!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
        }

        // POST: Dicionario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            
            using (await dbContext.Database.BeginTransactionAsync())
            {

                try
                {
                    Termo termo = await dbContext.Dicionario.Where(t => t.CodTermo == id.Value
                    && t.Ativo == true).SingleOrDefaultAsync();

                    if (termo == null)
                    {
                        return NotFound();
                    }

                    termo.Ativo = false;
                    dbContext.Update(termo);
                    await dbContext.SaveChangesAsync();
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.CommitTransaction();
                    }
                    TempData["Mensagem"] = "Termo excluído do dicionário com sucesso!";
                    TempData["Sucesso"] = true;
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao excluir termo do dicionário!";
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