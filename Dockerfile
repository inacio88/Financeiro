FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .

WORKDIR "/src/Dima.Api"
RUN dotnet publish "Dima.Api.csproj" -c Release -o /app/build

WORKDIR "/src/Dima.Web"
RUN dotnet publish "Dima.Web.csproj" -c Release -o /app/web

FROM mcr.microsoft.com/dotnet/aspnet:8.0

EXPOSE 8080
EXPOSE 8081

ENV FrontendUrl="http://localhost:8080"
ENV BackendUrl="http://localhost:8080"

WORKDIR /app

COPY --from=build /app/build .
COPY --from=build /app/web /app/

ENTRYPOINT ["dotnet", "Dima.Api.dll"]
