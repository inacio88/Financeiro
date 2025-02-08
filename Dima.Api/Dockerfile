FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Dima.Api/Dima.Api.csproj", "Dima.Api/"]
COPY ["Dima.Core/Dima.Core.csproj", "Dima.Core/"]
RUN dotnet restore "Dima.Api/Dima.Api.csproj"

COPY . .
WORKDIR "/src/Dima.Api"
RUN dotnet build "Dima.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dima.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 8080
EXPOSE 8081

WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dima.Api.dll"]
