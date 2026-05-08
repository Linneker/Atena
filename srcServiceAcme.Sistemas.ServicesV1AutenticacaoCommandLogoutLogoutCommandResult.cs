namespace ;

// Marker — este Command/Query não tem payload de resposta com nome próprio.
// Convenção do blueprint exige um arquivo *Result.cs por funcionalidade; o tipo de
// resposta efetivo é declarado no Command/Query (em geral ResponseDefault sem genérico,
// ou um tipo externo como BalancoResult/DREResult em Domain/Reports).
