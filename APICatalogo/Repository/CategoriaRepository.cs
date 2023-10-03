using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        protected AppDbContext _context;  
        public CategoriaRepository(AppDbContext contexto) : base(contexto) 
        {
            _context = contexto;
        }

        public IEnumerable<Categoria> GetCategoriasProdutos()
        {
            return Get().Include(x => x.Produtos);  
        }
    }
}
