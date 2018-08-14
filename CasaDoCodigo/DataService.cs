using CasaDoCodigo.Models;
using CasaDoCodigo.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace CasaDoCodigo
{
    public class DataService : IDataService
    {
        private readonly ApplicationContext _context;
        private readonly IProdutoRepository _produtoRepository;

        // Nosso ApplicationContext vem via injeção de dependência
        // pois foi configurado no ConfigureServices
        public DataService(ApplicationContext context, IProdutoRepository produtoRepository)
        {
            this._context = context;
            this._produtoRepository = produtoRepository;
        }

        public void InicializaDB()
        {
            _context.Database.Migrate();
            if (!_produtoRepository.HasAny())
            {
                List<Livro> livros = GetLivrosFromJSON();
                _produtoRepository.SaveProdutos(livros);
                _context.SaveChanges();
            }
        }

        private static List<Livro> GetLivrosFromJSON()
        {
            var livrosJson = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(livrosJson);
            return livros;
        }
    }
}
