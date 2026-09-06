/*
    Aseguranza - VerificarImportacionHdc_SQLServer.sql

    Verificación posterior a una importación HDC en SQL Server / Azure SQL.

    IMPORTANTE:
    - Este script es SOLO DE LECTURA.
    - No inserta, actualiza ni elimina datos.
    - Ejecútalo sobre la misma base configurada en Aseguranza.
    - Normalmente: AseguranzaBD.
*/

SET NOCOUNT ON;
SET XACT_ABORT ON;

PRINT '============================================================';
PRINT 'Aseguranza - Verificación de importación HDC';
PRINT 'Base actual: ' + DB_NAME();
PRINT 'Fecha: ' + CONVERT(varchar(19), GETDATE(), 120);
PRINT '============================================================';
PRINT '';

/* 1. Última importación HDC registrada */
PRINT '1. Última importación HDC registrada';

SELECT TOP (1)
    Id,
    NombreArchivo,
    NombreHoja,
    FechaImportacion,
    TotalRegistros,
    Nuevos,
    Actualizados,
    SinCambios,
    ConAdvertencias,
    Observaciones
FROM dbo.ImportacionHdc
ORDER BY Id DESC;

PRINT '';

/* 2. Últimas 5 importaciones HDC */
PRINT '2. Últimas 5 importaciones HDC';

SELECT TOP (5)
    Id,
    NombreArchivo,
    NombreHoja,
    FechaImportacion,
    TotalRegistros,
    Nuevos,
    Actualizados,
    SinCambios,
    ConAdvertencias
FROM dbo.ImportacionHdc
ORDER BY Id DESC;

PRINT '';

/* 3. Cantidad total de perfiles HDC guardados */
PRINT '3. Cantidad total de perfiles HDC guardados';

SELECT
    COUNT(*) AS PerfilesHdc
FROM dbo.PerfilHdcTrabajador;

PRINT '';

/* 4. Perfiles HDC sin trabajador relacionado: debe ser 0 */
PRINT '4. Perfiles HDC sin trabajador relacionado';

SELECT
    COUNT(*) AS PerfilesSinTrabajador
FROM dbo.PerfilHdcTrabajador AS H
LEFT JOIN dbo.Trabajador AS T
    ON T.Id = H.IdTrabajador
WHERE T.Id IS NULL;

PRINT '';

/* 5. Trabajadores con línea fuera de su planta: debe ser 0 */
PRINT '5. Trabajadores con línea fuera de su planta';

SELECT
    COUNT(*) AS TrabajadoresConLineaFueraDePlanta
FROM dbo.Trabajador AS T
INNER JOIN dbo.Linea AS L
    ON L.Id = T.IdLinea
WHERE T.IdLinea IS NOT NULL
  AND L.IdPlanta <> T.IdPlanta;

PRINT '';

/* 6. No. Reloj duplicados: debe devolver 0 filas */
PRINT '6. No. Reloj duplicados';

SELECT
    T.NoReloj,
    COUNT(*) AS Cantidad
FROM dbo.Trabajador AS T
GROUP BY T.NoReloj
HAVING COUNT(*) > 1
ORDER BY T.NoReloj;

PRINT '';

/* 7. Relaciones obligatorias inválidas */
PRINT '7. Relaciones obligatorias inválidas';

SELECT
    SUM(CASE WHEN P.Id IS NULL THEN 1 ELSE 0 END) AS TrabajadoresSinPlantaValida,
    SUM(CASE WHEN Tu.Id IS NULL THEN 1 ELSE 0 END) AS TrabajadoresSinTurnoValido
FROM dbo.Trabajador AS T
LEFT JOIN dbo.Planta AS P
    ON P.Id = T.IdPlanta
LEFT JOIN dbo.Turno AS Tu
    ON Tu.Id = T.IdTurno;

PRINT '';

/* 8. Resumen de trabajadores por planta */
PRINT '8. Resumen de trabajadores por planta';

SELECT
    P.Nombre AS Planta,
    COUNT(*) AS Trabajadores
FROM dbo.Trabajador AS T
INNER JOIN dbo.Planta AS P
    ON P.Id = T.IdPlanta
GROUP BY P.Nombre
ORDER BY P.Nombre;

PRINT '';

/* 9. Caso de control */
DECLARE @NoReloj varchar(50) = '278654';

PRINT '9. Caso de control - Trabajador ' + @NoReloj;

SELECT
    T.Id,
    T.NoReloj,
    T.Nombre,
    P.Nombre AS Planta,
    Tu.Nombre AS Turno,
    COALESCE(L.Nombre, 'SIN ASIGNAR') AS Linea,
    T.RutaFoto
FROM dbo.Trabajador AS T
INNER JOIN dbo.Planta AS P
    ON P.Id = T.IdPlanta
INNER JOIN dbo.Turno AS Tu
    ON Tu.Id = T.IdTurno
LEFT JOIN dbo.Linea AS L
    ON L.Id = T.IdLinea
WHERE T.NoReloj = @NoReloj;

PRINT '';

/* 10. Perfil HDC del caso de control */
PRINT '10. Perfil HDC del trabajador ' + @NoReloj;

SELECT
    T.NoReloj,
    H.EmpleadoHdc,
    H.NombreHdc,
    H.LocalidadHdc,
    H.TurnoHdc,
    H.LineaHdc,
    H.FechaServicio,
    H.DepartamentoHdc,
    H.PuestoHdc,
    H.CategoriaHdc,
    H.PositionHdc,
    H.FunctionHdc,
    H.ProcesoHdc,
    H.DptoHdc,
    H.FechaUltimaImportacion,
    H.EncontradoUltimoHdc
FROM dbo.PerfilHdcTrabajador AS H
INNER JOIN dbo.Trabajador AS T
    ON T.Id = H.IdTrabajador
WHERE T.NoReloj = @NoReloj;

PRINT '';
PRINT '============================================================';
PRINT 'Verificación terminada.';
PRINT 'Revisa especialmente:';
PRINT '- Última importación registrada.';
PRINT '- PerfilesSinTrabajador = 0.';
PRINT '- TrabajadoresConLineaFueraDePlanta = 0.';
PRINT '- No. Reloj duplicados = 0 filas.';
PRINT '- Relaciones obligatorias inválidas = 0.';
PRINT '============================================================';
