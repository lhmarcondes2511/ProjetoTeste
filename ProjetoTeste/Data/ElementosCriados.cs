using ProjetoTeste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Data {
    public class ElementosCriados {
        private LksMarcondesDbContext _context;

        public ElementosCriados(LksMarcondesDbContext context) {
            _context = context;
        }

        public void Criado() {
            if (_context.Produtos.Any() || _context.Clientes.Any()) {
                return;
            }

            Produto prod1 = new Produto(1, "Produto 1", false);
            Produto prod2 = new Produto(2, "Produto 2", true);
            Produto prod3 = new Produto(3, "Produto 3", false);
            Produto prod4 = new Produto(4, "Produto 4", false);
            Produto prod5 = new Produto(5, "Produto 5", true);
            Produto prod6 = new Produto(6, "Produto 6", false);
            Produto prod7 = new Produto(7, "Produto 7", true);
            Produto prod8 = new Produto(8, "Produto 8", true);
            Produto prod9 = new Produto(9, "Produto 9", true);
            Produto prod10 = new Produto(10, "Produto 10", false);

            Cliente cli1 = new Cliente(1, "Lucas Henrique", "lucashenrique@gmail.com", "060.352.134-90", prod2);
            Cliente cli2 = new Cliente(2, "Matheus Douglas", "matheusdouglas@gmail.com", "457.321.746-03", prod6);
            Cliente cli3 = new Cliente(3, "Esther Fernanda", "estherfernanda@gmail.com", "743.759.142-58", prod10);
            Cliente cli4 = new Cliente(4, "Debora Lais", "deboralais@gmail.com", "415.689.243-09", prod1);
            Cliente cli5 = new Cliente(5, "João Kennedy", "joaokennedy@gmail.com", "824.713.464-35", prod5);
            Cliente cli6 = new Cliente(6, "Zenilda Amaral", "zenildaamaral@gmail.com", "474.234.655-33", prod8);
            Cliente cli7 = new Cliente(7, "Amanda Arguleho", "amanadaarguelho@gmail.com", "189.664.246-76", prod4);

            Listas comp1 = new Listas(1, 1, 2);
            Listas comp2 = new Listas(2, 2, 6);
            Listas comp3 = new Listas(3, 3, 10);
            Listas comp4 = new Listas(4, 4, 1);
            Listas comp5 = new Listas(5, 5, 5);
            Listas comp6 = new Listas(6, 6, 8);
            Listas comp7 = new Listas(7, 7, 4);

            _context.Produtos.AddRange(prod1, prod2, prod3, prod4, prod5, prod6, prod7, prod8, prod9, prod10);
            _context.Clientes.AddRange(cli1, cli2, cli3, cli4, cli5, cli6, cli7);
            _context.Listas.AddRange(comp1, comp2, comp3, comp4, comp5, comp6, comp7);
            _context.SaveChanges();

        }
    }
}
