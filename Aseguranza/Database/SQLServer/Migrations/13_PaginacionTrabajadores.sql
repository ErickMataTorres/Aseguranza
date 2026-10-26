SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
        THROW 50040, 'Missing dbo.SchemaVersion; apply migrations 10 through 12 first.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 12)
        THROW 50041, 'Migration 12 must be deployed before migration 13.', 1;
    IF COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NULL
        THROW 50042, 'Missing dbo.Trabajador.Activo; apply migration 10 first.', 1;

    EXEC(N'CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadores]
    @TextoBuscar VARCHAR(100) = '''',
    @Activo BIT = NULL,
    @PageNumber INT = 1 OUTPUT,
    @PageSize INT = 100,
    @TotalCount INT = 0 OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF @PageSize IS NULL OR @PageSize <= 0 OR @PageSize > 100
        SET @PageSize = 100;
    IF @PageNumber IS NULL OR @PageNumber < 1
        SET @PageNumber = 1;

    SELECT
        T.Id,
        T.Nombre
    INTO #TrabajadoresFiltrados
    FROM dbo.Trabajador AS T
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    WHERE (@Activo IS NULL OR T.Activo = @Activo)
      AND (
        T.NoReloj LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR T.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR ISNULL(Lo.Nombre, '''') LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR Tu.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR P.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR COALESCE(Li.Nombre, ''SIN ASIGNAR'')
            LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%'')

    SELECT @TotalCount = COUNT(*) FROM #TrabajadoresFiltrados;

    DECLARE @TotalPages INT = CASE WHEN @TotalCount = 0 THEN 0
        ELSE (@TotalCount - 1) / @PageSize + 1 END;
    IF @PageNumber > @TotalPages
        SET @PageNumber = CASE WHEN @TotalPages = 0 THEN 1
            ELSE @TotalPages END;

    DECLARE @Offset BIGINT = (CONVERT(BIGINT, @PageNumber) - 1) * @PageSize;
    ;WITH Pagina AS
    (
        SELECT Id, Nombre FROM #TrabajadoresFiltrados
        ORDER BY Nombre, Id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
    )
    SELECT
        T.Id,
        T.NoReloj,
        T.Nombre,
        T.Activo,
        T.RutaFoto,
        T.IdLocalidad,
        Lo.Nombre AS NombreLocalidad,
        T.IdTurno,
        Tu.Nombre AS NombreTurno,
        T.IdPlanta,
        P.Nombre AS NombrePlanta,
        T.IdLinea,
        COALESCE(Li.Nombre, ''SIN ASIGNAR'') AS NombreLinea
    FROM Pagina AS F
    INNER JOIN dbo.Trabajador AS T ON T.Id = F.Id
    LEFT JOIN dbo.Localidad AS Lo ON Lo.Id = T.IdLocalidad
    INNER JOIN dbo.Turno AS Tu ON Tu.Id = T.IdTurno
    INNER JOIN dbo.Planta AS P ON P.Id = T.IdPlanta
    LEFT JOIN dbo.Linea AS Li ON Li.Id = T.IdLinea
    ORDER BY F.Nombre, F.Id;
END;');

    IF NOT EXISTS (
        SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadores')
          AND name = N'@PageNumber' AND is_output = 1
    ) OR NOT EXISTS (
        SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadores')
          AND name = N'@TotalCount' AND is_output = 1
    )
        THROW 50043, 'Worker pagination procedure validation failed.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 13)
        INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
        VALUES (13, SYSUTCDATETIME(), 'Paginación de la lista de trabajadores');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
