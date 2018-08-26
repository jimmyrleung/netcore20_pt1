using CasaDoCodigo.Models;
using Microsoft.EntityFrameworkCore;

namespace CasaDoCodigo.Repositories
{
    // Base repository para uma entidade genérica T que seja do tipo BaseModel
    // Ou seja, qualquer classe que implementar/herdar BaseModel poderá herdar de BaseRepository
    public class BaseRepository<T> where T : BaseModel
    {
        // Protected pois devem ser visíveis para quem herdar de BaseRepository
        protected readonly ApplicationContext _context;
        protected DbSet<T> dbSet;

        public BaseRepository(ApplicationContext context)
        {
            this._context = context;
            dbSet = _context.Set<T>();
        }
    }
}
