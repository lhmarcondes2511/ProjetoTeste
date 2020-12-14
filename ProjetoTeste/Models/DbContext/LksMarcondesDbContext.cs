using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjetoTeste.Models;

namespace ProjetoTeste.Models {
    public class LksMarcondesDbContext : DbContext {
        public LksMarcondesDbContext(DbContextOptions<LksMarcondesDbContext> options) : base(options) {
        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ProjetoTeste.Models.Listas> Listas { get; set; }

    }
}
