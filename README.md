# AppsVendas

Docker
https://docs.docker.com/desktop/windows/install/
Kernel Linux WSL2
https://docs.microsoft.com/pt-br/windows/wsl/install-manual#step-4---download-the-linux-kernel-update-package

Redis
docker run --name apps-redis -p 5002:6379 -d redis

Rabbit
docker run -d --hostname my-rabbit --name apps-rabbit -p 8080:15672 rabbitmq:3-management
