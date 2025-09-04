MAKEFILE_DIR := $(dir $(abspath $(lastword $(MAKEFILE_LIST))))
DB_DIR       := $(abspath $(MAKEFILE_DIR))
ENV_FILE     := $(firstword $(wildcard $(DB_DIR)/.env) $(wildcard $(DB_DIR)/sample.env))

PROJECT_NAME := whoisdb
COMPOSE      := COMPOSE_PROJECT_NAME=$(PROJECT_NAME) docker compose $(if $(ENV_FILE),--env-file $(ENV_FILE),)

up:
	$(COMPOSE) up -d postgres
	@echo "postgres started"
	@sleep 2

migrate:
	$(COMPOSE) run --rm migrate

postgres: up migrate

down:
	$(COMPOSE) down

clean:
	$(COMPOSE) down -v

status:
	docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" | (head -n 1; grep -E "$(PROJECT_NAME)|postgres" || true)
