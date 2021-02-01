FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /app

COPY ./src/ ./src/

RUN dotnet publish ./src/Lymer.Web.App --configuration Release --output /app/artifacts

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

COPY --from=build /app/artifacts ./

ENTRYPOINT ["dotnet", "Lymer.Web.App.dll"]