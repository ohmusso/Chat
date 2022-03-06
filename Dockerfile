# -------------------------
# docker build command
# -------------------------
# 
# docker build --no-cache --progress=plain -t grpcserver-image -f Dockerfile .
#
# -------------------------
# docker run command
# -------------------------
# 
# docker run -it --rm -p 5000:80 --name grpcserver grpcserver-image
#
# -------------------------
# build context
# -------------------------
#
# app
#     | GrpcServer.dll (copy from src)
# src
#     | Grpc
#            | GrpcServer
#            | GrpcSheared
#     | Assets/Scripts/Shared
#

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ./Grpc ./Grpc
COPY ./Assets/Scripts/Chat/Shared ./Assets/Scripts/Chat/Shared
WORKDIR /src/Grpc/GrpcServer
RUN dotnet build -c Release "GrpcServer.csproj"

FROM build as publish
RUN dotnet publish -c Release "GrpcServer.csproj"

FROM base AS final
WORKDIR /app
COPY --from=publish src/Grpc/GrpcServer/bin/Release/net5.0 .
ENTRYPOINT ["dotnet", "GrpcServer.dll"]
