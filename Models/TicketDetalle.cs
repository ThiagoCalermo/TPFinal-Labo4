using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoFinal_Labo4.Models
{
    public class TicketDetalle
    {
        public int Id { get; set; }
        // Foreign Keys
        public int TicketId { get; set; }
        [StringLength(500)]
        public string DescripcionPedido { get; set; }
        // Foreing Keys
        public int EstadoId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FechaEstado { get; set; }

        // Relaciones/ TicketDetallo llama a estas dos entidades:
        public Ticket? Ticket { get; set; }
        public Estado? Estado { get; set; }
    }
}
