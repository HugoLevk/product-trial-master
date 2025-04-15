# Variables
DOCKER_COMPOSE = docker-compose
DOTNET = dotnet
SOLUTION = back/AltenTest.sln
DB_CONTAINER = alten-test-db
DB_PORT = 1433
DB_PASSWORD = YourStrong!Password

# Commandes de base Docker
.PHONY: up
up:
	$(DOCKER_COMPOSE) up -d

.PHONY: down
down:
	$(DOCKER_COMPOSE) down

.PHONY: restart
restart: down up

# Commandes avec les logs
.PHONY: logs
logs:
	$(DOCKER_COMPOSE) logs -f

.PHONY: logs-api
logs-api:
	$(DOCKER_COMPOSE) logs -f api

.PHONY: logs-frontend
logs-frontend:
	$(DOCKER_COMPOSE) logs -f frontend

.PHONY: logs-db
logs-db:
	$(DOCKER_COMPOSE) logs -f db

# Commandes de build
.PHONY: build
build:
	$(DOTNET) build $(SOLUTION)
	$(DOCKER_COMPOSE) build

.PHONY: rebuild
rebuild:
	$(DOTNET) clean $(SOLUTION)
	$(DOCKER_COMPOSE) build --no-cache

# Nettoyage
.PHONY: clean
clean:
	$(DOTNET) clean $(SOLUTION)
	$(DOCKER_COMPOSE) down -v --remove-orphans
	docker system prune -f

# Commandes de développement
.PHONY: dev
dev: up logs

.PHONY: fresh
fresh: clean build up logs

# Aide
.PHONY: help
help:
	@echo "Commandes disponibles:"
	@echo "  make up          - Démarre les conteneurs"
	@echo "  make down        - Arrête les conteneurs"
	@echo "  make restart     - Redémarre les conteneurs"
	@echo "  make logs        - Affiche les logs de tous les conteneurs"
	@echo "  make logs-api    - Affiche les logs de l'API"
	@echo "  make logs-frontend - Affiche les logs du frontend Angular"
	@echo "  make logs-db     - Affiche les logs de la base de données"
	@echo "  make build       - Build la solution .NET et les images Docker"
	@echo "  make rebuild     - Clean, rebuild la solution et les images sans cache"
	@echo "  make clean       - Nettoie tout (solution, conteneurs, volumes)"
	@echo "  make dev         - Démarre en mode développement"
	@echo "  make fresh       - Clean, rebuild et redémarre tout"

# Commande par défaut
.DEFAULT_GOAL := help