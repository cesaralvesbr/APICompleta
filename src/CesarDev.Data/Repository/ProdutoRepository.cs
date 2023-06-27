using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using CesarDev.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CesarDev.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DevDbContext context) : base(context) { }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            // Vai na Tabela Produtos e faz um Join com a Tabela Fornecedor, onde recupera o Produto com base no ID
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await Db.Produtos.AsNoTracking().Include(f => f.Fornecedor).OrderBy(p => p.Nome)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            //return await Db.Produtos.AsNoTracking().Include(f=>f.Fornecedor).Where(p=> p.Id == fornecedorId).ToListAsync();
            return await Buscar(p => p.FornecedorId == fornecedorId);
        }
    }
}
