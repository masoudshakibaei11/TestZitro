
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY OnlineShopZitro.sln .


COPY src/OnlineShopZitro.API/OnlineShopZitro.API.csproj src/OnlineShopZitro.API/
COPY src/OnlineShopZitro.Domain/OnlineShopZitro.Domain.csproj src/OnlineShopZitro.Domain/
COPY src/OnlineShopZitro.Application/OnlineShopZitro.Application.csproj src/OnlineShopZitro.Application/
COPY src/OnlineShopZitro.Infrastructure/OnlineShopZitro.Infrastructure.csproj src/OnlineShopZitro.Infrastructure/


RUN dotnet restore


COPY . .


WORKDIR /src/src/OnlineShopZitro.API
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "OnlineShopZitro.API.dll"]
