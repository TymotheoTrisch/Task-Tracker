using System;
using CLI.props;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net;
using System.Linq;
using System.Security;
using System.Diagnostics;
using System.Data.Common;
using System.Text;

// dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true


namespace CLI
{
    class Program
    {
        
        static string caminhoArquivo = "tarefa.json";
        

        public static void Main(string[] args)
        {
            
            Console.WriteLine("\n");

            
            if (args.Length == 0)
            {
                Console.WriteLine("Bem vindo ao To-Do-List. Siga os comandos para utilizar:");

                Console.WriteLine("\n\n# Adicionar uma nova tarefa:");
                Console.WriteLine("- Adicionar tarefa: task-cli add \"descrição\"");

                Console.WriteLine("\n\n# Atualizar e deletar uma tarefa:");
                Console.WriteLine("- Atualizar tarefa: task-cli update 1 \"descrição nova\"");
                Console.WriteLine("- Deletar tarefa:   task-cli delete 1");

                Console.WriteLine("\n\n# Marcar uma tarefa em andamento ou concluído:");
                Console.WriteLine("- Marcar o andamento: task-cli mark-in-progress 1");
                Console.WriteLine("- Marcar concluído:   task-cli mark-done 1");

                Console.WriteLine("\n\n# Listar todas as tarefas:");
                Console.WriteLine("- Listar: task-cli list");

                Console.WriteLine("\n\n# Listar todas as tarefas com o status em:");
                Console.WriteLine("- Listar tarefas concluídas: task-cli list done");
                Console.WriteLine("- Listar tarefas novas: task-cli list todo");
                Console.WriteLine("- Listar tarefas em andamento: task-cli list in-progress");
                

                Console.WriteLine("\nUso: task-cli <comando> [parametros]");
                // Console.WriteLine("Comandos: add, list, update");
                return;
            }

            List<Tarefa> tarefas = listarTarefasExistentes();
            string comando = args[0].ToLower();

            switch (comando)
            {
                case "add":
                    if (!verificaParametro(args)) return;
                    
                    // Descobre o próximo ID
                    int proximoId = (tarefas.Count > 0) ? tarefas.Max(t => t.Id) + 1 : 1;
                    string descricao = string.Join(" ", args.Skip(1));
                    
                    var novaTarefa = new Tarefa(proximoId, descricao);

                    tarefas.Add(novaTarefa);
                    SalvarTarefas(tarefas);
                    
                    Console.WriteLine($"Tarefa adicionada. Id: {novaTarefa.Id} - {novaTarefa.Description} \nData/Hora: {formataData(novaTarefa.UpdatedAt)}");
                    break;
                case "update":
                    if(!verificaParametro(args)) return;

                    // int id = int.Parse(args[1]);
                    int idEdit;
                    
                    if(!int.TryParse(args[1], out idEdit)) {
                       Console.WriteLine("O parâmetro [id] informado não pode ser convertido para um inteiro");
                        return;
                    }
                        
                    var editTarefa = tarefas.Find(t => t.Id == idEdit);
                    
                    if (editTarefa is null)
                    {
                        Console.WriteLine($"Nenhuma tarefa encontrada com o ID {idEdit}.");
                        return;
                    }

                    string newDescricao = string.Join(" ", args.Skip(2));

                    editTarefa.AtualizarDescricao(newDescricao);
                    
                    SalvarTarefas(tarefas);
                    
                    Console.WriteLine($"Tarefa editada. Id: {editTarefa.Id} - {editTarefa.Description} \nData/Hora: {formataData(editTarefa.UpdatedAt)}");
                    
                    // foreach (var t in tarefas)
                    // {
                    //     Console.WriteLine($"[{t.Id}] {t.Description} - {t.Status}");
                    // }
                    break;
                case "delete":
                    if(!verificaParametro(args)) return;
                    
                    int idDelete;
                    if(!int.TryParse(args[1], out idDelete)) {
                        Console.WriteLine("O parâmetro [id] informado não pode ser convertido para um inteiro");
                        return;
                    }

                    var tarefaDelete = tarefas.Find(t => t.Id == idDelete);
                    
                    if (tarefaDelete is null)
                    {
                        Console.WriteLine($"Nenhuma tarefa encontrada com o ID {idDelete}.");
                        return;
                    }

                    tarefas.Remove(tarefaDelete);                    
                    SalvarTarefas(tarefas);

                    Console.WriteLine($"Tarefa Deletada. Id: {tarefaDelete.Id} - {tarefaDelete.Description} \nData/Hora: {formataData(DateTime.Now)}");
                    
                    
                    break;
                case "list":
                    if (tarefas.Count == 0)
                    {
                        Console.WriteLine("Nenhuma tarefa cadastrada.");
                        return;
                    }
                    
                    if(args[1] == "done") {
                        listTasksDone(tarefas);
                    } else if(args[1] == "in-progress") {
                        listTasksInProgress(tarefas);
                    } else if(args[1] == "todo") {
                        listTasksTodo(tarefas);
                    } else {
                        listAllTasks(tarefas);                        
                    }
                    
                    break;
                case "mark-in-progress":
                    if(!verificaParametro(args)) return;

                    // int id = int.Parse(args[1]);
                    int idEditStatusProgress;
                    
                    if(!int.TryParse(args[1], out idEditStatusProgress)) {
                       Console.WriteLine("O parâmetro [id] informado não pode ser convertido para um inteiro");
                        return;
                    }
                        
                    var editTarefaStatusProgress = tarefas.Find(t => t.Id == idEditStatusProgress);
                    
                    if (editTarefaStatusProgress is null)
                    {
                        Console.WriteLine($"Nenhuma tarefa encontrada com o ID {idEditStatusProgress}.");
                        return;
                    }

                    string markProgressTarefa = "in-progress";

                    editTarefaStatusProgress.AtualizarStatus(markProgressTarefa);
                    
                    SalvarTarefas(tarefas);
                    
                    Console.WriteLine($"Tarefa editada. Id: {editTarefaStatusProgress.Id} - {editTarefaStatusProgress.Description} - {editTarefaStatusProgress.Status} \nData/Hora: {formataData(editTarefaStatusProgress.UpdatedAt)}");
                    break;
                case "mark-done":
                    if(!verificaParametro(args)) return;

                    // int id = int.Parse(args[1]);
                    int idEditStatusDone;
                    
                    if(!int.TryParse(args[1], out idEditStatusDone)) {
                       Console.WriteLine("O parâmetro [id] informado não pode ser convertido para um inteiro");
                        return;
                    }
                        
                    var editTarefaStatusDone = tarefas.Find(t => t.Id == idEditStatusDone);
                    
                    if (editTarefaStatusDone is null)
                    {
                        Console.WriteLine($"Nenhuma tarefa encontrada com o ID {idEditStatusDone}.");
                        return;
                    }

                    string markDoneTarefa = "done";

                    editTarefaStatusDone.AtualizarStatus(markDoneTarefa);
                    
                    SalvarTarefas(tarefas);
                    
                    Console.WriteLine($"Tarefa editada. Id: {editTarefaStatusDone.Id} - {editTarefaStatusDone.Description} - {editTarefaStatusDone.Status} \nData/Hora: {formataData(editTarefaStatusDone.UpdatedAt)}");
                    break;
                default:
                    Console.WriteLine("Opção não encontrada");
                    Console.WriteLine("Utilize: task-cli <comando> [parametros]");
                    break;
            }

            Console.WriteLine("\n");
        }
        
        public static bool verificaParametro(string[] args) {
            if(args.Length == 1) {
                Console.WriteLine("Por gentileza, insira corretamente o comando.");
                Console.WriteLine("\nExemplo: task-cli <comando> [parametros]");
                return false;
            } else {
                return true;
            }
        }
        
        public static string formataData(DateTime? date) {
            if(date is null) {
                return "Sem data";
            } else {
                return date?.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
        
        public static List<Tarefa> listarTarefasExistentes() {
            // string caminhoArquivo = "tarefa.json";
            List<Tarefa> tarefas = new();
            
            if (File.Exists(caminhoArquivo))
            {
                try
                {
                    string json = File.ReadAllText(caminhoArquivo);

                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        tarefas = JsonSerializer.Deserialize<List<Tarefa>>(json) ?? new List<Tarefa>();
                    }
                }
                catch (JsonException)
                {
                    Console.WriteLine("⚠ Arquivo JSON inválido. Criando nova lista de tarefas.");
                    tarefas = new List<Tarefa>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠ Erro ao ler o arquivo: {ex.Message}");
                    tarefas = new List<Tarefa>();
                }
            }

            return tarefas;
                        
        }
        
        public static void SalvarTarefas(List<Tarefa> tarefas)
        {
            string json = JsonSerializer.Serialize(tarefas, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(caminhoArquivo, json);
        }
    
        static string MontarTabela(List<Tarefa> tarefas)
        {
            var sb = new StringBuilder();

            // Cabeçalho
            sb.AppendLine($"{"ID",-5} {"Descrição",-50} {"Status",-15} {"Data de criação", -22} {"Data de modificação", -22}");
            sb.AppendLine(new string('-', 115));

            // Linhas
            foreach (var t in tarefas)
            {
                sb.AppendLine($"{t.Id,-5} {t.Description,-50} {t.Status,-15} {formataData(t.CreatedAt),-22}{formataData(t.UpdatedAt),-22}");
            }

            return sb.ToString();
        }
    
        static void listAllTasks(List<Tarefa> tarefas) {
            Console.WriteLine(MontarTabela(tarefas));
        }
        
        static void listTasksTodo(List<Tarefa> tarefas) {
            List<Tarefa> tasksTodo = tarefas.FindAll(t => t.Status == "todo");
            
            if (tasksTodo is null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o status 'todo'.");
                return;
            }
            
            Console.WriteLine(MontarTabela(tasksTodo));
        }
        
        static void listTasksInProgress(List<Tarefa> tarefas) {
            List<Tarefa> tasksInProgress = tarefas.FindAll(t => t.Status == "in-progress");
            
            if (tasksInProgress is null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o status 'in-progress'.");
                return;
            }
            
            Console.WriteLine(MontarTabela(tasksInProgress));
        }
        
        static void listTasksDone(List<Tarefa> tarefas) {
            List<Tarefa> tasksDone = tarefas.FindAll(t => t.Status == "done");
            
            if (tasksDone is null)
            {
                Console.WriteLine($"Nenhuma tarefa encontrada com o status 'done'.");
                return;
            }
            
            Console.WriteLine(MontarTabela(tasksDone));
        }
    
    }
}

// task-cli update 1 "Buy groceries and cook dinner"
// task-cli delete 1

// # Marking a task as in progress or done
// task-cli mark-in-progress 1
// task-cli mark-done 1

// # Listing all tasks
// task-cli list

// # Listing tasks by status
// task-cli list done
// task-cli list todo
// task-cli list in-progress