# Stage 1: Build .NET backend
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-backend
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY aspnext.Server/aspnext.Server.csproj ./aspnext.Server/
RUN dotnet restore ./aspnext.Server/aspnext.Server.csproj

# Copy everything else and build
COPY aspnext.Server/. ./aspnext.Server/
RUN dotnet publish ./aspnext.Server/aspnext.Server.csproj -c Release -o out

# Stage 2: Build Node.js frontend
FROM node:22 AS build-frontend
WORKDIR /app
COPY aspnext.client/package.json aspnext.client/package-lock.json ./
RUN npm install
COPY aspnext.client/ ./
RUN npm run build

# Stage 3: Final image with .NET runtime, Node.js, and Nginx
FROM mcr.microsoft.com/dotnet/aspnet:9.0

# Install nginx, nodejs, npm, and .NET SDK
RUN apk update && apk add --no-cache nginx nodejs npm && \
    apk add --no-cache --virtual .build-deps \
    icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib

# Copy built artifacts
COPY --from=build-frontend /app/dist /var/www/frontend
COPY --from=build-backend /app/out /var/www/backend

# Copy package.json for npm
COPY aspnext.client/package.json /var/www/frontend/

# Nginx configuration
RUN rm -f /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d

# Copy start.sh and set executable permissions
COPY start.sh /start.sh
RUN chmod +x /start.sh

EXPOSE 80

CMD ["/start.sh"]