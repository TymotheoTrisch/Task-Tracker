# Task Tracker (Console App em C#)

> Um simples rastreador de tarefas em console, implementado em C#.  
> Projeto desenvolvido como parte do desafio “Task Tracker” do roadmap.sh.

## Motivação
Este projeto foi criado para cumprir o desafio do roadmap: https://roadmap.sh/projects/task-tracker  
Ele visa reforçar conceitos como classes, collections, entrada/saída no console e fluxos básicos de manipulação de dados em C#.  
Além disso, serve como exemplo de aplicação simples para acompanhar tarefas (como “to-do”).

## Funcionalidades
- Criar nova tarefa com título, descrição, data 
- Listar todas as tarefas registradas  
- Marcar tarefa como concluída  
- Filtrar/mostrar apenas tarefas pendentes ou concluídas  
- Apagar e editar tarefa  

## Tecnologias utilizadas
- Linguagem: C#  
- Plataforma: .NET (versão 9.0)  
- Tipo de aplicação: Console (um único arquivo executável + classe `Tarefa`)  
- Sistema de execução: linha de comando / terminal  

## Como rodar
### Pré-requisitos
- .NET SDK instalado (por exemplo, .NET 6 ou .NET 7)  
- Terminal / linha de comando  

### Passos
```bash
git clone https://github.com/TymotheoTrisch/Task-Tracker.git
cd Task-Tracker
dotnet build
dotnet run
````

## Uso

Para utilizar o programa, utilize esse padrão:

``` 
task-cli <comando> [parametros]
```

Ao executar o programa, você será apresentado a um menu no console com opções como:

```
# Adicionar uma nova tarefa:
- Adicionar tarefa: task-cli add "descrição"


# Atualizar e deletar uma tarefa:
- Atualizar tarefa: task-cli update 1 "descrição nova"
- Deletar tarefa:   task-cli delete 1


# Marcar uma tarefa em andamento ou concluído:
- Marcar o andamento: task-cli mark-in-progress 1
- Marcar concluído:   task-cli mark-done 1


# Listar todas as tarefas:
- Listar: task-cli list


# Listar todas as tarefas com o status em:
- Listar tarefas concluídas: task-cli list done
- Listar tarefas novas: task-cli list todo
- Listar tarefas em andamento: task-cli list in-progress

```