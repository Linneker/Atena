namespace Acme.Sistemas.Core.Const;

public static class Permissions
{
    public static class Acoes
    {
        public const string Ler = "ler";
        public const string Criar = "criar";
        public const string Editar = "editar";
        public const string Excluir = "excluir";
        public const string Aprovar = "aprovar";
        public const string Faturar = "faturar";
        public const string Cancelar = "cancelar";
        public const string Exportar = "exportar";
    }

    public static class Recursos
    {
        public const string Tenant = "tenant";
        public const string Usuario = "usuario";
        public const string Role = "role";
        public const string Permissao = "permissao";
        public const string Empresa = "empresa";
        public const string Cliente = "cliente";
        public const string Fornecedor = "fornecedor";
        public const string Funcionario = "funcionario";
        public const string Produto = "produto";
        public const string TipoProduto = "tipo-produto";
        public const string Estoque = "estoque";
        public const string Inventario = "inventario";
        public const string Despesa = "despesa";
        public const string Receita = "receita";
        public const string ContaPagar = "conta-pagar";
        public const string ContaReceber = "conta-receber";
        public const string FluxoDeCaixa = "fluxo-de-caixa";
        public const string ConciliacaoBancaria = "conciliacao-bancaria";
        public const string PlanoDeContas = "plano-de-contas";
        public const string CentroDeCusto = "centro-de-custo";
        public const string SolicitacaoCompra = "solicitacao-compra";
        public const string PedidoCompra = "pedido-compra";
        public const string Recebimento = "recebimento";
        public const string Orcamento = "orcamento";
        public const string PedidoVenda = "pedido-venda";
        public const string Faturamento = "faturamento";
        public const string Devolucao = "devolucao";
        public const string NFe = "nfe";
        public const string ConfiguracaoFiscal = "configuracao-fiscal";
        public const string Relatorio = "relatorio";
        public const string Dashboard = "dashboard";
        public const string Auditoria = "auditoria";
        public const string ApiKey = "api-key";
        public const string FeatureFlags = "feature-flags";
    }

    public static string Of(string recurso, string acao) => $"{recurso}:{acao}";

    public static IReadOnlyList<string> All()
    {
        var recursos = typeof(Recursos)
            .GetFields()
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue()!);

        var acoes = typeof(Acoes)
            .GetFields()
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => (string)f.GetRawConstantValue()!);

        return recursos.SelectMany(r => acoes.Select(a => Of(r, a))).ToList();
    }
}
