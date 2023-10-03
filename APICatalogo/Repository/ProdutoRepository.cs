﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository 
    {
        protected AppDbContext _context;  

        public ProdutoRepository(AppDbContext contexto) : base(contexto) 
        {
            _context = contexto;
        }

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters) 
        {
            return PagedList<Produto>.ToPagedList(Get().OrderBy(on => on.ProdutoId),
            produtosParameters.PageNumber, produtosParameters.PageSize);


        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList(); 
        }
    }
}

