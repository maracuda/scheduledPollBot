docker build -t counter-image -f Dockerfile .
docker run --env-file EnvironmentVariables counter-image