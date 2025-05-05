using ProjetoPW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoPW.Controllers
{
    public class ChecklistController : Controller
    {
        // GET: Checklist
        public ActionResult Index() => RedirectToAction("Listar");

        // Listar todos os Checklists
        public ActionResult Listar()
        {
            Checklist.GerarLista(Session);
            double porcentagem = Checklist.Porcentagem(Session);
            return View(Session["ListaChecklist"] as List<Checklist>);
        }

        // Criar um novo Checklist
        public ActionResult Create() => View(new Checklist());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Checklist Checklist)
        {
            if (ModelState.IsValid)
            {
                Checklist.Adicionar(Session);
                return RedirectToAction("Listar");
            }
            return View(Checklist);
        }

        // Editar um Checklist
        public ActionResult Edit(int id)
        {
            var checklist = Checklist.Procurar(Session, id);
            if (checklist == null)
                return HttpNotFound();

            return View(checklist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Checklist Checklist)
        {
            if (ModelState.IsValid)
            {
                Checklist.Editar(Session, id);
                return RedirectToAction("Listar");
            }
            return View(Checklist);
        }

        // Exclusão de Checklist com Ajax
        [HttpPost]
        public ActionResult DeleteAjax(int id)
        {
            var checklist = Checklist.Procurar(Session, id);
            if (checklist != null)
            {
                checklist.Excluir(Session);
                return Json(new { sucesso = true });
            }
            return new HttpStatusCodeResult(404, "Checklist não encontrado");
        }
    }
}