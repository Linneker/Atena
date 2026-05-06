namespace Acme.Sistemas.Core.Erros;

public static class MessageErros
{
    public const string CampoObrigatorio = "O campo '{0}' é obrigatório.";
    public const string CampoInvalido = "O campo '{0}' está inválido.";
    public const string RegistroNaoEncontrado = "Registro não encontrado.";
    public const string RegistroDuplicado = "Já existe um registro com este identificador.";
    public const string OperacaoNaoAutorizada = "Operação não autorizada.";
    public const string TenantInvalido = "Tenant inválido ou inativo.";
    public const string CnpjInvalido = "CNPJ inválido.";
    public const string CpfInvalido = "CPF inválido.";
    public const string CredenciaisInvalidas = "Usuário ou senha incorretos.";
    public const string ContaBloqueada = "Conta temporariamente bloqueada por excesso de tentativas.";
    public const string EmailNaoConfirmado = "E-mail ainda não confirmado. Verifique sua caixa de entrada.";
    public const string TokenConfirmacaoInvalido = "Token de confirmação inválido ou expirado.";
    public const string TokenExpirado = "Token expirado.";
    public const string TokenInvalido = "Token inválido.";
    public const string LimitePlanoAtingido = "Limite do plano atingido. Faça upgrade do plano.";
    public const string SemPermissao = "Você não tem permissão para executar esta ação.";
    public const string ErroInterno = "Ocorreu um erro interno. Tente novamente mais tarde.";
}
