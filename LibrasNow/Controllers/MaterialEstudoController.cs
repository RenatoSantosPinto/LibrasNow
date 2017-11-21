using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LibrasNow.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LibrasNow.Models;

namespace LibrasNow.Controllers
{
    public class MaterialEstudoController : Controller
    {

        private readonly LibrasNowDb dbContext;

        public MaterialEstudoController(LibrasNowDb context)
        {
            dbContext = context;
        }
        
        public async Task<IActionResult> Index()
        {
            try
            {
                var materiaisEstudo = await dbContext.MateriaisEstudo.Where(me => me.Ativo == true)
                .OrderBy(me => me.Titulo).ToListAsync();

                return View(materiaisEstudo);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar os materiais de estudo no sistema!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return View();
        }
        
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MaterialEstudo matEst)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    if (ModelState.IsValid)
                    {
                        var existMatEst = await dbContext.MateriaisEstudo.Where(me => me.Descricao ==
                        matEst.Descricao).SingleOrDefaultAsync();

                        if(existMatEst == null)
                        {
                            matEst.Ativo = true;
                            dbContext.Add(matEst);
                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Material de estudo cadastrado com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe material de estudo com esse título!";
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao cadastrar material de estudo!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }


            return View(matEst);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {          
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                MaterialEstudo matEst = await dbContext.MateriaisEstudo.Where(me => me.CodMatEst == id
                && me.Ativo == true).SingleOrDefaultAsync();

                if (matEst == null)
                {
                    return NotFound();
                }

                return View(matEst);

            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao visualizar material de estudo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, MaterialEstudo matEst)
        {
            if(id == null)
            {
                return NotFound();
            }

            using (await dbContext.Database.BeginTransactionAsync())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        var existMatEst = await dbContext.MateriaisEstudo.Where(me => me.Descricao ==
                        matEst.Descricao && me.CodMatEst != id.Value && me.Ativo == true).SingleOrDefaultAsync();

                        if (existMatEst == null)
                        {
                            matEst.CodMatEst = id.Value;
                            matEst.Ativo = true;

                            dbContext.Update(matEst);
                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Material de estudo alterado com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe material de estudo com esse título!";
                        }

                        
                    }

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao alterar material de estudo!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(matEst);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            MaterialEstudo matEst = null;

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                matEst = await dbContext.MateriaisEstudo.Where(me => me.CodMatEst == id
                && me.Ativo == true).SingleOrDefaultAsync();

                if (matEst == null)
                {
                    return NotFound();
                }

                return View(matEst);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao excluir material de estudo!";
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
                    MaterialEstudo matEst = await dbContext.MateriaisEstudo.Where(me => me.CodMatEst == id
                    && me.Ativo == true).SingleOrDefaultAsync();

                    if (matEst == null)
                    {
                        return NotFound();
                    }

                    matEst.Ativo = false;
                    dbContext.Update(matEst);
                    await dbContext.SaveChangesAsync();
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.CommitTransaction();
                    }
                    TempData["Mensagem"] = "Material de estudo excluído com sucesso!";
                    TempData["Sucesso"] = true;
                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao excluir material de estudo!";
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