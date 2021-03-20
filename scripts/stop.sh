#!/usr/bin/env bash

PLATFORM_ROOT="$(dirname "$(dirname "$(readlink -fm "$0")")")"

pushd "$PLATFORM_ROOT" > /dev/null || exit

docker-compose down

popd > /dev/null || exit 