using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using LibrasNow.Data;
using LibrasNow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Internal;

namespace LibrasNow.Controllers
{
    public class VideoController : Controller
    {
        private readonly LibrasNowDb dbContext;

        public VideoController(LibrasNowDb context)
        {
            dbContext = context;
        }

        

        public async Task<IActionResult> Index()
        {
            try
            {
                var videos = await dbContext.Videos.Where(v => v.Ativo == true)
                .OrderBy(v => v.Descricao).ToListAsync();

                return View(videos);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao buscar os vídeos no sistema!";
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
        public async Task<IActionResult> Create(Video video, IFormFile file)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        video.Descricao = video.Descricao.Trim();

                        Video existVideo = await dbContext.Videos.Where(v => v.Descricao == video.Descricao
                        && v.Ativo == true).SingleOrDefaultAsync();

                        if (existVideo == null)
                        {
                            if (file != null)
                            {

                                if (file.ContentType.ToLower().StartsWith("video/"))
                                {
                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        await file.CopyToAsync(stream);
                                        video.Arquivo = stream.ToArray();
                                    }

                                    video.Descricao = video.Descricao.Trim();
                                    video.Tipo = file.ContentType;
                                    video.Ativo = true;
                                    dbContext.Add(video);
                                    await dbContext.SaveChangesAsync();
                                    if (dbContext.Database.CurrentTransaction != null)
                                    {
                                        dbContext.Database.CommitTransaction();
                                    }
                                    TempData["Mensagem"] = "Vídeo cadastrado com sucesso!";
                                    TempData["Sucesso"] = true;
                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    TempData["Mensagem"] = "Arquivo enviado não é um vídeo!";
                                }
                            }
                            else
                            {
                                TempData["Mensagem"] = "Nenhum vídeo foi escolhido para envio!";
                            }


                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe um vídeo com essa descrição!";
                        }
                    }

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao cadastrar vídeo!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(video);
        }

        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            try
            {
                Video video = await dbContext.Videos.Where(v => v.CodVideo == id
                && v.Ativo == true).SingleOrDefaultAsync();

                if (video == null)
                {
                    return NotFound();
                }

                return View(video);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao visualizar vídeo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");
            
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Video editedVideo, IFormFile file)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Video oldVideo = await dbContext.Videos.Where(v => v.CodVideo == id.Value
                        && v.Ativo == true).SingleOrDefaultAsync();

                    if (ModelState.IsValid)
                    { 
                        editedVideo.Descricao = editedVideo.Descricao.Trim();

                        Video existVideo = await dbContext.Videos.Where(v => v.Descricao == editedVideo.Descricao
                        && v.CodVideo != oldVideo.CodVideo && v.Ativo == true).SingleOrDefaultAsync();

                        if (existVideo == null)
                        {
                            if (file != null)
                            {

                                if (file.ContentType.ToLower().StartsWith("video/"))
                                {
                                    using (MemoryStream stream = new MemoryStream())
                                    {
                                        await file.CopyToAsync(stream);
                                        oldVideo.Arquivo = stream.ToArray();
                                    }

                                    oldVideo.Tipo = file.ContentType;
                                }
                                else
                                {
                                    TempData["Mensagem"] = "Arquivo enviado não é um vídeo!";
                                    editedVideo.CodVideo = oldVideo.CodVideo;
                                    editedVideo.Arquivo = oldVideo.Arquivo;
                                    return View(editedVideo);
                                }
                            }

                            oldVideo.Descricao = editedVideo.Descricao;
                            dbContext.Update(oldVideo);
                            await dbContext.SaveChangesAsync();
                            if (dbContext.Database.CurrentTransaction != null)
                            {
                                dbContext.Database.CommitTransaction();
                            }
                            TempData["Mensagem"] = "Vídeo alterado com sucesso!";
                            TempData["Sucesso"] = true;
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            TempData["Mensagem"] = "Já existe um vídeo com essa descrição!";
                        }

                    }

                    editedVideo.CodVideo = oldVideo.CodVideo;
                    editedVideo.Arquivo = oldVideo.Arquivo;

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao alterar vídeo!";
                    TempData["Exception"] = ex;
                    TempData["Sucesso"] = false;
                    if (dbContext.Database.CurrentTransaction != null)
                    {
                        dbContext.Database.RollbackTransaction();
                    }
                    return RedirectToAction("Index");
                }
            }
            
            return View(editedVideo);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            try
            {
                Video video = await dbContext.Videos.Where(v => v.CodVideo == id.Value
                && v.Ativo == true).SingleOrDefaultAsync();
                if(video == null)
                {
                    return NotFound();
                }

                return View(video);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao excluir vídeo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return RedirectToAction("Index");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            using (await dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    Video video = await dbContext.Videos.Where(v => v.CodVideo == id
                    && v.Ativo == true).SingleOrDefaultAsync();
                    Exercicio exercicio = await dbContext.Exercicios.Where(e => e.CodVideo == video.CodVideo
                    && e.Ativo == true).FirstOrDefaultAsync();
                    Termo termo = await dbContext.Dicionario.Where(t => t.CodVideo == video.CodVideo
                    && t.Ativo == true).FirstOrDefaultAsync();

                    if (video == null)
                    {
                        return NotFound();
                    }
                    else if (exercicio != null)
                    {
                        TempData["Mensagem"] = "Vídeo não pode ser excluído porque está sendo usado por algum exercício!";
                        return View(video);
                    }
                    else if (termo != null)
                    {
                        TempData["Mensagem"] = "Vídeo não pode ser excluído porque está sendo usado por algum termo!";
                        return View(video);
                    }
                    else
                    {
                        video.Ativo = false;
                        dbContext.Update(video);
                        await dbContext.SaveChangesAsync();
                        if (dbContext.Database.CurrentTransaction != null)
                        {
                            dbContext.Database.CommitTransaction();
                        }
                        TempData["Mensagem"] = "Vídeo excluído com sucesso!";
                        TempData["Sucesso"] = true;
                    }

                }
                catch (Exception ex)
                {
                    TempData["Mensagem"] = "Erro ao excluir vídeo!";
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

        [HttpGet]
        public FileStreamResult ShowVideo(int id)
        {
            try
            {
                Video video = dbContext.Videos.Where(v => v.CodVideo == id
                && v.Ativo == true).OrderBy(v => v.Descricao).SingleOrDefault();

                MemoryStream ms = new MemoryStream(video.Arquivo);

                return new FileStreamResult(ms, video.Tipo);
            }
            catch(Exception ex)
            {
                TempData["Mensagem"] = "Erro ao exibir vídeo!";
                TempData["Exception"] = ex;
                TempData["Sucesso"] = false;
            }

            return null;
             
        }

        [HttpGet]
        static public IEnumerable<Video> GetVideos(LibrasNowDb dbContext)
        {
            try
            {
                IEnumerable<Video> videos = dbContext.Videos.Where(v => v.Ativo == true)
                    .OrderBy(v => v.Descricao).ToList();

                return videos;
            }
            catch(Exception ex)
            {
                throw ex;
            }            
        }
    }
}