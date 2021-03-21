#!/usr/bin/env bash

PLATFORM_ROOT="$(dirname "$(dirname "$(readlink -fm "$0")")")"

# shellcheck disable=SC2046
# shellcheck disable=SC2196
export $(egrep -v '^#' "$PLATFORM_ROOT"/.env | xargs)

pushd "$PLATFORM_ROOT"/RankCalculator > /dev/null || exit

./bin/run-build.sh
docker-compose build

docker-compose run \
  --rm \
  rank-calculator-app

popd > /dev/null || exit 