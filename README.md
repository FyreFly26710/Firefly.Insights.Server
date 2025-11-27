# Firefly.Insights.Server

Client App: React
Server App: .NET 8 Web API Microservice Architecture
- Server.Gateway.Api (http://localhost:31000): Yarp as API Gateway
- Server.Identity.Api (http://localhost:32000): JWT IdentityServer 

Database: PostgreSQL

Development Environment:
- macos & windows
- PgSql run on docker
- Server run on local machine, expose http://localhost:31000 to client
- Client run on local machine, expose http://localhost:3000 to browser

Production Environment:
- macos
- PgSql & server run on docker
- Nginx as reverse proxy, expose https://insights.firefly.com to client
- Client run on client side hosted on CloudFlare