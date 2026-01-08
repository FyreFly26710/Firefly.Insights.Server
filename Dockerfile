FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG TARGETARCH
WORKDIR /src

COPY . .

RUN dotnet restore Firefly.Insights.Server.sln -a $TARGETARCH

RUN dotnet publish src/Server.Identity.Api/Server.Identity.Api.csproj -c Release -o /app/publish/identity -a $TARGETARCH --no-restore
RUN dotnet publish src/Server.Contents.Api/Server.Contents.Api.csproj -c Release -o /app/publish/contents -a $TARGETARCH --no-restore
RUN dotnet publish src/Server.Ai.Api/Server.Ai.Api.csproj -c Release -o /app/publish/ai -a $TARGETARCH --no-restore
RUN dotnet publish src/Server.Gateway.Api/Server.Gateway.Api.csproj -c Release -o /app/publish/gateway -a $TARGETARCH --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 21000
EXPOSE 32000
EXPOSE 33000
EXPOSE 34000

ENTRYPOINT ["sh", "-c", "\
dotnet /app/identity/Server.Identity.Api.dll --urls http://0.0.0.0:32000 & \
dotnet /app/contents/Server.Contents.Api.dll --urls http://0.0.0.0:33000 & \
dotnet /app/ai/Server.Ai.Api.dll --urls http://0.0.0.0:34000 & \
dotnet /app/gateway/Server.Gateway.Api.dll --urls http://0.0.0.0:21000"]