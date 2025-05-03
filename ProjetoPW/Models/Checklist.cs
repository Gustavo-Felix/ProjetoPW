using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoPW.Models
{
    public class Checklist
    {
        public int id { get; set; }
        public string nomeTarefa { get; set; }
        [Required(ErrorMessage = "É necessário dar um nome a tarefa!")]
        public string descricao { get; set; }
        [Required(ErrorMessage = "É necessário colocar uma descrição!")]
        public bool concluido { get; set; }
        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Nascimento")]
        public DateTime dataCadastro { get; set; }

        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaChecklist"] != null && ((List<Checklist>)session["ListaChecklist"]).Count > 0)
            {
                return;
            }
            var lista = new List<Checklist>
            {
                new Checklist { id = 0, nomeTarefa = "Teste", descricao = "Esta é uma tarefa exemplo!", concluido = false, dataCadastro = new DateTime(2025, 01, 01) },
            };
            session["ListaChecklist"] = lista;
        }
        public void Adicionar(HttpSessionStateBase session)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;
            if (lista == null)
            {
                lista = new List<Checklist>();
                session["ListaChecklist"] = lista;
            }

            this.id = lista.Count > 0 ? lista.Max(a => a.id) + 1 : 0;
            lista.Add(this);
        }

        public static Checklist Procurar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;
            return lista?.FirstOrDefault(a => a.id == id);
        }

        public void Editar(HttpSessionStateBase session, int id)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;
            var original = lista?.FirstOrDefault(a => a.id == id);

            if (original != null)
            {
                original.nomeTarefa = this.nomeTarefa;
                original.descricao = this.descricao;
                original.concluido = this.concluido;
                original.dataCadastro = this.dataCadastro;
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;
            lista?.RemoveAll(a => a.id == this.id);
        }
    }
}