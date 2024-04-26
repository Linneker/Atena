using acme.atena.application.Handler.Produtos.Commands.Response;
using acme.atena.domain.DTO.Product;
using acme.atena.domain.Interface.Repository.Product;
using acme.atena.domain.Interface.Service.Product.Price;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace acme.atena.application.Handler.Produtos.Query
{
    public class ProtudoQuery
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IValorProdutoService _valorProdutoService;
        public ProtudoQuery(IProdutoRepository produtoRepository, IValorProdutoService valorProdutoService)
        {
            _produtoRepository = produtoRepository;
            _valorProdutoService = valorProdutoService;
        }

        public async Task<OutputProdutoCommand> ObterProdutoPeloId(Guid id)
        {
            var produto =await  _produtoRepository.ObterProdutosPeloId(id);
            var outputProdutoCommands = ToOutputProdutoCommand(produto);
            return outputProdutoCommands;
        }
        public async Task<List<OutputProdutoCommand>> ObterProdutosJoinTipoProduto()
        {
            List<OutputProdutoCommand> outputProdutoCommands = new List<OutputProdutoCommand>();
            var produtos = await _produtoRepository.ObterProdutosJoinTipoProduto();
            produtos.ForEach(produto =>
            {
                outputProdutoCommands.Add(ToOutputProdutoCommand(produto));
            });
            return outputProdutoCommands;
        }

        private OutputProdutoCommand ToOutputProdutoCommand(Produto produto) {
            OutputProdutoCommand outputProdutoCommand = new OutputProdutoCommand();
            outputProdutoCommand.TipoProduto = produto.TipoProduto.Nome;
            outputProdutoCommand.Nome = produto.Nome;
            outputProdutoCommand.Descricao = produto.Descricao;
            outputProdutoCommand.Validade = produto.Validade;
            outputProdutoCommand.Codigo = produto.Codigo;
            outputProdutoCommand.Id = produto.Id;
            outputProdutoCommand.Status = produto.Status;
            outputProdutoCommand.DataCriacao = produto.DataCriacao;
            outputProdutoCommand.DataModificacao = produto.DataModificacao;
            outputProdutoCommand.DataInativacao = produto.DataInativacao;
            outputProdutoCommand.UsuarioCriacao = produto.UsuarioCriacao;
            outputProdutoCommand.UsuarioModificacao = produto.UsuarioModificacao;
            return outputProdutoCommand;
        }

    }
}
