using Microsoft.EntityFrameworkCore;


namespace TrabajoPracticoFinal_Labo4.Models
{
	public class AppDbContext : DbContext
	{

		public AppDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<Ticket> Tickets { get; set; }
		public DbSet<TicketDetalle> TicketDetalles { get; set; }
		public DbSet<Estado> Estado { get; set; }
		public DbSet<Afiliado> Afiliados { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			// Configuración de relaciones y restricciones adicionales si es necesario

			modelBuilder.Entity<Ticket>()//Un ticket
				.HasOne(t => t.TicketDetalle)//Tiene un TicketDetalle
				.WithOne(td => td.Ticket)//con un Ticket (Osea un TicketDetalle puede tener solo un ticket)
				.HasForeignKey<TicketDetalle>(td => td.TicketId)//la clave foranea de TD es TicketId
				.OnDelete(DeleteBehavior.Cascade);//Si elimino un Ticket se borrara su correspondiente TicketD

			modelBuilder.Entity<Ticket>()//Un ticket
				.HasOne(t => t.Afiliado)//Puede tener un afiliado
				.WithMany(a => a.Tickets)//que puede tener muchos tickets
				.HasForeignKey(t => t.AfiliadoId)//con la clave foranea en ticket "AfiliadoId"
				.OnDelete(DeleteBehavior.Cascade);//Si elimino un ticket se eliminan los afiliados correspondientes

			modelBuilder.Entity<TicketDetalle>()//Un tockectDetalle
				.HasOne(td => td.Estado)//tiene un estado
				.WithMany(e => e.TicketDetalles)//que tiene muchos ticketDetalles
				.HasForeignKey(td => td.EstadoId)//con la clave foranea EstadoId
				.OnDelete(DeleteBehavior.Restrict);//Si se intenta eliminar un Estado que está referenciado por algún TicketDetalle,
												   //se restringe la eliminación para evitar la inconsistencia de datos.

		}

	}
}
