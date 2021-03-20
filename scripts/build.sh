#!/usr/bin/env bash

PLATFORM_ROOT="$(dirname "$(dirname "$(readlink -fm "$0")")")"

APP_NAMES=(Valuator RankCalculator EventsLogger) 

echo_call() {
  echo "$@"
  $"$@"
}

for app in ${APP_NAMES[*]} ; do
    echo_call "$PLATFORM_ROOT"/"$app"/bin/run-build.sh || exit
done