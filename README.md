# Gamer Reviews - Backend

A .NET 8-based backend Web API that powers the GamerReviews application. Provides endpoints for games, reviews, user management, authentication, tags, favorites and ranking. Built for fast development with Entity Framework Core, secured with JWT, and documented with Swagger. This project was developed as part of the final project for the Algorithms and Data Structures III course of the Systems Analyst career in 2025.


## Features

- **Game Reviews**: Create, read, update, and delete game reviews
- **User Management**: Register, login, user profiles, roles (user/admin)
- **Authentication**: JWT-based authentication and authorization
- **Search**: Search games by title, tags, or other metadata
- **Favorites**: Save and manage favorite games per user
- **Ranking System**: Aggregate ratings to produce game rankings
- **Tags**: Add and manage tags for games
- **Admin API**: Endpoints for administrative tasks (manage games, users, tags)
- **API Documentation**: Swagger/OpenAPI UI for exploring endpoints

## Technologies Used

- .NET 8 and ASP.NET Core Web API
- Entity Framework Core (migrations and data access)
- AutoMapper (DTO mapping)
- FluentValidation (request validation)
- Swashbuckle / Swagger (API documentation)
- JWT Bearer Authentication (token-based security)
- SQL Server or SQLite as database provider

## Installation

### 1. Clone the repository
git clone https://github.com/Kohenkyo/GamerReviews_Backend.git cd GamerReviews_Backend

### 2. Restore and build
dotnet restore dotnet build

### 3. Import the database
Tables and stored procedures on the database.sql file on the repository

### 4. Configure environment variables
Create an `appsettings.Development.json` file in the API project root with the following structure:
{ "ConnectionStrings": { "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=GamerReviewsDb;Trusted_Connection=True;MultipleActiveResultSets=true" }, "JWT": { "Key": "<your-secret-key-min-32-chars>", "Issuer": "GamerReviews", "Audience": "GamerReviewsUsers", "ExpiresInMinutes": 60 } }

Alternatively, set environment variables:
**Windows PowerShell:**
$env:ConnectionStrings__DefaultConnection = "Server=(localdb)\mssqllocaldb;Database=GamerReviewsDb;Trusted_Connection=True;" $env:JWT__Key = "<your-secret-key>" $env:JWT__Issuer = "GamerReviews" $env:JWT__Audience = "GamerReviewsUsers"

**Linux/macOS:**
export ConnectionStrings__DefaultConnection="Server=localhost;Database=GamerReviewsDb;User Id=sa;Password=your-password;" export JWT__Key="<your-secret-key>"

### 5. Run the API
dotnet run --urls "https://localhost:7292"

The API will be available at `https://localhost:7292`

## Usage
### API Base URL
https://localhost:7292/api

### Swagger Documentation
When running in Development environment, access the interactive API documentation:
https://localhost:7292/swagger/index.html

- Frontend repository: [GamerReviews_Frontend](https://github.com/Kohenkyo/GamerReviews)


*This Repository is a copy of the original project that was developed with a team.
