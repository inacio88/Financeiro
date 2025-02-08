# Stage 1: Build the Blazor WASM project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project files
COPY ["Dima.Web/Dima.Web.csproj", "Dima.Web/"]
COPY ["Dima.Core/Dima.Core.csproj", "Dima.Core/"]

# Restore dependencies
RUN dotnet restore "Dima.Web/Dima.Web.csproj"

# Copy the rest of the application code
COPY . .

# Build the Blazor WASM project
WORKDIR "/src/Dima.Web"
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Serve the Blazor WASM app using a lightweight web server
FROM nginx:alpine
WORKDIR /usr/share/nginx/html

# Copy the published Blazor WASM files to the NGINX directory
COPY --from=build /app/publish/wwwroot .
# Copie o arquivo de configuração personalizado
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Expose ports for HTTP and HTTPS
EXPOSE 80
EXPOSE 443

# Start NGINX
CMD ["nginx", "-g", "daemon off;"]