#!/usr/bin/bash

./test/env-down.sh

./test/env-clear.sh

echo -e '[#] Rebuilding env...\n'

docker buildx build \
    --platform linux/amd64 \
    -t juliorovesta/rinhabackend2024q1:latest \
    -f src/Dockerfile \
    ./src

./test/env-clear.sh

