# Portfolio API (.NET 9)

REST API for an Angular 19 portfolio SPA: About, Skills, Projects, JWT Auth.

## Stack

- ASP.NET Core 9 Web API
- EF Core + PostgreSQL (Npgsql) / SQLite (local)
- JWT Bearer authentication
- Swagger / OpenAPI
- Docker (Render-ready)

## Local run

```bash
dotnet restore
dotnet run
```

API: `http://localhost:5080` (see `Properties/launchSettings.json`)  
Swagger: `http://localhost:5080/swagger`

Default admin (seed):

- Email: `admin@portfolio.local`
- Password: `Admin123!`

Local DB: SQLite file `portfolio.db` (`ConnectionStrings:DefaultConnection`).

## Endpoints

| Method | Path | Auth |
|--------|------|------|
| GET | `/api/about` | Public |
| PUT | `/api/about` | JWT |
| GET | `/api/skills` | Public |
| GET | `/api/skills/{id}` | Public |
| POST | `/api/skills` | JWT |
| PUT | `/api/skills/{id}` | JWT |
| DELETE | `/api/skills/{id}` | JWT |
| GET | `/api/projects` | Public |
| GET | `/api/projects/{id}` | Public |
| POST | `/api/projects` | JWT |
| PUT | `/api/projects/{id}` | JWT |
| DELETE | `/api/projects/{id}` | JWT |
| POST | `/api/auth/login` | Public |

JSON uses **camelCase** (`avatarUrl`, `socialLinks`, `skillIds`, `projectIds`, skill `level` as `beginner|intermediate|advanced|expert`).

## Login example

```bash
curl -X POST http://localhost:5080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@portfolio.local","password":"Admin123!"}'
```

Use header: `Authorization: Bearer <token>`.

## Migrations (PostgreSQL)

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

On startup with PostgreSQL the API runs `MigrateAsync` + seeds empty DB.

## Render deploy

1. Create a Web Service from this repo (Docker).
2. Attach a PostgreSQL database (Render sets `DATABASE_URL`).
3. Environment variables:

| Variable | Notes |
|----------|--------|
| `DATABASE_URL` | Auto from Render Postgres |
| `JwtSettings__Secret` | Long random secret (≥32 chars) |
| `JwtSettings__Issuer` | e.g. `PortfolioApi` |
| `JwtSettings__Audience` | e.g. `PortfolioSpa` |
| `Seed__AdminEmail` | Admin login |
| `Seed__AdminPassword` | Admin password |
| `Cors__AllowedOrigins__0` | `https://your-angular.onrender.com` |
| `Cors__AllowedOrigins__1` | `http://localhost:4200` |
| `ENABLE_SWAGGER` | `true` (optional) |
| `ASPNETCORE_ENVIRONMENT` | `Production` |

The Dockerfile listens on `$PORT` (Render) / `8080`.

## Angular CORS

Add your SPA origin to `Cors:AllowedOrigins` in `appsettings.json` / `appsettings.Production.json` or via env vars above.
