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
    public class ClientesController : Controller {
        private readonly LksMarcondesDbContext _context;

        public ClientesController(LksMarcondesDbContext context) {
            _context = context;
        }

        public async Task<IActionResult> Index() {
            var cliente = _context.Clientes.Include(c => c.Produto);
            return View(await cliente.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) {
                return NotFound();
            }

            return View(cliente);
        }

        public IActionResult Create() {
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos.Where(x => x.Inserido != true), "Id", "Nome");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Cpf,ProdutoId")] Cliente cliente) {
            ViewBag.Message = null;
            if (ModelState.IsValid && TamanhoCpf(cliente.Cpf) && !ProcurarCPF(cliente.Cpf) && !ProcurarEmail(cliente.Email)) {
                _context.Add(cliente);
                ProdutoInserido(cliente, 1);
                await _context.SaveChangesAsync();
                InserirNaLista(cliente);
                return RedirectToAction(nameof(Index));
            } else if (!TamanhoCpf(cliente.Cpf)) {
                ViewBag.Message = string.Format("CPF invalido, verifique se possui letras ou simbolos.");
            } else if (ProcurarCPF(cliente.Cpf) || ProcurarEmail(cliente.Email)) {
                ViewBag.Message = string.Format("CPF ou E-mail ja esta cadastrado em nosso sistema. Tente novamente.");
            }
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos, "Id", "Nome", cliente.ProdutoId);
            return View(cliente);
        }

        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }
            var cliente = await _context.Clientes.FindAsync(id);
            ProdutoInserido(cliente, 0);
            if (cliente == null) {
                return NotFound();
            }
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos.Where(x => x.Inserido != true), "Id", "Nome", cliente.ProdutoId);
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Cpf,ProdutoId")] Cliente cliente) {
            if (id != cliente.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    ProdutoInserido(cliente, 1);
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                } catch (DbUpdateConcurrencyException) {
                    if (!ClienteExists(cliente.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProdutoNome"] = new SelectList(_context.Produtos, "Id", "Nome", cliente.ProdutoId);
            return View(cliente);
        }

        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }
            var cliente = await _context.Clientes.Include(c => c.Produto).FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) {
                return NotFound();
            }

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var cliente = await _context.Clientes.FindAsync(id);
            int IdLista = RemoverDaLista(id);
            Listas lista = await _context.Listas.FindAsync(IdLista);
            _context.Clientes.Remove(cliente);
            _context.Listas.Remove(lista);
            ProdutoInserido(cliente, 0);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public int RemoverDaLista(int id) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = $"select Id from Listas where ClienteId = @id;";
                    command.Parameters.AddWithValue("@id", id);
                    dr = command.ExecuteReader();
                    if (dr.Read()) {
                        Listas lista = new Listas();
                        lista.Id = Convert.ToInt32(dr["Id"]);
                        return lista.Id;
                    } else {
                        return 0;
                    }
                }
            }
        }

        private bool ClienteExists(int id) {
            return _context.Clientes.Any(e => e.Id == id);
        }

        public bool TamanhoCpf(string cpf) {
            var valorSemPontoETraco = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length == 11 && !ContemLetras(valorSemPontoETraco)) {
                return true;
            } else {
                return false;
            }
        }

        public bool ContemLetras(string texto) {
            if (texto.Where(c => char.IsLetter(c)).Count() > 0) {
                return true;
            } else {
                return false;
            }
        }

        public bool ProcurarCPF(string cpf) {
            var valorSemPontoETraco = cpf.Replace(".", "").Replace("-", "");
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            var result = 0;
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = "select COUNT(*) from Clientes where Cpf = @cpf;";
                    command.Parameters.AddWithValue("@cpf", valorSemPontoETraco);
                    result = (int)command.ExecuteScalar();
                    command.Dispose();
                }
            }
            if (result == 1) {
                return true;
            }
            return false;
        }

        public bool ProcurarEmail(string email) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            var result = 0;
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = "select COUNT(*) from Clientes where Email = @email;";
                    command.Parameters.AddWithValue("@email", email);
                    result = (int)command.ExecuteScalar();
                    command.Dispose();
                }
            }
            if (result == 1) {
                return true;
            }
            return false;
        }

        public void InserirNaLista(Cliente cliente) {
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

        public void ProdutoInserido(Cliente cliente, int i) {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=lksmarcondes;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(connection)) {
                conn.Open();
                using (SqlCommand command = new SqlCommand("", conn)) {
                    command.CommandText = $"update Produtos set Inserido = @i where id = @id;";
                    command.Parameters.AddWithValue("@i", i);
                    command.Parameters.AddWithValue("@id", cliente.ProdutoId);
                    command.ExecuteNonQuery();
                    command.Dispose();
                }
            }
        }

    }
}
