# Padrão de Testes e Harness - SprintUp

## Ferramentas
- **Framework:** xUnit
- **Asserções:** FluentAssertions
- **Mocks:** NSubstitute ou Moq

## Estrutura do Harness
A pasta `tests/` deve espelhar exatamente a estrutura de `src/`.

## Estratégia de Testes
1. **Testes de Domínio (Unitários):** Toda regra matemática, validação ou estado dentro das entidades em `Domain/` deve ser testada de forma isolada, sem mocks.
2. **Testes de Feature (Integração Focada):** Testar o `Handler` isolando a infraestrutura. Mockar as interfaces de Repositório (`IRepository`) e APIs externas.
3. **Gatilho do Harness:** O comando oficial para validar o código é `make harness`. A IA deve rodar este comando após qualquer alteração de código.