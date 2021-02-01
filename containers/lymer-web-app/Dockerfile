FROM mcr.microsoft.com/dotnet/core/sdk:2.1.700-stretch AS build

WORKDIR /app

COPY ./src/ ./src/

RUN dotnet publish ./src/Lymer.Web.App --configuration Release --output /app/artifacts

FROM mcr.microsoft.com/dotnet/core/aspnet:2.1.11-stretch-slim

WORKDIR /app

COPY --from=build /app/artifacts ./

ENV EFOE_PORT=4999

EXPOSE 4999

# USER scrape-api

ENTRYPOINT ["dotnet", "Lymer.Web.App.dll"]