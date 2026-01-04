# Stage 1: Build everything
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all source code
COPY . .

# 1. Restore all
RUN dotnet restore Firefly.Insights.Server.sln

# 2. Loop through and publish
RUN for file in src/*/*.csproj; do \
    dotnet publish "$file" -c Release -o /app/publish --no-restore; \
    done

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Copy the build output
COPY --from=build /app/publish .

EXPOSE 31000

# Launch all 4 services
ENTRYPOINT ["sh", "-c", "\
dotnet /app/Server.Identity.Api.dll --urls http://0.0.0.0:32000 & \
dotnet /app/Server.Contents.Api.dll --urls http://0.0.0.0:33000 & \
dotnet /app/Server.Ai.Api.dll --urls http://0.0.0.0:34000 & \
dotnet /app/Server.Gateway.Api.dll --urls http://0.0.0.0:31000"]