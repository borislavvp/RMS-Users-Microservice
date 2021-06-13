#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Users.API/Users.API.csproj", "Users.API/"]
COPY ["Users.Data/Users.Data.csproj", "Users.Data/"]
COPY ["Users.Service/Users.Service.csproj", "Users.Service/"]
RUN dotnet restore "Users.API/Users.API.csproj"
COPY . .
WORKDIR "/src/Users.API"
RUN dotnet build "Users.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Users.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.API.dll"]