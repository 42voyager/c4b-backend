#!/usr/bin/env bash

# criando os arquivos para o deploy
dotnet publish -c Release

# para o container
docker stop voyager-api-container

# deleta image antiga
docker rmi voyager-api

# criando a imagem
docker build -t voyager-api ../.

# executando a aplicação
docker run -it -p 5000:80 --rm --name voyager-api-container voyager-api
