using System;

namespace GerenciadorProjetos
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int Prioridade { get; set; } // 1=alta, 2=média, 3=baixa
        public string Status { get; set; }  // "Aberta", "Fechada", "Cancelada"
        public DateTime DataCriacao { get; set; }
        public DateTime DataConclusao { get; set; }

        public Tarefa()
        {
            DataCriacao = DateTime.Now;
            Status = "Aberta";
        }

        public Tarefa(int id, string titulo, string descricao, int prioridade) : this()
        {
            Id = id;
            Titulo = titulo;
            Descricao = descricao;
            Prioridade = prioridade;
        }

        public void Concluir()
        {
            Status = "Fechada";
            DataConclusao = DateTime.Now;
        }

        public void Cancelar()
        {
            Status = "Cancelada";
            DataConclusao = DateTime.Now;
        }

        public void Reabrir()
        {
            Status = "Aberta";
            DataConclusao = DateTime.MinValue;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tarefa t)
            {
                return this.Id == t.Id;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public string ObterNomePrioridade()
        {
            return Prioridade switch
            {
                1 => "Alta",
                2 => "Média",
                3 => "Baixa",
                _ => "Desconhecida"
            };
        }

        public override string ToString()
        {
            string conclusaoStr = Status == "Fechada" || Status == "Cancelada" 
                ? $", Conclusão: {DataConclusao:dd/MM/yyyy HH:mm}" 
                : "";
            
            return $"[ID: {Id}] {Titulo} | Prioridade: {ObterNomePrioridade()} ({Prioridade}) | Status: {Status} | Criada: {DataCriacao:dd/MM/yyyy HH:mm}{conclusaoStr}\n  Descrição: {Descricao}";
        }
    }
}