using AutoMapper;
using CesarDev.Api.ViewModels;
using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace CesarDev.Api.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {
        //Validacao de notificação de erro
        //Validacao de modelstate
        //Validacao dde operacao de negocios
    }
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IFornecedorService _fornecedorService;

        public FornecedoresController(IMapper mapper, IFornecedorRepository fornecedorRepository, IFornecedorService fornecedorService)
        {
            _mapper = mapper;
            _fornecedorRepository = fornecedorRepository;
            _fornecedorService = fornecedorService;
        }

        [HttpGet]
        public async Task<IEnumerable<FornecedorViewModel>> ObterTodos()
        {
            var fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());
            return fornecedores;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> ObterPorId(Guid id)
        {
            FornecedorViewModel fornecedor = await ObterFornecedorProdutosEndereco(id);

            if (fornecedor == null) return NotFound();

            return fornecedor;
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorViewModel>> Adicionar(FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            Fornecedor fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            bool resultado = await _fornecedorService.Adicionar(fornecedor);

            if (!resultado) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            Fornecedor fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            bool resultado = await _fornecedorService.Atualizar(fornecedor);

            if (!resultado) return BadRequest();

            return Ok(fornecedor);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedorEndereco(id);
            if (fornecedor == null) return NotFound(); 

            bool resultado = await _fornecedorService.Remover(fornecedor.Id);

            if (!resultado) return BadRequest();

            return Ok(fornecedor);
        }

        public async Task<FornecedorViewModel> ObterFornecedorProdutosEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorProdutosEndereco(id));
            return fornecedor;
        }

        public async Task<FornecedorViewModel> ObterFornecedorEndereco(Guid id)
        {
            var fornecedor = _mapper.Map<FornecedorViewModel>(await _fornecedorRepository.ObterFornecedorEndereco(id));
            return fornecedor;
        }
    }
}
