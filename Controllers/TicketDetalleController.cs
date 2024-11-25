using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TrabajoPracticoFinal_Labo4.Models;

namespace TrabajoPracticoFinal_Labo4.Controllers
{
	public class TicketDetalleController : Controller
	{
		private readonly AppDbContext _context;

		public TicketDetalleController(AppDbContext context)
		{
			_context = context;
		}

		// GET: TicketDetalle
		public async Task<IActionResult> Index()
		{
			var appDbContent = _context.TicketDetalles.Include(t => t.Estado.Id).Include(t => t.Ticket.Id);
			return View(await appDbContent.ToListAsync());
		}

		// GET: TicketDetalle/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var ticketDetalle = await _context.TicketDetalles
				.Include(t => t.Estado)
				.Include(t => t.Ticket)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (ticketDetalle == null)
			{
				return NotFound();
			}

			return View(ticketDetalle);
		}

		// GET: TicketDetalle/Create
		public async Task<IActionResult> Create()
		{
			ViewData["EstadoId"] = new SelectList(await _context.Estado.ToListAsync(), "Id", "Id");
			ViewData["TicketId"] = new SelectList(await _context.Tickets.ToListAsync(), "Id", "Id");
			return View();
		}

		// POST: TicketDetalle/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
		{
			if (ModelState.IsValid)
			{
				_context.Add(ticketDetalle);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", ticketDetalle.EstadoId);
			ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
			return View(ticketDetalle);
		}

		// GET: TicketDetalle/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var ticketDetalle = await _context.TicketDetalles.FindAsync(id);
			if (ticketDetalle == null)
			{
				return NotFound();
			}
			ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", ticketDetalle.EstadoId);
			ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
			return View(ticketDetalle);
		}

		// POST: TicketDetalle/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,TicketId,DescripcionPedido,EstadoId,FechaEstado")] TicketDetalle ticketDetalle)
		{
			if (id != ticketDetalle.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(ticketDetalle);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!TicketDetalleExists(ticketDetalle.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["EstadoId"] = new SelectList(_context.Estado, "Id", "Id", ticketDetalle.EstadoId);
			ViewData["TicketId"] = new SelectList(_context.Tickets, "Id", "Id", ticketDetalle.TicketId);
			return View(ticketDetalle);
		}

		// GET: TicketDetalle/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var ticketDetalle = await _context.TicketDetalles
				.Include(t => t.Estado)
				.Include(t => t.Ticket)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (ticketDetalle == null)
			{
				return NotFound();
			}

			return View(ticketDetalle);
		}

		// POST: TicketDetalle/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var ticketDetalle = await _context.TicketDetalles.FindAsync(id);
			if (ticketDetalle != null)
			{
				_context.TicketDetalles.Remove(ticketDetalle);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool TicketDetalleExists(int id)
		{
			return _context.TicketDetalles.Any(e => e.Id == id);
		}
	}
}
