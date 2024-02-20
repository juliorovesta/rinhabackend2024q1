#!/usr/bin/bash

clear

echo -e '[#] Downing env...\n'

docker compose \
    -p teste-rinhabackend2024q1 \
    -f ./deploy/docker-compose-dev.yml \
    down
