#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Services/Association/Association/Association.csproj", "src/Services/Association/Association/"]
COPY ["src/Services/Association/Association.Infrastructure/Association.Infrastructure.csproj", "src/Services/Association/Association.Infrastructure/"]
COPY ["src/Services/Association/Association.Domain/Association.Domain.csproj", "src/Services/Association/Association.Domain/"]
COPY ["src/Common/Shared.Domain/Shared.Domain.csproj", "src/Common/Shared.Domain/"]
COPY ["src/Common/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Common/Shared.Infrastructure/"]
RUN dotnet restore "src/Services/Association/Association/Association.csproj"
COPY . .
WORKDIR "/src/src/Services/Association/Association"
RUN dotnet build "Association.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Association.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Association.dll"]