using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoTeste.Models {
    public class Cliente {
        [Key]
        public int Id { get; set; }

        [DisplayName("Nome do Cliente")]
        [Required(ErrorMessage = "Nome obrigatório")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O nome precisa ter mais de 3 caracteres")]
        public string Nome { get; set; }

        [DisplayName("E-mail")]
        [Required(ErrorMessage = "E-mail obrigatório")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [DisplayName("CPF")]
        [Required(ErrorMessage = "CPF obrigatório")]
        [MinLength(11, ErrorMessage = "O CPF deve conter 11 números")]
        public string Cpf { get; set; }

        [DisplayName("Nome do Produto")]
        public Produto Produto { get; set; }
        public int ProdutoId { get; set; }

        public Cliente() {
        }

        public Cliente(int id, string nome, string email, string cpf, Produto produto) {
            Id = id;
            Nome = nome;
            Email = email;
            Cpf = cpf;
            Produto = produto;
        }
    }
}
