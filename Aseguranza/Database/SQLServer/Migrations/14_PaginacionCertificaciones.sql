SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;
    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
        THROW 50050, 'Missing dbo.SchemaVersion; apply migrations 10 through 13 first.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 13)
        THROW 50051, 'Migration 13 must be deployed before migration 14.', 1;
    IF COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NULL
        THROW 50052, 'Missing dbo.Trabajador.Activo; apply migration 10 first.', 1;

    EXEC(N'CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadoresEstadoCertificacion]
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100),
    @Activo BIT = NULL,
    @AsOfDate DATE = NULL,
    @PageNumber INT = 1 OUTPUT,
    @PageSize INT = 100,
    @TotalCount INT = 0 OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @AsOfDate IS NULL SET @AsOfDate = CAST(GETDATE() AS DATE);
    IF @PageSize IS NULL OR @PageSize <= 0 OR @PageSize > 100
        SET @PageSize = 100;
    IF @PageNumber IS NULL OR @PageNumber < 1
        SET @PageNumber = 1;

    SELECT
        T.Id, T.NoReloj, T.Nombre, T.Activo, T.RutaFoto,
        T.IdLocalidad, Lo.Nombre AS NombreLocalidad,
        T.IdTurno, Tu.Nombre AS NombreTurno,
        T.IdPlanta, P.Nombre AS NombrePlanta,
        T.IdLinea, COALESCE(Li.Nombre, ''SIN ASIGNAR'') AS NombreLinea,
        CASE
            WHEN COUNT(C.Id) = 0 THEN ''Sin certificar''
            WHEN SUM(CASE WHEN C.FechaVencimiento < @AsOfDate
                          THEN 1 ELSE 0 END) > 0 THEN ''Vencida''
            WHEN SUM(CASE WHEN C.FechaVencimiento
                               BETWEEN @AsOfDate AND DATEADD(DAY, 30, @AsOfDate)
                          THEN 1 ELSE 0 END) > 0 THEN ''Por vencer''
            ELSE ''Vigente''
        END AS EstadoCertificacion
    INTO #TrabajadoresEstado
    FROM dbo.Trabajador AS T
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    LEFT JOIN dbo.Certificacion AS C ON C.IdTrabajador = T.Id
        AND NOT EXISTS
        (
            SELECT 1 FROM dbo.CertificacionAnulacion AS CA
            WHERE CA.IdCertificacion = C.Id AND CA.Activa = 1
              AND (CA.EsPermanente = 1 OR CA.FechaFin IS NULL
                   OR CAST(CA.FechaFin AS DATE) >= @AsOfDate)
        )
    WHERE (@Activo IS NULL OR T.Activo = @Activo)
      AND (T.NoReloj LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR T.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR ISNULL(Lo.Nombre, '''') LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR Tu.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR P.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR COALESCE(Li.Nombre, ''SIN ASIGNAR'')
               LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
           OR ISNULL(@TextoBuscar, '''') = '''')
    GROUP BY T.Id, T.NoReloj, T.Nombre, T.Activo, T.RutaFoto,
             T.IdLocalidad, Lo.Nombre, T.IdTurno, Tu.Nombre,
             T.IdPlanta, P.Nombre, T.IdLinea, Li.Nombre;

    SELECT COUNT(*) AS Total,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = ''Vigente''
                           THEN 1 ELSE 0 END), 0) AS Vigentes,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = ''Por vencer''
                           THEN 1 ELSE 0 END), 0) AS PorVencer,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = ''Vencida''
                           THEN 1 ELSE 0 END), 0) AS Vencidas,
           ISNULL(SUM(CASE WHEN EstadoCertificacion = ''Sin certificar''
                           THEN 1 ELSE 0 END), 0) AS SinCertificar
    FROM #TrabajadoresEstado;

    SELECT @TotalCount = COUNT(*) FROM #TrabajadoresEstado
    WHERE @MostrarPor = ''Todas'' OR EstadoCertificacion = @MostrarPor;
    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 0
        ELSE (@TotalCount - 1) / @PageSize + 1 END;
    IF @PageNumber > @TotalPages
        SET @PageNumber = CASE WHEN @TotalPages = 0 THEN 1
            ELSE @TotalPages END;
    DECLARE @Offset BIGINT = (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize;

    SELECT Id, NoReloj, Nombre, Activo, RutaFoto,
           IdLocalidad, NombreLocalidad, IdTurno, NombreTurno,
           IdPlanta, NombrePlanta, IdLinea, NombreLinea,
           EstadoCertificacion
    FROM #TrabajadoresEstado
    WHERE @MostrarPor = ''Todas'' OR EstadoCertificacion = @MostrarPor
    ORDER BY Nombre, Id
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END;');

    IF NOT EXISTS (SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadoresEstadoCertificacion')
          AND name = N'@PageNumber' AND is_output = 1)
       OR NOT EXISTS (SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadoresEstadoCertificacion')
          AND name = N'@TotalCount' AND is_output = 1)
       OR NOT EXISTS (SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadoresEstadoCertificacion')
          AND name = N'@AsOfDate')
        THROW 50053, 'Certification pagination procedure validation failed.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 14)
        INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
        VALUES (14, SYSUTCDATETIME(), 'Paginación de la lista de certificaciones');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
