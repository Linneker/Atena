# Migração de funcionários legados — guia

Antes da change `rh-fundacao` (W1), `funcionarios` usava `cargo` e `departamento` como
**string livre** (não FK). A change introduz `cargo_id`, `departamento_id`, `lotacao_id` +
campos RH/Folha. Este documento descreve a estratégia de migração dos registros pré-existentes.

## Migrations envolvidas

1. **`V20260628011_AlterarFuncionariosAdicionarCamposRh`** — adiciona colunas novas (idempotente).
2. **`V20260628012_MigrarFuncionariosLegadosCargoDepto`** — popular FKs a partir dos textos:
   - Para cada `cargo` distinto, cria entrada em `cargos` (descrição = string) e popula `cargo_id`.
   - Para cada `departamento` distinto, cria entrada em `departamentos` (nome = string) e popula `departamento_id`.
   - Funcionários sem cargo/depto recebem entradas "NAO-CLASS" pré-existentes via seed RH.
3. **`V20260628013_CriarUsuariosDesativadosParaFuncionariosLegados`** — funcionários sem `usuario_id`
   ganham usuário desativado com email gerado a partir do CPF (idempotência por CPF/email).
4. **`V20260628014_MarcarCamposObsoletosEmFuncionarios`** — comentário SQL marcando `cargo` e
   `departamento` como `DEPRECATED desde rh-fundacao`. Remoção física planejada para W3.

## Estratégia para ambientes em produção

1. **Backup completo** antes de rodar migrations (snapshot do MySQL).
2. Subir aplicação em **modo manutenção** (Kubernetes: scale to 0, exec migration apartada).
3. Executar `dotnet ef database update` (ou bootstrap via `MigrationRunner` no boot da API).
4. **Verificação** pós-migração:
   ```sql
   SELECT COUNT(*) FROM funcionarios WHERE deleted_at IS NULL;
   -- comparar com snapshot pré-migração
   SELECT COUNT(*) FROM funcionarios WHERE cargo_id IS NULL AND deleted_at IS NULL;
   -- esperar 0 (todos foram migrados para cargo NAO-CLASS ou específico)
   ```
5. Validar que `escalas_funcionario`, `historico_salarios`, etc. estão **vazias** para os legados
   (eles não tinham essas tabelas). UI de "Registrar reajuste" será o ponto de entrada para
   começar a popular o histórico salarial dos legados.

## Coexistência com o cadastro antigo

- A rota `/cadastros/funcionarios` (frontend Cadastros) **continua disponível** e usa o mesmo
  endpoint `/api/v1/funcionarios` (não foi removido).
- A rota nova `/rh/funcionarios` (frontend RH) usa o mesmo backend para listagem, mas o wizard
  de **criação** usa `POST /api/v1/rh/funcionarios` (criação completa atômica).
- **Redirect** de `/cadastros/funcionarios → /rh/funcionarios` planejado para W2 atrás de
  feature flag `rh.unifyFuncionariosUi`.
- Os campos `cargo` e `departamento` (texto livre) **permanecem populados** para registros
  legados — são lidos pela API mas marcados como `[Obsolete]` no DTO. Novas inserções via wizard
  usam apenas `cargo_id`/`departamento_id`.

## Rollback

Cada migration tem `Down()` implementado:
- `MarcarCamposObsoletos` → remove comentário SQL
- `CriarUsuariosDesativados` → não rollback (preservativo; usuários podem permanecer)
- `MigrarFuncionariosLegados` → não desfaz; rollback significaria perder vínculos cargo_id/depto_id
- `AlterarFuncionariosAdicionarCamposRh` → `DROP COLUMN` para cada coluna nova

Para rollback **completo** (não recomendado em produção):
```bash
dotnet ef database update V20260628010_AddTabelaCbosCatalogoNacional --project src/Data/Acme.Sistemas.Infrastructure
```

## Testes que cobrem a migração

- `MigrationsRhFundacaoTests` (integration) — roda migrations em MySQL real, verifica idempotência
- `ConstraintsRhFundacaoTests` (integration) — valida UNIQUE keys, FKs, NOT NULLs
- `SeedRhDefaultsTests` — verifica que seed-tenant cria role RH + defaults NAO-CLASS

## Métrica de saúde

Painel pós-migração (a ser adicionado no dashboard de Operações em W2):
- `% funcionários com cargo_id != NULL` (meta: 100%)
- `% funcionários com historico_salarios não vazio` (esperado: 0% para legados, ~100% para novos)
- `% funcionários com usuário ativo associado` (depende do contexto do tenant)
