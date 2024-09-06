# Etapa 1: Construcción
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia los archivos .csproj y restaura las dependencias
COPY ["src/UserManagement.Api/UserManagement.Api.csproj", "UserManagement.Api/"]
COPY ["src/UserManagement.Application/UserManagement.Application.csproj", "UserManagement.Application/"]
COPY ["src/UserManagement.Domain/UserManagement.Domain.csproj", "UserManagement.Domain/"]
COPY ["src/UserManagement.Infrastructure/UserManagement.Infrastructure.csproj", "UserManagement.Infrastructure/"]

# Restaura las dependencias del proyecto
RUN dotnet restore "UserManagement.Api/UserManagement.Api.csproj"

# Copia el resto del código fuente y construye la aplicación
COPY src/ . 
WORKDIR "/src/UserManagement.Api"
RUN dotnet build "UserManagement.Api.csproj" -c Release -o /app/build

# Etapa 2: Publicación
FROM build AS publish
RUN dotnet publish "UserManagement.Api.csproj" -c Release -o /app/publish

# Etapa 3: Ejecutar
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
RUN apt-get update && apt-get install -y curl
ENTRYPOINT ["dotnet", "UserManagement.Api.dll"]
