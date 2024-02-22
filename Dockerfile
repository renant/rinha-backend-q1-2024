FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . ./
RUN dotnet restore
RUN dotnet publish ./src/RinhaBackEnd2024.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 8080
EXPOSE 8081

ENTRYPOINT ["dotnet", "RinhaBackEnd2024.dll"]