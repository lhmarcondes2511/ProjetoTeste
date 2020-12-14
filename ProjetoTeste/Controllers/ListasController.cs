using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Models;

namespace ProjetoTeste.Controllers {
    public class ListasController : Controller {
        private readonly LksMarcondesDbContext _context;

        public ListasController(LksMarcondesDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index(string nome, string cpf, string email) {
            var lksMarcondesDbContext = _context.Listas.Include(l => l.Cliente).Include(l => l.Produto).Where(x => x.Produto.Ativo != false);

            if (!String.IsNullOrEmpty(nome)) {
                lksMarcondesDbContext = lksMarcondesDbContext.Where(x => x.Cliente.Nome.ToLower().Contains(nome.ToLower()));
            }
            if (!String.IsNullOrEmpty(cpf)) {
                lksMarcondesDbContext = lksMarcondesDbContext.Where(x => x.Cliente.Cpf.Contains(cpf));
            }
            if (!String.IsNullOrEmpty(email)) {
                lksMarcondesDbContext = lksMarcondesDbContext.Where(x => x.Cliente.Email.ToLower().Contains(email.ToLower()));
            }
            
            return View(await lksMarcondesDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var listas = await _context.Listas.Include(l => l.Cliente).Include(l => l.Produto).FirstOrDefaultAsync(m => m.Id == id);
            if (listas == null) {
                return NotFound();
            }

            return View(listas);
        }

         public IActionResult Create() {
            ViewData["ClienteNome"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos.Where(x => x.Inserido != true), "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClienteId,ProdutoId")] Listas listas) {
            if (ModelState.IsValid) {
                _context.Add(listas);
                await _context.SaveChangesAsync();
                ProdutoInserido(listas, 1);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteNome"] = new SelectList(_context.Clientes, "Id", "Nome", listas.ClienteId);
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos, "Id", "Nome", listas.ProdutoId);
            return View(listas);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var listas = await _context.Listas.FindAsync(id);
            ProdutoInserido(listas, 0);
            if (listas == null) {
                return NotFound();
            }
            ViewData["ClienteNome"] = new SelectList(_context.Clientes, "Id", "Nome", listas.ClienteId);
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos.Where(x => x.Inserido != true), "Id", "Nome", listas.ProdutoId);
            return View(listas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,ProdutoId")] Listas listas) {
            if (id != listas.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    ProdutoInserido(listas, 1);
                    _context.Update(listas);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!ListasExists(listas.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteNome"] = new SelectList(_context.Clientes, "Id", "Nome", listas.ClienteId);
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos, "Id", "Nome", listas.ProdutoId);
            return View(listas);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var listas = await _context.Listas.Include(l => l.Cliente).Include(l => l.Produto).FirstOrDefaultAsync(m => m.Id == id);
            if (listas == null) {
                return NotFound();
            }

            return View(listas);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            ViewBag.Message = null;
            var listas = await _context.Listas.FindAsync(id);
            if (MinimoCliente(listas.ClienteId) == 1) {
                ViewBag.Message = string.Format("O Cliente precisa ter no minimo um produto");
                var lista = await _context.Listas.Include(l => l.Cliente).Include(l => l.Produto).FirstOrDefaultAsync(m => m.Id == id);
                if (lista == null) {
                    return NotFound();
                }

                return View(lista);
            } else {
                _context.Remove(listas);
                ProdutoInserido(listas, 0);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public int MinimoCliente(int clienteId) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            var result = 0;
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = "select COUNT(*) from Listas where ClienteId = @id;";
                    command.Parameters.AddWithValue("@id", clienteId);
                    result = (int)command.ExecuteScalar();
                    command.Dispose();
                }
            }
            return result;
        }

        private bool ListasExists(int id) {
            return _context.Listas.Any(e => e.Id == id);
        }

        public void InsertInLista(Cliente cliente) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = "insert into Listas (ClienteId, ProdutoId) values (@ClienteId, @ProdutoId)";
                    command.Parameters.AddWithValue("@ClienteId", cliente.Id);
                    command.Parameters.AddWithValue("@ProdutoId", cliente.ProdutoId);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
        }

        public void ProdutoInserido(Listas lista, int i) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = $"update Produtos set Inserido = @i where id = @id;";
                    command.Parameters.AddWithValue("@i", i);
                    command.Parameters.AddWithValue("@id", lista.ProdutoId);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
        }

    }
}
