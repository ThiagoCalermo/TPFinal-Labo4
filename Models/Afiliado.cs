using System.ComponentModel.DataAnnotations;

namespace TrabajoPracticoFinal_Labo4.Models
{
	public class Afiliado
	{
		public int Id { get; set; }
		[Required]
		[StringLength(50)]
		public string Apellido { get; set; }
		[Required]
		[StringLength(50)]
		public string Nombre { get; set; }
		[Required]
		public int Dni { get; set; }
		[DataType(DataType.Date)]
		public DateTime FechaNacimiento { get; set; }
		public string? Foto { get; set; }

		// Relación/ esto define una relación de 1 a m entre afiliados y tickets, en este caso un afiliado puede estar en muchos tickets.
		public ICollection<Ticket> Tickets { get; set; }


	}
}
