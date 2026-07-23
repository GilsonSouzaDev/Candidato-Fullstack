# Estágio de Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

# Copia os arquivos csproj e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos e realiza o build
COPY . ./
RUN dotnet publish -c Release -o out

# Estágio de Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "candidato.dll"]
