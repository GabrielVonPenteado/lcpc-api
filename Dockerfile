# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos do projeto e restaurar dependências
COPY *.csproj ./
RUN dotnet restore

# Copiar todo o código e publicar a aplicação
COPY . ./
RUN dotnet publish -c Release -o /publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar os artefatos de build
COPY --from=build /publish .

# Expor a porta usada pela aplicação
EXPOSE 8080

# Definir a variável de ambiente para ouvir na porta 8080 (necessária para Heroku)
ENV ASPNETCORE_URLS=http://+:8080

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "lcpc.dll"]