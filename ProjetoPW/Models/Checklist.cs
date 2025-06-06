﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetoPW.Models
{
    public class Checklist
    {
        public int id { get; set; } //id 

        [Required(ErrorMessage = "É necessário dar um nome a tarefa!")]
        public string nome { get; set; } //nome tarefa
        [Required(ErrorMessage = "É necessário colocar uma descrição!")]
        public string descricao { get; set; } //descricao da tarefa/meta
        
        
        [DataType(DataType.Date)]
        [Display(Name = "Data de cadastro")]
        public DateTime dataCadastro { get; set; } //data de cadastro

        [Required(ErrorMessage = "A data do prazo é obrigatória!")]
        [DataType(DataType.Date)]
        [Display(Name = "Data de Prazo")]
        public DateTime prazo { get; set; } //prazo
        public bool concluido { get; set; } //se concluido ou nao
        public string categoria { get; set; } // Prazo vencido, em andamento ou concluido.


        public static void GerarLista(HttpSessionStateBase session)
        {
            if (session["ListaChecklist"] != null && ((List<Checklist>)session["ListaChecklist"]).Count > 0)
            {
                return;
            }
            var lista = new List<Checklist>
            {
                new Checklist { id = 0, nome = "Teste", descricao = "Esta é uma tarefa exemplo!", dataCadastro = new DateTime(2025, 01, 01), prazo = new DateTime(2025, 12, 31), categoria = "Em andamento",  concluido = false },
                new Checklist { id = 1, nome = "Ler livro", descricao = "Terminar leitura do livro favorito", dataCadastro = new DateTime(2025, 02, 15), prazo = new DateTime(2025, 06, 30), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 2, nome = "Exercícios", descricao = "Fazer treino de academia", dataCadastro = new DateTime(2025, 03, 10), prazo = new DateTime(2025, 12, 31), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 3, nome = "Compras", descricao = "Comprar itens essenciais do mercado", dataCadastro = new DateTime(2025, 04, 20), prazo = new DateTime(2025, 04, 21), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 4, nome = "Projeto pessoal", descricao = "Desenvolver aplicação mobile", dataCadastro = new DateTime(2025, 05, 05), prazo = new DateTime(2025, 12, 01), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 5, nome = "Estudos", descricao = "Aprender um novo idioma", dataCadastro = new DateTime(2025, 06, 01), prazo = new DateTime(2026, 01, 01), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 6, nome = "Meditação", descricao = "Praticar mindfulness diariamente", dataCadastro = new DateTime(2025, 07, 07), prazo = new DateTime(2025, 12, 31), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 7, nome = "Curso online", descricao = "Finalizar curso de programação", dataCadastro = new DateTime(2025, 08, 10), prazo = new DateTime(2025, 10, 30), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 8, nome = "Reunião", descricao = "Preparar apresentação para equipe", dataCadastro = new DateTime(2025, 09, 15), prazo = new DateTime(2025, 09, 20), categoria = "Em Andamento", concluido = false },
                new Checklist { id = 9, nome = "Viagem", descricao = "Planejar férias de fim de ano", dataCadastro = new DateTime(2025, 10, 01), prazo = new DateTime(2025, 11, 30), categoria = "Em Andamento", concluido = false },
            };
            session["ListaChecklist"] = lista;
        }
        public static double Porcentagem(HttpSessionStateBase session)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;

            if (lista == null || lista.Count == 0)
                return 0;

            int total = lista.Count;
            int concluidas = lista.Count(t => t.concluido);

            double porcentagem = (double)concluidas / total * 100;
            return porcentagem;

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
            this.categoria = Categoria(this.prazo, this.concluido);
            this.dataCadastro = DateTime.Now;
            lista.Add(this);
        }
        public string Categoria(DateTime prazo, bool concluido)
        {
            switch (concluido)
            {
                case true when DateTime.Now <= prazo:
                    return "Concluída";
                case false when DateTime.Now <= prazo:
                    return "Em Andamento";
                case true when DateTime.Now > prazo:
                    return "Concluída com Atraso";
                case false when DateTime.Now > prazo:
                    return "Atrasada";
                default:
                    return "Sem Categoria";
            }
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
                original.nome = this.nome;
                original.descricao = this.descricao;
                original.concluido = this.concluido;
                original.dataCadastro = this.dataCadastro;
                original.prazo = this.prazo;
                original.concluido = this.concluido;
                original.categoria = Categoria(this.prazo, this.concluido);
            }
        }

        public void Excluir(HttpSessionStateBase session)
        {
            var lista = session["ListaChecklist"] as List<Checklist>;
            lista?.RemoveAll(a => a.id == this.id);
        }
    }
}