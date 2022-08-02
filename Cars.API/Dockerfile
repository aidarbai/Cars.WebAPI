#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Cars.API/Settings/config.json", "Cars.API/Settings/config.json"]
COPY ["Cars.API/Cars.API.csproj", "Cars.API/"]
COPY ["Cars.FACADE/Cars.FACADE.csproj", "Cars.FACADE/"]
COPY ["Cars.BLL/Cars.BLL.csproj", "Cars.BLL/"]
COPY ["Cars.DAL/Cars.DAL.csproj", "Cars.DAL/"]
COPY ["Cars.COMMON/Cars.COMMON.csproj", "Cars.COMMON/"]
RUN dotnet restore "Cars.API/Cars.API.csproj"
COPY . .
WORKDIR "/src/Cars.API"
RUN dotnet build "Cars.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cars.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt-get update; apt-get install -y iputils-ping; apt-get install -y vim; apt-get install -y traceroute

ENTRYPOINT ["dotnet", "Cars.API.dll"]