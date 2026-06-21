docker compose -f docker-compose.yml -f docker-compose.override.yml down -v
docker rmi -f association:latest url:latest apigatewayweb:latest
docker system prune -f
