using System;

namespace Data
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
        }
    }
}
