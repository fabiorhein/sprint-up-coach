# Makefile - Harness de Automação para SprintUp
# Simplifica comandos complexos do .NET para agentes de IA e desenvolvedores

.PHONY: help build harness test migrate clean restore run docker-up docker-down

# Variáveis
DOTNET := dotnet
SRC_PROJECT := src/SprintUp.csproj
TEST_PROJECT := tests/SprintUp.Tests.csproj
MIGRATIONS_CONTEXT := SprintUpDbContext

# Cores para output (opcional, melhora legibilidade)
GREEN := \033[0;32m
BLUE := \033[0;34m
NC := \033[0m # No Color

## help: Mostra todos os comandos disponíveis
help:
	@echo "$(BLUE)=== SprintUp Makefile ===$(NC)"
	@echo ""
	@echo "Comandos disponíveis:"
	@echo "  $(GREEN)make help$(NC)           - Mostra este menu"
	@echo "  $(GREEN)make restore$(NC)        - Restaura dependências NuGet"
	@echo "  $(GREEN)make build$(NC)          - Compila o projeto"
	@echo "  $(GREEN)make test$(NC)           - Executa testes unitários"
	@echo "  $(GREEN)make harness$(NC)        - Executa o Harness (testes + validação)"
	@echo "  $(GREEN)make run$(NC)            - Inicia a API localmente"
	@echo "  $(GREEN)make migrate$(NC)        - Executa migrações do banco de dados"
	@echo "  $(GREEN)make clean$(NC)          - Remove builds e artifacts"
	@echo "  $(GREEN)make docker-up$(NC)      - Inicia containers (PostgreSQL, etc)"
	@echo "  $(GREEN)make docker-down$(NC)    - Para containers"
	@echo ""

## restore: Restaura dependências NuGet (necessário após git clone)
restore:
	@echo "$(BLUE)[RESTORE] Restaurando dependências...$(NC)"
	$(DOTNET) restore $(SRC_PROJECT)
	$(DOTNET) restore $(TEST_PROJECT)
	@echo "$(GREEN)✓ Dependências restauradas$(NC)"

## build: Compila o projeto
build: restore
	@echo "$(BLUE)[BUILD] Compilando projeto...$(NC)"
	$(DOTNET) build $(SRC_PROJECT) -c Release
	@echo "$(GREEN)✓ Build concluído$(NC)"

## test: Executa testes unitários
test:
	@echo "$(BLUE)[TEST] Executando testes...$(NC)"
	$(DOTNET) test $(TEST_PROJECT) --no-build --logger "console;verbosity=normal"

## harness: O COMANDO PRINCIPAL - Valida TUDO (Build + Testes)
## Este é o comando que o Aider vai rodar após implementar uma spec
harness: build test
	@echo ""
	@echo "$(GREEN)✓✓✓ HARNESS PASSOU ✓✓✓$(NC)"
	@echo "$(GREEN)Implementação validada com sucesso!$(NC)"
	@echo ""

## clean: Remove builds, binaries e artifacts
clean:
	@echo "$(BLUE)[CLEAN] Limpando artifacts...$(NC)"
	$(DOTNET) clean $(SRC_PROJECT)
	$(DOTNET) clean $(TEST_PROJECT)
	find . -type d -name "bin" -o -name "obj" | xargs rm -rf
	@echo "$(GREEN)✓ Limpeza concluída$(NC)"

## migrate: Cria e aplica migrações do EF Core
migrate:
	@echo "$(BLUE)[MIGRATE] Executando migrações...$(NC)"
	$(DOTNET) ef database update --project $(SRC_PROJECT) --context $(MIGRATIONS_CONTEXT)
	@echo "$(GREEN)✓ Migrações aplicadas$(NC)"

## migrate-add: Cria uma nova migration (uso: make migrate-add NAME="NomeDaMigracao")
migrate-add:
	@if [ -z "$(NAME)" ]; then \
		echo "$(GREEN)Uso: make migrate-add NAME=\"SuaMigracao\"$(NC)"; \
		exit 1; \
	fi
	@echo "$(BLUE)[MIGRATE] Criando migration: $(NAME)...$(NC)"
	$(DOTNET) ef migrations add $(NAME) --project $(SRC_PROJECT) --context $(MIGRATIONS_CONTEXT)
	@echo "$(GREEN)✓ Migration criada$(NC)"

## run: Inicia a aplicação localmente (requer PostgreSQL rodando)
run: build migrate
	@echo "$(BLUE)[RUN] Iniciando SprintUp API...$(NC)"
	$(DOTNET) run --project $(SRC_PROJECT) --no-build

## docker-up: Inicia container do PostgreSQL (Docker Compose)
docker-up:
	@echo "$(BLUE)[DOCKER] Iniciando containers...$(NC)"
	docker-compose up -d
	@echo "$(GREEN)✓ Containers iniciados$(NC)"
	@echo "$(BLUE)PostgreSQL disponível em: localhost:5432$(NC)"

## docker-down: Para containers
docker-down:
	@echo "$(BLUE)[DOCKER] Parando containers...$(NC)"
	docker-compose down
	@echo "$(GREEN)✓ Containers parados$(NC)"

## docker-logs: Mostra logs dos containers
docker-logs:
	docker-compose logs -f

## watch: Monitora mudanças e executa testes automaticamente (requer dotnet-watch)
watch:
	@echo "$(BLUE)[WATCH] Monitorando mudanças... (Ctrl+C para parar)$(NC)"
	$(DOTNET) watch --project $(TEST_PROJECT) test

## format: Formata código C# com padrões do projeto
format:
	@echo "$(BLUE)[FORMAT] Formatando código...$(NC)"
	$(DOTNET) format $(SRC_PROJECT)
	@echo "$(GREEN)✓ Código formatado$(NC)"

## info: Mostra informações do projeto
info:
	@echo "$(BLUE)=== Informações do Projeto ===$(NC)"
	@echo "Projeto Principal: $(SRC_PROJECT)"
	@echo "Projeto de Testes: $(TEST_PROJECT)"
	@echo "DbContext: $(MIGRATIONS_CONTEXT)"
	@echo ""
	@$(DOTNET) --version
	@echo ""

# Comando padrão (quando você só digita 'make')
.DEFAULT_GOAL := help