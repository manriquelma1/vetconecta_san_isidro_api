# VetConecta San Isidro API

API del proyecto universitario VetConecta, separado del frontend Angular. Incluye la solución .NET 10, las migraciones de Entity Framework Core y el backup de SQL Server solicitado para la entrega.

## Estructura

- `VetConecta.slnx`: solución con los cuatro proyectos.
- `src/VetConecta.Api`: controladores y configuración HTTP.
- `src/VetConecta.Application`: casos de uso y validaciones.
- `src/VetConecta.Domain`: entidades.
- `src/VetConecta.Infrastructure`: acceso a SQL Server, autenticación y migraciones.
- `database/BD_VETCONECTA.bak`: respaldo original.

## Ejecutar localmente

Requisitos: SDK de .NET 10 y SQL Server. Restaura el backup en una base nueva llamada `BD_VETCONECTA_TESTING`, o créala mediante las migraciones según [database/README.md](database/README.md).

Desde la raíz del repositorio:

```powershell
dotnet restore VetConecta.slnx
dotnet run --project src/VetConecta.Api --launch-profile http
```

El API escucha en `http://localhost:5242` y Swagger está en `http://localhost:5242/swagger`. El perfil `http` activa Development.

La conexión de desarrollo usa SQL Server en `localhost`, autenticación Windows y la base `BD_VETCONECTA_TESTING`. Para otra instancia, configura la conexión en la misma terminal antes de ejecutar el API:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost\SQLEXPRESS;Database=BD_VETCONECTA_TESTING;Trusted_Connection=True;TrustServerCertificate=True"
dotnet run --project src/VetConecta.Api --launch-profile http
```

Para autenticación SQL, usa una cadena adecuada a tu servidor mediante variables de entorno o `dotnet user-secrets`. No publiques contraseñas personales.

## Frontend

Angular se mantiene en su repositorio independiente. Su `apiUrl` debe apuntar a `http://localhost:5242/api` durante el desarrollo. CORS permite `http://localhost:4200`.

## Datos académicos

Las migraciones incluyen cuentas y datos de demostración. `appsettings.Development.json` contiene una clave JWT pública exclusivamente de demostración. El backup se conserva sin modificaciones; no se ha restaurado ni comparado con las migraciones.

## Preparación para un VPS

La separación de repositorios no cambia la lógica del API. Antes de publicarlo, configura la conexión a SQL Server y una clave `Jwt__Key` propia del servidor (la clave de la configuración base está vacía), ajusta los orígenes CORS en `src/VetConecta.Api/Program.cs` y configura la URL pública del API en Angular. La autenticación Windows de la conexión local no sirve tal cual en un VPS Linux. Swagger está habilitado únicamente en Development.

Para generar los archivos de publicación:

```powershell
dotnet publish src/VetConecta.Api -c Release -o publish
```

La instalación y ejecución como servicio en el VPS dependen de su sistema operativo y de dónde se aloje SQL Server.

## Validación

La separación conserva exactamente los archivos de código y configuración del backend del repositorio de pruebas; solo adapta documentación y exclusiones a la nueva raíz. Se verifican las referencias relativas de proyectos y la integridad del backup.

La compilación anterior con SDK 10.0.401 quedó bloqueada por un error de inicialización de rutas de NuGet en el entorno (`Value cannot be null`, `path1`). No se presenta como compilación superada. Tampoco se ha comprobado el login contra una base restaurada. Para validar en el equipo de desarrollo:

```powershell
dotnet build VetConecta.slnx
```
