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
	public class AfiliadoController : Controller
	{
		private readonly AppDbContext _context;

		public AfiliadoController(AppDbContext context)
		{
			_context = context;
		}

		// GET: Afiliado
		public async Task<IActionResult> Index()
		{
			return View(await _context.Afiliados.ToListAsync());
		}

		// GET: Afiliado/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var afiliado = await _context.Afiliados
				.FirstOrDefaultAsync(m => m.Id == id);
			if (afiliado == null)
			{
				return NotFound();
			}

			return View(afiliado);
		}

		// GET: Afiliado/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Afiliado/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Apellido,Nombre,Dni,FechaNacimiento,Foto")] Afiliado modelo)
		{
			if (ModelState.IsValid)
			{
				_context.Add(modelo);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(modelo);
		}

		// GET: Afiliado/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var afiliado = await _context.Afiliados.FindAsync(id);
			if (afiliado == null)
			{
				return NotFound();
			}
			return View(afiliado);
		}

		// POST: Afiliado/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Apellido,Nombre,Dni,FechaNacimiento,Foto")] Afiliado afiliado)
		{
			if (id != afiliado.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(afiliado);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!AfiliadoExists(afiliado.Id))
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
			return View(afiliado);
		}

		// GET: Afiliado/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var afiliado = await _context.Afiliados
				.FirstOrDefaultAsync(m => m.Id == id);
			if (afiliado == null)
			{
				return NotFound();
			}

			return View(afiliado);
		}

		// POST: Afiliado/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var afiliado = await _context.Afiliados.FindAsync(id);
			if (afiliado != null)
			{
				_context.Afiliados.Remove(afiliado);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool AfiliadoExists(int id)
		{
			return _context.Afiliados.Any(e => e.Id == id);
		}
	}
}
