using System;
using System.Collections.Generic;
using System.Linq;

namespace GerenciadorProjetos
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Tarefa> Tarefas { get; set; }

        public Projeto()
        {
            Tarefas = new List<Tarefa>();
        }

        public Projeto(int id, string nome) : this()
        {
            Id = id;
            Nome = nome;
        }

        public void AdicionarTarefa(Tarefa t)
        {
            Tarefas.Add(t);
        }

        public bool RemoverTarefa(Tarefa t)
        {
            return Tarefas.Remove(t);
        }

        public Tarefa BuscarTarefa(Tarefa t)
        {
            return Tarefas.FirstOrDefault(x => x.Equals(t));
        }

        public List<Tarefa> TarefasPorStatus(string s)
        {
            return Tarefas.Where(t => t.Status.Equals(s, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public List<Tarefa> TarefasPorPrioridade(int p)
        {
            return Tarefas.Where(t => t.Prioridade == p).ToList();
        }

        public int TotalAbertas()
        {
            return Tarefas.Count(t => t.Status.Equals("Aberta", StringComparison.OrdinalIgnoreCase));
        }

        public int TotalFechadas()
        {
            return Tarefas.Count(t => t.Status.Equals("Fechada", StringComparison.OrdinalIgnoreCase));
        }

        public int TotalCanceladas()
        {
            return Tarefas.Count(t => t.Status.Equals("Cancelada", StringComparison.OrdinalIgnoreCase));
        }

        public override bool Equals(object obj)
        {
            if (obj is Projeto p)
            {
                return this.Id == p.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"[ID: {Id}] {Nome} (Total de Tarefas: {Tarefas.Count})";
        }
    }
}