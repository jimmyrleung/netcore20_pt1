using CasaDoCodigo.Models;
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

        // Nosso ApplicationContext vem via injeção de dependência
        // pois foi configurado no ConfigureServices
        public DataService(ApplicationContext context)
        {
            this._context = context;
        }

        public void InicializaDB()
        {
            _context.Database.Migrate();

            var livrosJson = File.ReadAllText("livros.json");
            var livros = JsonConvert.DeserializeObject<List<Livro>>(livrosJson);

            foreach (var livro in livros)
            {
                // Cada tabela é representada por um "Set" do EF
                _context.Set<Produto>().Add(
                    new Produto(livro.Codigo, livro.Nome, livro.Preco)
                );
            }

            _context.SaveChanges();
        }
    }

    class Livro
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
    }
}
