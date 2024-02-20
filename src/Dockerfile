FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-alpine3.19-aot as base
# FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-jammy-chiseled-aot as base
# FROM mcr.microsoft.com/dotnet/runtime-deps:8.0-jammy-chiseled-extra as base
# FROM alpine:3.19 as base
# FROM ubuntu:23.10 as base
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
# FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.19 as base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=80
EXPOSE 80

FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-alpine3.19-aot AS build
# FROM mcr.microsoft.com/dotnet/nightly/sdk:8.0-jammy-aot AS build
# FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build
# FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.19 AS build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
# ARG BUILD_RUNTIME=linux-x64
ARG BUILD_RUNTIME=linux-musl-x64
# RUN apk update && apk upgrade \
#     && apk add --no-cache clang build-base zlib-dev
# libssl1.1
# RUN apt-get update -y \
#     && apt-get install -y clang zlib1g-dev
WORKDIR /build
COPY webapi.csproj .
RUN dotnet restore -r $BUILD_RUNTIME
COPY . .
RUN dotnet build -r $BUILD_RUNTIME -c $BUILD_CONFIGURATION -o /out/build

FROM build as publish
ARG BUILD_CONFIGURATION=Release
# ARG BUILD_RUNTIME=linux-x64
ARG BUILD_RUNTIME=linux-musl-x64
WORKDIR /build
RUN dotnet publish -r $BUILD_RUNTIME -c $BUILD_CONFIGURATION -o /out/publish --no-build --no-restore
RUN rm /out/publish/*.dbg /out/publish/*.Development.json

FROM base as final
# RUN apk update && apk upgrade \
#     && apk add --no-cache libstdc++
# RUN apt-get update -y \
#     && apt-get install -y curl \
#     && apt-get autoremove -y \
#     && apt-get clean -y
COPY --from=publish /out/publish ./
ENTRYPOINT ["./webapi"]
# ENTRYPOINT ["dotnet", "webapi.dll"]
