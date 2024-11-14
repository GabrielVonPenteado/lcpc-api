# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar arquivos do projeto e restaurar dependências
COPY ./*.csproj ./
RUN dotnet restore

# Copiar todo o código-fonte e publicar a aplicação
COPY . ./
RUN dotnet publish -c Release -o /publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copiar os artefatos de build
COPY --from=build /publish .

# Definir variável de ambiente para o ASP.NET Core ouvir na porta definida pelo Heroku
ENV ASPNETCORE_URLS=http://*:$PORT

# Comando para iniciar a aplicação
CMD ["dotnet", "lcpc-api2.dll"]