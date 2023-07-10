using AutoMapper;
using CesarDev.Api.ViewModels;
using CesarDev.Business.Interfaces;
using CesarDev.Business.Models;
using Microsoft.AspNetCore.Mvc;

namespace CesarDev.Api.Controllers
{
    [Route("api/fornecedores")]
    public class FornecedoresController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IEnderecoRepositoy _enderecoRepositoy;
        private readonly IFornecedorService _fornecedorService;

        public FornecedoresController(
            IMapper mapper,
            INotificador notificador,
            IFornecedorService fornecedorService,
            IFornecedorRepository fornecedorRepository,
            IEnderecoRepositoy enderecoRepositoy) : base(notificador)
        {
            _mapper = mapper;
            _fornecedorService = fornecedorService;
            _fornecedorRepository = fornecedorRepository;
            _enderecoRepositoy = enderecoRepositoy;
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
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            Fornecedor fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Adicionar(fornecedor);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Atualizar(Guid id, FornecedorViewModel fornecedorViewModel)
        {
            if (id != fornecedorViewModel.Id) return BadRequest();

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            Fornecedor fornecedor = _mapper.Map<Fornecedor>(fornecedorViewModel);
            await _fornecedorService.Atualizar(fornecedor);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<FornecedorViewModel>> Excluir(Guid id)
        {
            var fornecedorViewModel = await ObterFornecedorEndereco(id);
            if (fornecedorViewModel == null) return NotFound();

            await _fornecedorService.Remover(fornecedorViewModel.Id);

            return CustomResponse(fornecedorViewModel);
        }

        [HttpGet("obter-endereco/{id:guid}")]
        public async Task<ActionResult<EnderecoViewModel>> ObterEnderecoPorId(Guid id)
        {
            EnderecoViewModel endereco = await ObterEndereco(id);

            if (endereco == null) return NotFound();

            return endereco;
        }

        [HttpPut("atualizar-endereco/{id:guid}")]
        public async Task<IActionResult> AtualizarEndereco(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (enderecoViewModel.Id != id) return NotFound();

            if (!ModelState.IsValid) return CustomResponse();

            await _fornecedorService.AtualizarEndereco(_mapper.Map<Endereco>(enderecoViewModel));
            return CustomResponse(enderecoViewModel);
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
        public async Task<EnderecoViewModel> ObterEndereco(Guid id)
        {
            var endereco = _mapper.Map<EnderecoViewModel>(await _enderecoRepositoy.ObterPorId(id));
            return endereco;
        }
    }
}
