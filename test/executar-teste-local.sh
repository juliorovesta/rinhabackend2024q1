#!/usr/bin/bash

# Use este script para executar testes locais

GATLING_WORKSPACE="$(pwd)/test/load-test/user-files"
GATLING_BIN_DIR=$GATLING_HOME/bin
# GATLING_BIN_DIR=$HOME/gatling/3.10.3/bin

runGatling() {
    sh $GATLING_BIN_DIR/gatling.sh -rm local -s RinhaBackendCrebitosSimulation \
        -rd "Rinha de Backend - 2024/Q1: Crébito" \
        -rf $GATLING_WORKSPACE/results \
        -sf $GATLING_WORKSPACE/simulations
}

startTest() {
    for i in {1..20}; do
        # 2 requests to wake the 2 api instances up :)
        curl --fail http://localhost:9999/clientes/1/extrato && \
        echo "" && \
        curl --fail http://localhost:9999/clientes/1/extrato && \
        echo "" && \
        runGatling && \
        break || sleep 2;
    done
}

startTest
