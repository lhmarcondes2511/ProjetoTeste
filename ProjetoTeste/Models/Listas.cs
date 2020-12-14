using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models {
    public class Listas {
        [Key]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public int ProdutoId { get; set; }
        public Produto Produto { get; set; }

        public Listas() {
        }

        public Listas(int id, int clienteId, int produtoId) {
            Id = id;
            ClienteId = clienteId;
            ProdutoId = produtoId;
        }
    }
}
