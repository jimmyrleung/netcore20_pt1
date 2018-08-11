using CasaDoCodigo.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>().HasKey(p => p.Id);

            modelBuilder.Entity<Pedido>().HasKey(p => p.Id);

            //Pedido 1:n ItemPedido
            modelBuilder.Entity<Pedido>()
                .HasMany(t => t.Itens) // Um pedido possui 'n' itens de pedido - Ida
                .WithOne(p => p.Pedido); // Um ItemPedido tem 1 Pedido - Volta

            //Pedido 1:1 Cadastro
            modelBuilder.Entity<Pedido>()
                .HasOne(t => t.Cadastro) // Um Pedido possui um Cadastro - Ida
                .WithOne(p => p.Pedido).HasForeignKey<Cadastro>(t => t.Id).IsRequired(); // Mapeando Foreign Key com a tabela cadastro

            modelBuilder.Entity<ItemPedido>().HasKey(p => p.Id);
            modelBuilder.Entity<ItemPedido>().HasOne(p => p.Pedido); // ItemPedido tem um Pedido
            modelBuilder.Entity<ItemPedido>().HasOne(p => p.Produto); // ItemPedido tem um Produto

            modelBuilder.Entity<Cadastro>().HasKey(p => p.Id);
            modelBuilder.Entity<Cadastro>().HasOne(p => p.Pedido); // Um cadastro tem um pedido
        }
    }
}
