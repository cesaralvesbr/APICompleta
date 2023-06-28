using CesarDev.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CesarDev.Business.Interfaces
{
    public interface IFornecedorService : IDisposable
    {
        Task<bool> Adicionar(Fornecedor fornecedor);
        Task<bool> Atualizar(Fornecedor fornecedor);
        Task<bool> Remover(Guid id);
        Task AtualizarEndereco(Endereco endereco);

    }
}
