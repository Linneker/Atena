namespace Acme.Sistemas.Core.Mediators;

public interface IAuditable
{
    string Recurso { get; }
    string Acao { get; }
}
