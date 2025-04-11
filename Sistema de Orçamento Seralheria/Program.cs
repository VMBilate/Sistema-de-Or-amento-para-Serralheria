using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        string caminhoPasta = @"C:\Banco de dados";
        string caminhoArquivo = Path.Combine(caminhoPasta, "orcamentos.json");


        if (!Directory.Exists(caminhoPasta))
        {
            Directory.CreateDirectory(caminhoPasta);
        }

        List<Orcamento> orcamentos = new List<Orcamento>();

        try
        {
            if (File.Exists(caminhoArquivo))
            {
                string json = File.ReadAllText(caminhoArquivo);
                orcamentos = JsonSerializer.Deserialize<List<Orcamento>>(json);
            }
        }
        catch (Exception erro)
        {
            Console.WriteLine("Houve um erro ao tentar ler os dados do arquivo: " + erro.Message);
        }

        int opcao = 0;


        while (opcao != 4)
        {
            Console.WriteLine("\nMenu de opções:");
            Console.WriteLine("1 - Criar um novo orçamento");
            Console.WriteLine("2 - Listar todos os orçamentos");
            Console.WriteLine("3 - Buscar orçamento pelo CPF");
            Console.WriteLine("4 - Sair");
            Console.Write("Escolha uma opção (1, 2, 3 ou 4): ");

            try
            {
                opcao = int.Parse(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Opção inválida, tente novamente.");
                continue;
            }

            if (opcao == 1)
            {
                Orcamento o = new Orcamento();
                o.Cliente = new Cliente();


                Console.Write("Nome do cliente: ");
                o.Cliente.Nome = Console.ReadLine();
                Console.Write("Telefone do cliente: ");
                o.Cliente.Telefone = Console.ReadLine();
                Console.Write("CPF do cliente: ");
                o.Cliente.CPF = Console.ReadLine();


                Console.Write("Quantos itens deseja adicionar? ");
                int qtd = 0;
                try
                {
                    qtd = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Número inválido.");
                    continue;
                }


                for (int i = 0; i < qtd; i++)
                {
                    Item item = new Item();

                    Console.Write("Nome do item: ");
                    item.Nome = Console.ReadLine();

                    Console.WriteLine("Tipo do item (1 - Portão, 2 - Grade, 3 - Telhado): ");
                    try
                    {
                        item.Tipo = (TipoItem)int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Tipo inválido.");
                        continue;
                    }

                    Console.Write("Quantidade do item: ");
                    try
                    {
                        item.Quantidade = int.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Quantidade inválida.");
                        continue;
                    }

                    Console.Write("Valor unitário do item: ");
                    try
                    {
                        item.ValorUnitario = double.Parse(Console.ReadLine());
                    }
                    catch
                    {
                        Console.WriteLine("Valor inválido.");
                        continue;
                    }

                    Console.Write("Material do item: ");
                    item.Material = Console.ReadLine();


                    o.Itens.Add(item);
                }


                orcamentos.Add(o);
                Console.WriteLine("Orçamento criado com sucesso!");
            }

            else if (opcao == 2)
            {
                if (orcamentos.Count == 0)
                {
                    Console.WriteLine("Nenhum orçamento cadastrado.");
                }
                else
                {
                    foreach (Orcamento o in orcamentos)
                    {

                        Console.WriteLine("----------------------------");
                        Console.WriteLine("Cliente: " + o.Cliente.Nome);
                        Console.WriteLine("CPF: " + o.Cliente.CPF);
                        Console.WriteLine("Data: " + o.Data.ToShortDateString());

                        foreach (Item item in o.Itens)
                        {
                            Console.WriteLine("Item: " + item.Nome);
                            Console.WriteLine("Tipo: " + item.Tipo);
                            Console.WriteLine("Material: " + item.Material);
                            Console.WriteLine("Quantidade: " + item.Quantidade);
                            Console.WriteLine("Valor Unitário: " + item.ValorUnitario);
                        }

                        Console.WriteLine("Total do orçamento: " + o.ValorTotal());
                    }
                }
            }

            else if (opcao == 3)
            {
                Console.Write("Digite o CPF para buscar o orçamento: ");
                string cpfBusca = Console.ReadLine();
                bool achou = false;

                foreach (Orcamento o in orcamentos)
                {

                    if (o.Cliente.CPF == cpfBusca)
                    {
                        Console.WriteLine("Orçamento encontrado:");
                        Console.WriteLine("Cliente: " + o.Cliente.Nome);
                        Console.WriteLine("Data: " + o.Data.ToShortDateString());
                        foreach (Item item in o.Itens)
                        {
                            Console.WriteLine("Item: " + item.Nome + " | Quantidade: " + item.Quantidade + " | Valor: " + item.ValorUnitario);
                        }
                        Console.WriteLine("Total do orçamento: " + o.ValorTotal());
                        achou = true;
                        break;
                    }
                }

                if (!achou)
                {
                    Console.WriteLine("Orçamento não encontrado.");
                }
            }

            if (opcao != 4)
            {
                try
                {
                    string json = JsonSerializer.Serialize(orcamentos, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(caminhoArquivo, json);
                    Console.WriteLine("Orçamentos salvos com sucesso!");
                }
                catch (Exception erro)
                {
                    Console.WriteLine("Houve um erro ao tentar salvar os orçamentos: " + erro.Message);
                }
            }
        }

        Console.WriteLine("Programa encerrado.");
    }
}

class Cliente
{
    public string Nome { get; set; }
    public string Telefone { get; set; }
    public string CPF { get; set; }
}

class Item
{
    public string Nome { get; set; }
    public TipoItem Tipo { get; set; }
    public int Quantidade { get; set; }
    public double ValorUnitario { get; set; }
    public string Material { get; set; }

    public double Total()
    {
        return Quantidade * ValorUnitario;
    }
}

class Orcamento
{
    public Cliente Cliente { get; set; }
    public List<Item> Itens { get; set; } = new List<Item>();
    public DateTime Data { get; set; } = DateTime.Now;

    public double ValorTotal()
    {
        double soma = 0;
        foreach (Item item in Itens)
        {
            soma += item.Total();
        }
        return soma;
    }
}

enum TipoItem
{
    Portao = 1,
    Grade = 2,
    Telhado = 3
}
