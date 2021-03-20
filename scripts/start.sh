#!/usr/bin/env bash

PLATFORM_ROOT="$(dirname "$(dirname "$(readlink -fm "$0")")")"

pushd "$PLATFORM_ROOT" > /dev/null || exit

./scripts/build.sh || exit

docker-compose up -d

popd > /dev/null || exit 