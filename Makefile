.PHONY: up
up:
	cd scripts && ./start.sh

.PHONY: down
down:
	cd scripts && ./stop.sh

.PHONY: build
build:
	docker-compose build
