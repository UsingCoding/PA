#!/usr/bin/env bash

APP_DIR="$(dirname "$(dirname "$(readlink -fm "$0")")")"

pushd "$APP_DIR" > /dev/null || exit

dotnet build -o ./bin/EventsLogger

popd > /dev/null || exit 