using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace APICatalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _uof;  
        private readonly IMapper _mapper;   
        public ProdutosController (IUnitOfWork context, IMapper mapper)   
        {
            _uof = context;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")] 
        public ActionResult<IEnumerable<Produto>> GetProdutosPrecos() 
        {
            return _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
        }


        [HttpGet] 
        public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters) 
        {
            var produtos = _uof.ProdutoRepository.GetProdutos(produtosParameters); 

            var metadata = new
            {          
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            var produtoDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            if (produtos is null)   
            {
                return NotFound();
            }
            return produtoDto; 
        }



        [HttpGet("{id:int}", Name = "ObterProduto")]   
        public ActionResult<ProdutoDTO> Get(int id) 
        { 

            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id); 
           
            if (produto is null) 
            {
                return NotFound("Produto não encontrado...");
            }

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return produtoDto; 
        }

        [HttpPost] 
        public ActionResult Post(ProdutoDTO produtoDto) 
        {

            var produto = _mapper.Map<Produto>(produtoDto);

            if (produto is null)    
                return BadRequest(); 

            _uof.ProdutoRepository.Add(produto); 
        
            _uof.Commit(); 
            

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return new CreatedAtRouteResult("ObterProduto", new { id = produtoDTO.ProdutoId }, produtoDTO);
            
        }

        [HttpPut("{id:int}")] 
        public ActionResult Put(int id, ProdutoDTO produtoDto) 
        {
            if (id != produtoDto.ProdutoId) 
            {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);  
           
            _uof.Commit(); 
         

            return Ok(produtoDto);
        }

        [HttpDelete("{id:int}")] 
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null) 
            {
                return NotFound("Produto não localizado..."); 
            }
            _uof.ProdutoRepository.Delete(produto); 
      
            _uof.Commit();
                           

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);   

        }

     }


}
