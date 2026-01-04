# Configuration
$DockerHubUser = "firefly26710"
$ImageName = "firefly-insights-server"
$FullImageTag = "${DockerHubUser}/${ImageName}:latest"

# 1. Check if Docker is running
docker ps >$null 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Error "Docker is not running. Please start Docker Desktop."
    exit
}

Write-Host "--- Starting Multi-Platform Build (Windows + Mac) ---" -ForegroundColor Cyan

# 2. Ensure Buildx builder is ready
Write-Host "Setting up multi-platform builder..." -ForegroundColor Gray
docker buildx create --name firefly-builder --use 2>$null
docker buildx inspect --bootstrap

# 3. Build and Push for both architectures
Write-Host "`nBuilding and Pushing: $FullImageTag" -ForegroundColor Green
Write-Host "Targeting: linux/amd64 and linux/arm64" -ForegroundColor Gray

# Note: buildx build --push combines the build and push steps
docker buildx build --platform linux/amd64,linux/arm64 -t "$FullImageTag" --push .

if ($LASTEXITCODE -ne 0) {
    Write-Error "Multi-platform build failed."
    exit $LASTEXITCODE
}

Write-Host "`nDone!" -ForegroundColor Green