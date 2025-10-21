using System;

namespace CLI.props
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Tarefa(int id, string description)
        {
            Id = id;
            Description = description;
            CreatedAt = DateTime.Now;
            Status = "todo";
        }
        
        public void AtualizarDescricao(string novaDescricao)
        {
            Description = novaDescricao;
            UpdatedAt = DateTime.Now;
        }

        public void AtualizarStatus(string novoStatus)
        {
            Status = novoStatus;
            UpdatedAt = DateTime.Now;
        }



    }
}