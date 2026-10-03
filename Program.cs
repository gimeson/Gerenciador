using System;
using System.Collections.Generic;

namespace GerenciadorProjetos
{
    class Program
    {
        static Projetos cadastroProjetos = new Projetos();
        static int contadorIdTarefa = 1;

        static void Main(string[] args)
        {
            int opcao = -1;

            do
            {
                Console.Clear();
                Console.WriteLine("====================================================");
                Console.WriteLine("             GERENCIADOR DE PROJETOS                ");
                Console.WriteLine("====================================================");
                Console.WriteLine("0. Sair");
                Console.WriteLine("1. Adicionar projeto");
                Console.WriteLine("2. Pesquisar projeto (mostrar tarefas por status e totais)");
                Console.WriteLine("3. Remover projeto (apenas se sem tarefas)");
                Console.WriteLine("4. Adicionar tarefa em projeto");
                Console.WriteLine("5. Concluir tarefa");
                Console.WriteLine("6. Cancelar tarefa");
                Console.WriteLine("7. Reabrir tarefa");
                Console.WriteLine("8. Listar tarefas de um projeto");
                Console.WriteLine("9. Filtrar tarefas por status ou prioridade em um projeto");
                Console.WriteLine("10. Filtrar tarefas por status ou prioridade em todos os projetos");
                Console.WriteLine("11. Resumo geral");
                Console.WriteLine("====================================================");
                Console.Write("Opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    ExibirMensagem("Opção inválida! Pressione qualquer tecla para tentar novamente.");
                    continue;
                }

                Console.Clear();
                switch (opcao)
                {
                    case 0:
                        Console.WriteLine("Saindo do sistema... Até logo!");
                        break;
                    case 1: AdicionarProjeto(); break;
                    case 2: PesquisarProjeto(); break;
                    case 3: RemoverProjeto(); break;
                    case 4: AdicionarTarefa(); break;
                    case 5: AlterarStatusTarefa("concluir"); break;
                    case 6: AlterarStatusTarefa("cancelar"); break;
                    case 7: AlterarStatusTarefa("reabrir"); break;
                    case 8: ListarTarefasProjeto(); break;
                    case 9: FiltrarTarefasEmProjeto(); break;
                    case 10: FiltrarTarefasEmTodosProjetos(); break;
                    case 11: ResumoGeral(); break;
                    default:
                        ExibirMensagem("Opção inválida!");
                        break;
                }

            } while (opcao != 0);
        }

        static void AdicionarProjeto()
        {
            Console.WriteLine("--- ADICIONAR PROJETO ---");
            Console.Write("ID do projeto: ");
            int id = LerInteiro();
            Console.Write("Nome do projeto: ");
            string nome = Console.ReadLine();

            Projeto p = new Projeto(id, nome);
            if (cadastroProjetos.Adicionar(p))
            {
                ExibirMensagem("Projeto adicionado com sucesso!");
            }
            else
            {
                ExibirMensagem("Erro: Já existe um projeto cadastrado com este ID.");
            }
        }

        static void PesquisarProjeto()
        {
            Console.WriteLine("--- PESQUISAR PROJETO ---");
            Console.Write("Digite o ID do projeto: ");
            int id = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(id, ""));
            if (p != null)
            {
                Console.WriteLine($"\nProjeto: {p.Nome} (ID: {p.Id})");
                Console.WriteLine($"Tarefas Abertas: {p.TotalAbertas()}");
                Console.WriteLine($"Tarefas Fechadas: {p.TotalFechadas()}");
                Console.WriteLine($"Tarefas Canceladas: {p.TotalCanceladas()}");

                Console.WriteLine("\n--- TAREFAS POR STATUS ---");
                
                Console.WriteLine("\n> ABERTAS:");
                foreach (var t in p.TarefasPorStatus("Aberta"))
                    Console.WriteLine(t);

                Console.WriteLine("\n> FECHADAS:");
                foreach (var t in p.TarefasPorStatus("Fechada"))
                    Console.WriteLine(t);

                Console.WriteLine("\n> CANCELADAS:");
                foreach (var t in p.TarefasPorStatus("Cancelada"))
                    Console.WriteLine(t);
            }
            else
            {
                Console.WriteLine("Projeto não encontrado.");
            }
            Pausar();
        }

        static void RemoverProjeto()
        {
            Console.WriteLine("--- REMOVER PROJETO ---");
            Console.Write("Digite o ID do projeto a remover: ");
            int id = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(id, ""));
            if (p == null)
            {
                ExibirMensagem("Projeto não encontrado.");
                return;
            }

            if (p.Tarefas.Count > 0)
            {
                ExibirMensagem($"Não é possível remover o projeto '{p.Nome}'. Ele possui {p.Tarefas.Count} tarefa(s) vinculada(s).");
                return;
            }

            if (cadastroProjetos.Remover(p))
            {
                ExibirMensagem("Projeto removido com sucesso!");
            }
            else
            {
                ExibirMensagem("Não foi possível remover o projeto.");
            }
        }

        static void AdicionarTarefa()
        {
            Console.WriteLine("--- ADICIONAR TAREFA ---");
            Console.Write("ID do projeto que receberá a tarefa: ");
            int idProj = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(idProj, ""));
            if (p == null)
            {
                ExibirMensagem("Projeto não encontrado.");
                return;
            }

            Console.Write("Título da tarefa: ");
            string titulo = Console.ReadLine();
            Console.Write("Descrição da tarefa: ");
            string desc = Console.ReadLine();
            Console.Write("Prioridade (1 = Alta, 2 = Média, 3 = Baixa): ");
            int prioridade = LerInteiro();

            if (prioridade < 1 || prioridade > 3)
            {
                ExibirMensagem("Prioridade inválida! Apenas valores entre 1 e 3 são aceitos.");
                return;
            }

            Tarefa t = new Tarefa(contadorIdTarefa++, titulo, desc, prioridade);
            p.AdicionarTarefa(t);

            ExibirMensagem($"Tarefa ID {t.Id} criada e adicionada com sucesso no projeto '{p.Nome}'!");
        }

        static void AlterarStatusTarefa(string acao)
        {
            Console.WriteLine($"--- {acao.ToUpper()} TAREFA ---");
            Console.Write("ID do projeto: ");
            int idProj = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(idProj, ""));
            if (p == null)
            {
                ExibirMensagem("Projeto não encontrado.");
                return;
            }

            Console.Write("ID da tarefa: ");
            int idTar = LerInteiro();

            Tarefa t = p.BuscarTarefa(new Tarefa { Id = idTar });
            if (t == null)
            {
                ExibirMensagem("Tarefa não encontrada neste projeto.");
                return;
            }

            switch (acao)
            {
                case "concluir":
                    t.Concluir();
                    ExibirMensagem($"Tarefa ID {t.Id} concluída com sucesso!");
                    break;
                case "cancelar":
                    t.Cancelar();
                    ExibirMensagem($"Tarefa ID {t.Id} cancelada com sucesso!");
                    break;
                case "reabrir":
                    t.Reabrir();
                    ExibirMensagem($"Tarefa ID {t.Id} reaberta com sucesso!");
                    break;
            }
        }

        static void ListarTarefasProjeto()
        {
            Console.WriteLine("--- LISTAR TAREFAS DO PROJETO ---");
            Console.Write("ID do projeto: ");
            int idProj = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(idProj, ""));
            if (p == null)
            {
                ExibirMensagem("Projeto não encontrado.");
                return;
            }

            Console.WriteLine($"\nTarefas do projeto '{p.Nome}':");
            if (p.Tarefas.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa cadastrada neste projeto.");
            }
            else
            {
                foreach (var t in p.Tarefas)
                {
                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine(t);
                }
            }
            Pausar();
        }

        static void FiltrarTarefasEmProjeto()
        {
            Console.WriteLine("--- FILTRAR TAREFAS DE UM PROJETO ---");
            Console.Write("ID do projeto: ");
            int idProj = LerInteiro();

            Projeto p = cadastroProjetos.Buscar(new Projeto(idProj, ""));
            if (p == null)
            {
                ExibirMensagem("Projeto não encontrado.");
                return;
            }

            Console.WriteLine("Filtrar por: 1 - Status | 2 - Prioridade");
            int tipo = LerInteiro();

            List<Tarefa> resultado = new List<Tarefa>();

            if (tipo == 1)
            {
                Console.Write("Digite o status (Aberta / Fechada / Cancelada): ");
                string st = Console.ReadLine();
                resultado = p.TarefasPorStatus(st);
            }
            else if (tipo == 2)
            {
                Console.Write("Digite a prioridade (1=Alta, 2=Média, 3=Baixa): ");
                int prio = LerInteiro();
                resultado = p.TarefasPorPrioridade(prio);
            }
            else
            {
                ExibirMensagem("Opção de filtro inválida.");
                return;
            }

            Console.WriteLine($"\n--- RESULTADO DA BUSCA NO PROJETO {p.Nome} ---");
            if (resultado.Count == 0)
            {
                Console.WriteLine("Nenhuma tarefa encontrada com o critério informado.");
            }
            else
            {
                foreach (var t in resultado)
                {
                    Console.WriteLine("----------------------------------------------------");
                    Console.WriteLine(t);
                }
            }
            Pausar();
        }

        static void FiltrarTarefasEmTodosProjetos()
        {
            Console.WriteLine("--- FILTRAR TAREFAS EM TODOS OS PROJETOS ---");
            Console.WriteLine("Filtrar por: 1 - Status | 2 - Prioridade");
            int tipo = LerInteiro();

            string statusBusca = "";
            int prioridadeBusca = 0;

            if (tipo == 1)
            {
                Console.Write("Digite o status (Aberta / Fechada / Cancelada): ");
                statusBusca = Console.ReadLine();
            }
            else if (tipo == 2)
            {
                Console.Write("Digite a prioridade (1=Alta, 2=Média, 3=Baixa): ");
                prioridadeBusca = LerInteiro();
            }
            else
            {
                ExibirMensagem("Opção de filtro inválida.");
                return;
            }

            bool encontrouAlguma = false;

            foreach (var proj in cadastroProjetos.Listar())
            {
                List<Tarefa> resultado = (tipo == 1) 
                    ? proj.TarefasPorStatus(statusBusca) 
                    : proj.TarefasPorPrioridade(prioridadeBusca);

                if (resultado.Count > 0)
                {
                    encontrouAlguma = true;
                    Console.WriteLine($"\n>>> Projeto: {proj.Nome} (ID: {proj.Id})");
                    foreach (var t in resultado)
                    {
                        Console.WriteLine("----------------------------------------------------");
                        Console.WriteLine(t);
                    }
                }
            }

            if (!encontrouAlguma)
            {
                Console.WriteLine("\nNenhuma tarefa encontrada em nenhum projeto com o critério informado.");
            }

            Pausar();
        }

        static void ResumoGeral()
        {
            Console.WriteLine("--- RESUMO GERAL DO SISTEMA ---");

            List<Projeto> listaProjetos = cadastroProjetos.Listar();
            int totalProjetos = listaProjetos.Count;

            int totalAbertas = 0;
            int totalFechadas = 0;
            int totalCanceladas = 0;

            foreach (var proj in listaProjetos)
            {
                totalAbertas += proj.TotalAbertas();
                totalFechadas += proj.TotalFechadas();
                totalCanceladas += proj.TotalCanceladas();
            }

            int totalGeralTarefas = totalAbertas + totalFechadas + totalCanceladas;

            // % concluídas em relação às abertas (Fechadas / (Fechadas + Abertas))
            double percentualConcluidas = (totalFechadas + totalAbertas) > 0 
                ? ((double)totalFechadas / (totalFechadas + totalAbertas)) * 100 
                : 0.0;

            Console.WriteLine($"Quantidade Total de Projetos : {totalProjetos}");
            Console.WriteLine($"Quantidade Total de Tarefas  : {totalGeralTarefas}");
            Console.WriteLine($"  - Tarefas Abertas          : {totalAbertas}");
            Console.WriteLine($"  - Tarefas Fechadas         : {totalFechadas}");
            Console.WriteLine($"  - Tarefas Canceladas       : {totalCanceladas}");
            Console.WriteLine($"% Concluídas (em rel. às abertas/fechadas): {percentualConcluidas:F2}%");

            Pausar();
        }

        static int LerInteiro()
        {
            int valor;
            while (!int.TryParse(Console.ReadLine(), out valor))
            {
                Console.Write("Entrada inválida! Digite um número inteiro válido: ");
            }
            return valor;
        }

        static void ExibirMensagem(string msg)
        {
            Console.WriteLine($"\n{msg}");
            Pausar();
        }

        static void Pausar()
        {
            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}