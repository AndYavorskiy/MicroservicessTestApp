FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/services/SecondService/SecondService.csproj", "src/services/SecondService/"]
COPY ["src/common/Infrastructure/Infrastructure.csproj", "src/common/Infrastructure/"]
RUN dotnet restore "src/services/SecondService/SecondService.csproj"
COPY . .
WORKDIR "/src/src/services/SecondService"
RUN dotnet build "SecondService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SecondService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecondService.dll"]