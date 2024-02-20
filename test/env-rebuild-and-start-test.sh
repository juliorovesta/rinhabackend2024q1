#!/usr/bin/bash

./test/env-rebuild.sh

./test/env-up.sh

sleep 1

clear

echo -e '[#] Starting test...\n'

sh ./test/executar-teste-local.sh
