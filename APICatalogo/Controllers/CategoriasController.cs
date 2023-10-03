using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;   
        private readonly ILogger _logger;


        public CategoriasController(AppDbContext context, ILogger<CategoriasController> logger)  
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("produtos")] 
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos() 
        {
            
            return _context.Categorias.Include(p => p.Produtos).ToList();  
      
        }

        [HttpGet] 
        public ActionResult<IEnumerable<Categoria>> Get() 
        {
            try
            {
                _logger.LogInformation("========================GET api/categorias =====================================");
                return _context.Categorias.AsNoTracking().ToList();  
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação.");
                
            }

            
        }

        [HttpGet("{id:int}", Name = "ObterCategorias")]   
        public ActionResult<Categoria> Get(int id) 
        { 
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id); 
            
            _logger.LogInformation($"========================GET api/categoria = {id} =====================================");

            if (categoria == null) 
            {
                return NotFound("Categoria não encontrada...");
            }
            return Ok(categoria); 
        }

        [HttpPost] 
        public ActionResult Post(Categoria categoria) 
        {
            if (categoria is null)    
                return BadRequest();

            _context.Categorias.Add(categoria); 
            _context.SaveChanges(); 

            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);

        }

        [HttpPut("{id:int}")] 
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId) 
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;  
            _context.SaveChanges(); 

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")] 
        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.CategoriaId == id); 

            if (categoria is null) 
            {
                return NotFound("Categoria não encontrada...");
            }
            _context.Categorias.Remove(categoria); 
            _context.SaveChanges(); 

            return Ok(categoria);   

        }

    }
}
