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

        public ActionResult Grafico(bool somenteConcluidas = false)
        {
            Checklist.GerarLista(Session); // Garante que a lista esteja carregada
            var lista = Session["ListaChecklist"] as List<Checklist>;

            if (somenteConcluidas)
            {
                lista = lista.Where(t => t.concluido).ToList();
            }

            var hoje = DateTime.Now;

            var dados = lista
                .Select(t => new
                {
                    Nome = t.nome,
                    Prazo = t.prazo.ToString("dd/MM/yyyy"),
                    DiasRestantes = (t.prazo - hoje).Days < 0 ? 0 : (t.prazo - hoje).Days,
                    Categoria = t.categoria
                })
                .ToList();

            var nomes = dados.Select(d => d.Nome).ToArray();
            var prazos = dados.Select(d => d.Prazo).ToArray();
            var diasRestantes = dados.Select(d => d.DiasRestantes).ToArray();
            var categorias = dados.Select(d => d.Categoria).ToArray();

            ViewBag.Nomes = Newtonsoft.Json.JsonConvert.SerializeObject(nomes);
            ViewBag.Prazos = Newtonsoft.Json.JsonConvert.SerializeObject(prazos);
            ViewBag.DiasRestantes = Newtonsoft.Json.JsonConvert.SerializeObject(diasRestantes);
            ViewBag.Categorias = Newtonsoft.Json.JsonConvert.SerializeObject(categorias);


            return View();
        }


        // Listar todos os Checklists
        public ActionResult Listar()
        {
            Checklist.GerarLista(Session); // Garante que a lista esteja carregada
            var lista = Session["ListaChecklist"] as List<Checklist>;

            double porcentagem = Checklist.Porcentagem(Session);
            ViewBag.Porcentagem = porcentagem;

            return View(lista);
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