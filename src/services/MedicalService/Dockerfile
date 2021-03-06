FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/services/MedicalService/MedicalService.csproj", "src/services/MedicalService/"]
COPY ["src/common/Infrastructure/Infrastructure.csproj", "src/common/Infrastructure/"]
RUN dotnet restore "src/services/MedicalService/MedicalService.csproj"
COPY . .
WORKDIR "/src/src/services/MedicalService"
RUN dotnet build "MedicalService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MedicalService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MedicalService.dll"]