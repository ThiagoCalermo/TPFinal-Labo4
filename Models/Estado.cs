using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoFinal_Labo4.Models
{
    public class Estado
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Descripcion { get; set; }

        // Relación/ un estado puede estar en muchos tickets detalle.
        public ICollection<TicketDetalle> TicketDetalles { get; set; }

    }
}
