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
    public class EnderecoRepository : Repository<Endereco>, IEnderecoRepositoy
    {
        public EnderecoRepository(DevDbContext context) : base(context) { }
        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            return await Db.Enderecos.AsNoTracking().FirstOrDefaultAsync(f => f.FornecedorId == fornecedorId);
        }
    }
}
