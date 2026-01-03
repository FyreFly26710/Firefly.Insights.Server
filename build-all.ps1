# Configuration
$DockerHubUser = "firefly26710"
$ImageName = "firefly-insights-server"
$FullImageTag = "${DockerHubUser}/${ImageName}:latest"

# 1. Check if Docker is running
docker ps >$null 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Error "Docker is not running. Please start Docker Desktop and try again."
    exit
}

Write-Host "--- Starting MVP All-in-One Build ---" -ForegroundColor Cyan

# 2. Login to Docker Hub
Write-Host "Logging into Docker Hub..." -ForegroundColor Gray
docker login

# 3. Build the single consolidated image
Write-Host "`nBuilding Image: $FullImageTag" -ForegroundColor Green

# Use --no-cache if you want to be 100% sure it's a fresh MVP build
docker build -t "$FullImageTag" .

if ($LASTEXITCODE -ne 0) {
    Write-Error "Build failed. Check the Dockerfile output above."
    exit $LASTEXITCODE
}

# 4. Push the image
Write-Host "`nPushing $FullImageTag to Docker Hub..." -ForegroundColor Yellow
docker push "$FullImageTag"

Write-Host "`nDone! Deployment Image: $FullImageTag" -ForegroundColor Green