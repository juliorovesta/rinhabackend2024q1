#!/usr/bin/bash

echo -e '[#] Cleaning env...\n'

docker container prune -f
docker image prune -f
docker volume prune -f
docker network prune -f
