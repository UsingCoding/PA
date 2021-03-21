.PHONY: up
up:
	./scripts/start.sh

.PHONY: down
down:
	./scripts/stop.sh

.PHONY: restart
restart: down up

.PHONY: run-rank-calculator
run-rank-calculator:
	./scripts/rank-calculator-run.sh

.PHONY: build
build:
	docker-compose build

.PHONY: logs
logs:
	docker-compose logs -f