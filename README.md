# GestionEquipos

Proyecto backend para gestionar usuarios, roles y autenticación JWT.

## Resumen

`GestionEquipos` es una API web en .NET (C#) que implementa autenticación y autorización basada en JWT, acceso a datos mediante un `DbContext`, y operaciones sobre usuarios y roles. El código fuente principal está en la carpeta `GestionEquipos`.

## Requisitos

- .NET SDK 8.0 or later
- Visual Studio 2022/2023 o VS Code

## Estructura importante

- `GestionEquipos/Program.cs`: punto de entrada y configuración del host.
- `GestionEquipos/Config`: configuración de `DbContext`, `JwtSettings` y middleware de excepciones.
- `GestionEquipos/Controllers`: controladores HTTP (p. ej. `AuthenticationController`, `RolesController`).
- `GestionEquipos/RepositoryPattern` y `ServiceLayer`: capa de repositorios y servicios.

Explora la estructura completa en la carpeta [GestionEquipos](GestionEquipos).

## Configuración

1. Copia y edita `appsettings.json` y/o `appsettings.Development.json` según tu entorno.
2. Configura la cadena de conexión de la base de datos en la sección `ConnectionStrings`.
3. Ajusta `JwtSettings` (clave secreta, issuer, audience, expiración) en `appsettings.json`.

Ejemplo (fragmento):

```json
{
	"JwtSettings": {
		"Secret": "TU_SECRETO_MUY_LARGO",
		"Issuer": "GestionEquipos",
		"Audience": "GestionEquiposClient",
		"ExpiryMinutes": 60
	}
}
```

### Nota sobre base de datos
El proyecto incluye un `DbContext` en `GestionEquipos/Config/DbContext.cs`. Usa migrations de EF Core si decides persistir datos:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## Ejecutar localmente

Desde la raíz del repositorio:

```bash
cd GestionEquipos
dotnet restore
dotnet build
dotnet run --configuration Development
```

La API expondrá los endpoints configurados (revisa `GestionEquipos/Controllers`).

## Endpoints principales

- Autenticación: `AuthenticationController` (login y register).
- Gestión de roles: `RolesController`.

Revisa las clases en [GestionEquipos/Controllers](GestionEquipos/Controllers) para ver rutas y contratos exactos.

## Pruebas y validación

- Añade pruebas unitarias/integración si lo deseas. Actualmente el proyecto no incluye una carpeta `tests` por defecto.

## Despliegue

- Puedes publicar con `dotnet publish -c Release` y desplegar al host que prefieras (Azure App Service, Docker, etc.).

