using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ApplicationContext _context;
        private DbSet<Produto> dbProduto;

        public ProdutoRepository(ApplicationContext context)
        {
            this._context = context;
            dbProduto = _context.Set<Produto>();
        }

        public void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                // Cada tabela é representada por um "Set" do EF
                dbProduto.Add(
                    new Produto(livro.Codigo, livro.Nome, livro.Preco)
                );
            }
        }

        public IList<Produto> GetProdutos()
        {
            return dbProduto.ToList();
        }

        public bool HasAny()
        {
            return dbProduto.Any();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
