SET XACT_ABORT ON;
BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
        THROW 50030, 'Missing dbo.SchemaVersion; apply migrations 10 and 11 first.', 1;
    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 11)
        THROW 50031, 'Migration 11 must be deployed before migration 12.', 1;
    IF COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NULL
        THROW 50032, 'Missing dbo.Trabajador.Activo; apply migration 10 first.', 1;

    EXEC(N'CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadores]
    @TextoBuscar VARCHAR(100) = '''',
    @Activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (100)
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

    FROM dbo.Trabajador AS T

    LEFT JOIN dbo.Localidad AS Lo
        ON Lo.Id = T.IdLocalidad

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    INNER JOIN dbo.Planta AS P
        ON P.Id = T.IdPlanta

    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    WHERE (@Activo IS NULL OR T.Activo = @Activo)
      AND (
        T.NoReloj LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR T.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR ISNULL(Lo.Nombre, '''') LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR Tu.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR P.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
        OR COALESCE(Li.Nombre, ''SIN ASIGNAR'')
            LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%'')

    ORDER BY T.Nombre;
END;');

    EXEC(N'CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadoresEstadoCertificacion]
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100),
    @Activo BIT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH TrabajadoresEstado AS
    (
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
            COALESCE(Li.Nombre, ''SIN ASIGNAR'') AS NombreLinea,

            CASE
                WHEN COUNT(C.Id) = 0
                    THEN ''Sin certificar''

                WHEN SUM
                (
                    CASE
                        WHEN C.FechaVencimiento
                             < CAST(GETDATE() AS DATE)
                        THEN 1
                        ELSE 0
                    END
                ) > 0
                    THEN ''Vencida''

                WHEN SUM
                (
                    CASE
                        WHEN C.FechaVencimiento
                             BETWEEN CAST(GETDATE() AS DATE)
                             AND DATEADD(
                                 DAY,
                                 30,
                                 CAST(GETDATE() AS DATE)
                             )
                        THEN 1
                        ELSE 0
                    END
                ) > 0
                    THEN ''Por vencer''

                ELSE ''Vigente''
            END AS EstadoCertificacion

        FROM dbo.Trabajador AS T

        LEFT JOIN dbo.Localidad AS Lo
            ON Lo.Id = T.IdLocalidad

        INNER JOIN dbo.Turno AS Tu
            ON Tu.Id = T.IdTurno

        INNER JOIN dbo.Planta AS P
            ON P.Id = T.IdPlanta

        LEFT JOIN dbo.Linea AS Li
            ON Li.Id = T.IdLinea

        LEFT JOIN dbo.Certificacion AS C
            ON C.IdTrabajador = T.Id
           AND NOT EXISTS
           (
                SELECT 1
                FROM dbo.CertificacionAnulacion AS CA
                WHERE CA.IdCertificacion = C.Id
                  AND CA.Activa = 1
                  AND
                  (
                      CA.EsPermanente = 1
                      OR CA.FechaFin IS NULL
                      OR CAST(CA.FechaFin AS DATE)
                         >= CAST(GETDATE() AS DATE)
                  )
           )

        WHERE (@Activo IS NULL OR T.Activo = @Activo)
        AND (
            T.NoReloj LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR T.Nombre LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR ISNULL(Lo.Nombre, '''')
                LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR Tu.Nombre
                LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR P.Nombre
                LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR COALESCE(Li.Nombre, ''SIN ASIGNAR'')
                LIKE ''%'' + ISNULL(@TextoBuscar, '''') + ''%''
            OR ISNULL(@TextoBuscar, '''') = ''''
        )

        GROUP BY
            T.Id,
            T.NoReloj,
            T.Nombre,
            T.Activo,
            T.RutaFoto,
            T.IdLocalidad,
            Lo.Nombre,
            T.IdTurno,
            Tu.Nombre,
            T.IdPlanta,
            P.Nombre,
            T.IdLinea,
            Li.Nombre
    )

    SELECT TOP (300) *
    FROM TrabajadoresEstado
    WHERE
        @MostrarPor = ''Todas''
        OR EstadoCertificacion = @MostrarPor
    ORDER BY Nombre;
END;');

    IF NOT EXISTS (
        SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadores')
          AND name = N'@Activo'
    ) OR NOT EXISTS (
        SELECT 1 FROM sys.parameters
        WHERE object_id = OBJECT_ID(N'dbo.spConsultarTrabajadoresEstadoCertificacion')
          AND name = N'@Activo'
    )
        THROW 50033, 'Worker-status procedure validation failed.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 12)
        INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
        VALUES (12, SYSUTCDATETIME(),
            'Filtros de personal activo e inactivo en listas operativas');

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;