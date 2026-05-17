# Arquitetura do Sistema - SprintUp

## Estilo Arquitetural: Monolito Modular (Single Project)
O sistema é composto por módulos independentes encapsulados dentro do mesmo projeto .NET (`src/`).

## Organização de Camadas por Módulo
Cada módulo dentro de `src/Modules/[NomeDoModulo]/` deve conter estritamente:
1. **Domain/**: Entidades ricas, objetos de valor, exceções de domínio e contratos de interfaces. *Dependência Zero*.
2. **Features/**: Casos de uso isolados organizados por pastas. Cada feature contém seu Endpoint (Minimal API), Handler (Lógica de aplicação usando SOLID) e DTOs.
3. **Infrastructure/**: Implementação de repositórios, contextos de banco específicos e clientes de APIs externas (ex: Strava API).

## Regras de Isolamento (Crucial para a IA)
- Um módulo **NÃO** pode injetar classes concretas de outro módulo.
- A comunicação entre módulos diferentes deve ser feita estritamente via **Interfaces de Domínio** ou disparando **Eventos em Memória (MediatR/Médiator)**.
- Se o Módulo A precisar de dados do Módulo B, ele deve depender de uma interface exposta pelo Módulo B.