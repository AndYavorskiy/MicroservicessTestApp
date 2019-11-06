FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/services/ThirdService/ThirdService.csproj", "src/services/ThirdService/"]
RUN dotnet restore "src/services/ThirdService/ThirdService.csproj"
COPY . .
WORKDIR "/src/src/services/ThirdService"
RUN dotnet build "ThirdService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ThirdService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ThirdService.dll"]