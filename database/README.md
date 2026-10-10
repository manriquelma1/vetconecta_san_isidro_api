# Base de datos del curso

## Opción A: restaurar el backup entregado

`BD_VETCONECTA.bak` es el respaldo original, incluido sin modificaciones.

1. Abre SSMS y conéctate a tu instancia de SQL Server.
2. En Bases de datos, selecciona Restaurar base de datos.
3. Elige Dispositivo y selecciona este `.bak`. Si SQL Server no puede acceder a la carpeta, copia el backup a una carpeta accesible para el servicio.
4. Usa como destino una base **nueva**, `BD_VETCONECTA_TESTING`. No sobrescribas tu base de trabajo.
5. En Archivos, verifica que los archivos de datos y log se restauren en rutas de tu instancia, con nombres que no coincidan con una base existente.
6. Ejecuta la restauración y configura el API para esa instancia y base.

La instancia de destino debe ser compatible con la versión que generó el backup; un respaldo de una versión más nueva no se restaura en una más antigua. La restauración y el contenido de este respaldo no se han verificado en esta preparación.

## Opción B: crear una base nueva con las migraciones

No requiere restaurar el `.bak`. Desde la raíz del repositorio y con SQL Server disponible:

```powershell
dotnet tool restore
$env:ASPNETCORE_ENVIRONMENT = "Development"
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=BD_VETCONECTA_TESTING;Trusted_Connection=True;TrustServerCertificate=True"
dotnet ef database update --project src/VetConecta.Infrastructure --startup-project src/VetConecta.Api
```

Las tres migraciones incluyen la estructura y datos de demostración. El usuario de la conexión debe tener permisos para crear la base o modificar una base de pruebas existente. No ejecutes estas instrucciones contra una base ajena al proyecto.

## Generar SQL para la entrega

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet tool restore
dotnet ef migrations script --idempotent --project src/VetConecta.Infrastructure --startup-project src/VetConecta.Api --output database/schema.sql
```

Las migraciones se mantienen dentro del backend para evitar duplicar su definición.

## Integridad del respaldo

SHA-256 de `BD_VETCONECTA.bak`:

```text
c5be3cb17485659560411fe020bf5177c51b586837913d08a8778c13ff0ad665
```
