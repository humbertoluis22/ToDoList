using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToDoList.Model
{
    public enum StatusTarefa
    {
        Pendente,
        EmAndamento,
        Concluido,
        Cancelado
    }

    public class TarefasModel
    {

        public int Id { get; set; }
        [Required,MaxLength(50)]
        public string  Titulo  { get; set; }

        [Required]
        public string Descricao {  get; set; }
        
        public DateTime Data { get; set; }

        [Required]
        public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;
    }
}
