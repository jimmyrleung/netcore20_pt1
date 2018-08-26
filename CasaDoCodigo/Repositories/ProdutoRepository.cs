using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CasaDoCodigo.Repositories
{
    public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
    {   
        // Passa para a classe base o contexto necessário
        public ProdutoRepository(ApplicationContext context) : base(context)
        {
        }

        public void SaveProdutos(List<Livro> livros)
        {
            foreach (var livro in livros)
            {
                // Cada tabela é representada por um "Set" do EF
                dbSet.Add(
                    new Produto(livro.Codigo, livro.Nome, livro.Preco)
                );
            }
        }

        public IList<Produto> GetProdutos()
        {
            return dbSet.ToList();
        }

        public bool HasAny()
        {
            return dbSet.Any();
        }
    }

    public class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
