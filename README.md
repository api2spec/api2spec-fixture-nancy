# api2spec-fixture-nancy

A Nancy (.NET) API fixture for testing api2spec framework detection and route extraction.

## Note

Nancy is a legacy framework that is no longer actively maintained. This fixture is provided for testing purposes only.

## Requirements

- .NET 6.0 SDK
- Docker (optional)

## Running Locally

```bash
dotnet restore
dotnet run
```

The API will be available at `http://localhost:8080`.

## Running with Docker

```bash
docker compose up --build
```

## API Endpoints

### Health
- `GET /health` - Health check
- `GET /health/ready` - Readiness check

### Users
- `GET /users` - List all users
- `GET /users/{id}` - Get user by ID
- `POST /users` - Create a new user
- `PUT /users/{id}` - Update user by ID
- `DELETE /users/{id}` - Delete user by ID
- `GET /users/{userId}/posts` - Get posts by user

### Posts
- `GET /posts` - List all posts
- `GET /posts/{id}` - Get post by ID
- `POST /posts` - Create a new post
