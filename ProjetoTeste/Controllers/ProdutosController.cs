using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Models;

namespace ProjetoTeste.Controllers {
    public class ProdutosController : Controller {
        private readonly LksMarcondesDbContext _context;

        public ProdutosController(LksMarcondesDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            return View(await _context.Produtos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null) {
                return NotFound();
            }

            return View(produto);
        }

        public IActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Ativo,Inserido")] Produto produto) {
            if (ModelState.IsValid) {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null) {
                return NotFound();
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Ativo,Inserido")] Produto produto) {
            if (id != produto.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    _context.Update(produto);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!ProdutoExists(produto.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(produto);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var produto = await _context.Produtos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (produto == null) {
                return NotFound();
            }

            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto.Inserido == true) {
                ViewBag.Message = String.Format("Nao pode ser excluido, pois ele esta relacionado com um Cliente");
                return View(produto);
            } else {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProdutoExists(int id) {
            return _context.Produtos.Any(e => e.Id == id);
        }
    }
}
