#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

USER root
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
USER app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Services/Url/Url/Url.csproj", "src/Services/Url/Url/"]
COPY ["src/Common/Shared.Domain/Shared.Domain.csproj", "src/Common/Shared.Domain/"]
COPY ["src/Services/Url/Url.Infrastructure/Url.Infrastructure.csproj", "src/Services/Url/Url.Infrastructure/"]
COPY ["src/Services/Url/Url.Domain/Url.Domain.csproj", "src/Services/Url/Url.Domain/"]
RUN dotnet restore "src/Services/Url/Url/Url.csproj"
COPY . .
WORKDIR "/src/src/Services/Url/Url"
RUN dotnet build "Url.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Url.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Url.dll"]
