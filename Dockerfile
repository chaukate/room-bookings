#Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-focal AS build
WORKDIR /source
COPY ..
RUN dotnet restore "./RB.Api/RB.Api.csproj" disable-parallel
RUN dotnet publish "./RB.Api/RB.Api.csproj" -c release -o /app --no-restore

@Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0-focal
WORKDIR /app
COPY --from-build /api ./

EXPOSE 5080

ENTRYPOINT ["dotnet", "RB.Api.dll"]
