# Deploy

- webapi image

  ```bash
  dotnet watch --project ./src
  ```
  
  ```bash
  docker buildx build \
    --no-cache \
    --platform linux/amd64 \
    -t juliorovesta/rinhabackend2024q1:latest \
    -f src/Dockerfile \
    ./src

  docker buildx build \
    --platform linux/amd64 \
    -t juliorovesta/rinhabackend2024q1:latest \
    -f src/Dockerfile \
    ./src
  ```

- environment docker

  ```bash
  sh ./test/clean-docker.sh
  ```

- environment load test up
  
  ```bash
  docker compose \
    -p teste-rinhabackend2024q1 \
    -f ./deploy/docker-compose-dev.yml \
    up -d
  ```

  ```bash
  docker compose \
    -p teste-rinhabackend2024q1 \
    -f ./deploy/docker-compose-dev.yml \
    down
  ```

- load test run

  ```bash
  sh ./test/executar-teste-local.sh
  ```
