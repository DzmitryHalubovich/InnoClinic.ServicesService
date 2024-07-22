#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services.API/Services.API.csproj", "src/Services.API/"]
COPY ["src/Services.Contracts/Services.Contracts.csproj", "src/Services.Contracts/"]
COPY ["src/Services.Domain/Services.Domain.csproj", "src/Services.Domain/"]
COPY ["src/Services.Infrastructure/Services.Infrastructure.csproj", "src/Services.Infrastructure/"]
COPY ["src/Services.Presentation/Services.Presentation.csproj", "src/Services.Presentation/"]
COPY ["src/Services.Services.Abstractions/Services.Services.Abstractions.csproj", "src/Services.Services.Abstractions/"]
COPY ["src/Services.Services/Services.Services.csproj", "src/Services.Services/"]
RUN dotnet restore "./src/Services.API/Services.API.csproj"
COPY . .
WORKDIR "/src/src/Services.API"
RUN dotnet build "./Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Services.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.API.dll"]