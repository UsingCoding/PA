.PHONY: up
up:
	./scripts/start.sh

.PHONY: down
down:
	./scripts/stop.sh

.PHONY: build
build:
	docker-compose build

.PHONY: logs
logs:
	docker-compose logs -f