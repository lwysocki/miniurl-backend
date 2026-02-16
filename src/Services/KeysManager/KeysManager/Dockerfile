#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/KeysManager/KeysManager/KeysManager.csproj", "src/Services/KeysManager/KeysManager/"]
COPY ["src/Services/KeysManager/KeysManager.Domain/KeysManager.Domain.csproj", "src/Services/KeysManager/KeysManager.Domain/"]
COPY ["src/Services/KeysManager/KeysManager.Infrastructure/KeysManager.Infrastructure.csproj", "src/Services/KeysManager/KeysManager.Infrastructure/"]
RUN dotnet restore "src/Services/KeysManager/KeysManager/KeysManager.csproj"
COPY . .
WORKDIR "/src/src/Services/KeysManager/KeysManager"
RUN dotnet build "KeysManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KeysManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KeysManager.dll"]
