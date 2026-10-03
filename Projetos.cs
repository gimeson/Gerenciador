using System.Collections.Generic;
using System.Linq;

namespace GerenciadorProjetos
{
    public class Projetos
    {
        private List<Projeto> itens;

        public Projetos()
        {
            itens = new List<Projeto>();
        }

        public bool Adicionar(Projeto p)
        {
            if (itens.Any(x => x.Equals(p)))
                return false;

            itens.Add(p);
            return true;
        }

        public bool Remover(Projeto p)
        {
            Projeto proj = Buscar(p);
            if (proj != null && proj.Tarefas.Count == 0)
            {
                return itens.Remove(proj);
            }
            return false;
        }

        public Projeto Buscar(Projeto p)
        {
            return itens.FirstOrDefault(x => x.Equals(p));
        }

        public List<Projeto> Listar()
        {
            return itens;
        }
    }
}