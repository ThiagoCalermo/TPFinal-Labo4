using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoFinal_Labo4.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        // Foreign Key que referencia a afiliados
        public int AfiliadoId { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime FechaSolicitud { get; set; }
        [StringLength(500)]
        public string Observacion { get; set; }

        // Relaciones/
        public Afiliado? Afiliado { get; set; }
        public TicketDetalle? TicketDetalle { get; set; }
    }
}
