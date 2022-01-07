# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY ./*.csproj ./deploy/
WORKDIR /source/deploy
RUN dotnet restore

# copy everything else and build app
COPY ./. ./deploy/
WORKDIR /source/deploy
RUN dotnet publish -c Release -o /App --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /App
COPY --from=build /App ./
ENTRYPOINT ["dotnet", "backend.dll"]