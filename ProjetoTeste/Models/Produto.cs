using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models {
    public class Produto {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O nome precisa ter mais de 3 caracteres")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public bool Inserido { get; set; }

        public Produto() {
        }

        public Produto(int id, string nome, bool ativo) {
            Id = id;
            Nome = nome;
            Ativo = ativo;
        }
    }
}
