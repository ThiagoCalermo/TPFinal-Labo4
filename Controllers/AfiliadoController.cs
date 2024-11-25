using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using TrabajoPracticoFinal_Labo4.Models;
using X.PagedList.Extensions;

namespace TrabajoPracticoFinal_Labo4.Controllers
{
	public class AfiliadoController : Controller
	{
		private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AfiliadoController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Afiliado
        public async Task<IActionResult> Index(string buscar, int? page, string filtro)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;

            var afiliado = from afiliados in _context.Afiliados select afiliados;

            // Filtro por nombre si se proporciona	
            if (!String.IsNullOrEmpty(buscar))
            {
                afiliado = afiliado.Where(s => s.Nombre!.Contains(buscar));
            }
            // Lógica de ordenación basada en el filtro
            switch (filtro)
            {
                case "NombreDescendente":
                    afiliado = afiliado.OrderByDescending(m => m.Nombre);
                    ViewData["FiltroNombre"] = "NombreAscendente";
                    break;
                case "NombreAscendente":
                    afiliado = afiliado.OrderBy(m => m.Nombre);
                    ViewData["FiltroNombre"] = "NombreDescendente";
                    break;
                default:
                    afiliado = afiliado.OrderBy(m => m.Nombre);
                    ViewData["FiltroNombre"] = "NombreDescendente";
                    ViewData["FiltroRaza"] = "RazaAscendente";
                    break;
            }
            // Realizamos la consulta de forma asíncrona y luego aplicamos la paginación
            var afiliadoList = await afiliado.OrderByDescending(s => s.Id).ToListAsync();
            var afiliadosPaginados = afiliadoList.ToPagedList(pageNumber, pageSize);

            return View(afiliadosPaginados);
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
		public async Task<IActionResult> Create([Bind("Id,Apellido,Nombre,Dni,FechaNacimiento,Foto")] Afiliado modelo, IFormFile images)
		{
            var archivos = HttpContext.Request.Form.Files;
            if (archivos != null && archivos.Count > 0)
            {
                var archivoFoto = archivos[0];
                if (archivoFoto.Length > 0)
                {
                    var pathDestino = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");
                    var extension = Path.GetExtension(archivoFoto.FileName);
                    archivoDestino += extension;

                    using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
                    {
                        archivoFoto.CopyTo(filestream);
                        if (modelo.Foto != null)
                        {
                            var archivoViejo = Path.Combine(pathDestino, modelo.Foto!);
                            if (System.IO.File.Exists(archivoViejo))
                            {
                                System.IO.File.Delete(archivoViejo);
                            }
                        }
                        modelo.Foto = archivoDestino;
                        Console.WriteLine(archivoDestino);
                    }
                }
            }

            _context.Add(modelo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

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
		public async Task<IActionResult> Edit(int id, [Bind("Id,Apellido,Nombre,Dni,FechaNacimiento,Foto")] Afiliado afiliado, IFormFile foto)
		{
			if (id != afiliado.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
                var archivos = HttpContext.Request.Form.Files;
				if (archivos != null && archivos.Count > 0)
				{
                    var archivoFoto = archivos[0];
					if (archivoFoto.Length > 0)
					{
                        var pathDestino = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                        var archivoDestino = Guid.NewGuid().ToString().Replace("-", "");
                        var extension = Path.GetExtension(archivoFoto.FileName);
                        archivoDestino += extension;

                        using (var filestream = new FileStream(Path.Combine(pathDestino, archivoDestino), FileMode.Create))
						{
							archivoFoto.CopyTo(filestream);
							if (afiliado.Foto != null)
							{
                                var archivoViejo = Path.Combine(pathDestino, afiliado.Foto!);
								if (System.IO.File.Exists(archivoViejo))
								{
                                    System.IO.File.Delete(archivoViejo);
                                }
                            }
							afiliado.Foto = archivoDestino;
						}
                    }
                }
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
				//Elimina el archivo de imagen si exite
				if (!string.IsNullOrEmpty(afiliado.Foto)) 
				{
					string ruta = Path.Combine(_webHostEnvironment.WebRootPath, afiliado.Foto.TrimStart('/'));
					if (System.IO.File.Exists(ruta))
					{
						System.IO.File.Delete(ruta);
					}
                }
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
