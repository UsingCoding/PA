#!/usr/bin/env bash

# shellcheck disable=SC2046
# shellcheck disable=SC2196
export $(egrep -v '^#' ../.env | xargs)

pushd ../RankCalculator > /dev/null || exit

./bin/run-build.sh
docker-compose build

docker-compose run \
  --rm \
  rank-calculator-app

popd > /dev/null || exit 