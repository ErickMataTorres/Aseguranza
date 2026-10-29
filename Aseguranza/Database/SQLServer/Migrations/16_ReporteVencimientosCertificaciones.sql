SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
        THROW 50050, 'Missing dbo.SchemaVersion; apply migrations 10 through 15 first.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 15)
        THROW 50051, 'Migration 15 must be deployed before migration 16.', 1;
    IF COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NULL
        THROW 50052, 'Missing dbo.Trabajador.Activo; apply migration 10 first.', 1;

    EXEC(N'CREATE OR ALTER PROCEDURE [dbo].[spExportarVencimientosCertificaciones]
    @Activo BIT = 1,
    @Categoria INT = 0,
    @HorizonteDias INT = 30,
    @IdPlanta INT = NULL,
    @IdLinea INT = NULL,
    @IdTurno INT = NULL,
    @IdProceso INT = NULL,
    @TextoBuscar VARCHAR(100) = '''',
    @FechaReferencia DATE
AS
BEGIN
    SET NOCOUNT ON;

    IF @FechaReferencia IS NULL
        THROW 50062, ''A reference date is required for export.'', 1;
    IF @HorizonteDias NOT IN (7, 15, 30, 60, 90)
        THROW 50060, ''Unsupported expiration horizon.'', 1;
    IF @Categoria NOT IN (0, 1, 2)
        THROW 50061, ''Unsupported expiration category.'', 1;
    DECLARE @FechaFin DATE = DATEADD(DAY, @HorizonteDias, @FechaReferencia);

    SELECT C.Id AS CertificationId,
           T.Id AS WorkerId, T.NoReloj AS WorkerNumber,
           T.Nombre AS WorkerName, T.Activo AS Active,
           T.IdPlanta AS PlantId, Pl.Nombre AS PlantName,
           Li.Id AS LineId, Li.Nombre AS LineName,
           T.IdTurno AS ShiftId, Tu.Nombre AS ShiftName,
           Pr.Id AS ProcessId, Pr.Nombre AS ProcessName,
           C.FechaCertificacion AS CertificationDate,
           C.FechaVencimiento AS ExpirationDate,
           DATEDIFF(DAY, @FechaReferencia, C.FechaVencimiento)
               AS DaysUntilExpiration,
           CASE WHEN C.FechaVencimiento < @FechaReferencia
                THEN 1 ELSE 2 END AS EventState
    INTO #Eventos
    FROM dbo.Certificacion AS C
    INNER JOIN dbo.Trabajador AS T ON T.Id = C.IdTrabajador
    INNER JOIN dbo.Planta AS Pl ON Pl.Id = T.IdPlanta
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
        AND Li.IdPlanta = T.IdPlanta
    INNER JOIN dbo.Proceso AS Pr ON Pr.Id = C.IdProceso
    WHERE C.FechaVencimiento <= @FechaFin
      AND NOT EXISTS
      (
          SELECT 1 FROM dbo.CertificacionAnulacion AS CA
          WHERE CA.IdCertificacion = C.Id AND CA.Activa = 1
            AND (CA.EsPermanente = 1 OR CA.FechaFin IS NULL
                 OR CAST(CA.FechaFin AS DATE) >= @FechaReferencia)
      )
      AND (@Activo IS NULL OR T.Activo = @Activo)
      AND (@IdPlanta IS NULL OR T.IdPlanta = @IdPlanta)
      AND (@IdLinea IS NULL OR Li.Id = @IdLinea)
      AND (@IdTurno IS NULL OR T.IdTurno = @IdTurno)
      AND (@IdProceso IS NULL OR C.IdProceso = @IdProceso)
      AND (T.NoReloj LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR T.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR Pr.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR Pl.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR ISNULL(Li.Nombre, '''') LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR Tu.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%'');

    SELECT ISNULL(SUM(CASE WHEN EventState = 1 THEN 1 ELSE 0 END), 0)
               AS ExpiredEvents,
           ISNULL(SUM(CASE WHEN EventState = 2 THEN 1 ELSE 0 END), 0)
               AS UpcomingEvents,
           COUNT(DISTINCT WorkerId) AS AffectedWorkers,
           COUNT(DISTINCT ProcessId) AS AffectedProcesses
    FROM #Eventos
    WHERE @Categoria = 0 OR EventState = @Categoria;

    SELECT CertificationId, WorkerId, WorkerNumber, WorkerName, Active,
           PlantId, PlantName, LineId, LineName, ShiftId, ShiftName,
           ProcessId, ProcessName, CertificationDate, ExpirationDate,
           DaysUntilExpiration, EventState
    FROM #Eventos
    WHERE @Categoria = 0 OR EventState = @Categoria
    ORDER BY EventState, ExpirationDate, WorkerName,
             WorkerId, ProcessId, CertificationId;
END;');

    IF OBJECT_ID(N'dbo.spExportarVencimientosCertificaciones', N'P') IS NULL
       OR NOT EXISTS (SELECT 1 FROM sys.parameters
           WHERE object_id = OBJECT_ID(N'dbo.spExportarVencimientosCertificaciones')
             AND name = N'@FechaReferencia')
        THROW 50053, 'Expiration export procedure validation failed.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 16)
        INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
        VALUES (16, SYSUTCDATETIME(), 'Consulta completa para reporte de vencimientos');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
