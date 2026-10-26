SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF OBJECT_ID(N'dbo.Trabajador', N'U') IS NULL
    THROW 50010, 'Missing dbo.Trabajador; apply the existing base schema first.', 1;

IF COL_LENGTH(N'dbo.Trabajador', N'Activo') IS NULL
    ALTER TABLE dbo.Trabajador ADD Activo bit NOT NULL
        CONSTRAINT DF_Trabajador_Activo DEFAULT (1) WITH VALUES;

IF OBJECT_ID(N'dbo.ImportacionHdc', N'U') IS NULL
    THROW 50011, 'Missing dbo.ImportacionHdc; apply the existing HDC schema first.', 1;

IF OBJECT_ID(N'dbo.TrabajadorEstadoHistorial', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TrabajadorEstadoHistorial (
        Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
        IdTrabajador int NOT NULL,
        ActivoAnterior bit NULL,
        ActivoNuevo bit NOT NULL,
        FechaCambioUtc datetime2(3) NOT NULL,
        Origen varchar(20) NOT NULL,
        IdImportacionHdc int NULL,
        Motivo nvarchar(500) NULL,
        CONSTRAINT CK_TrabajadorEstadoHistorial_Origen
            CHECK (Origen IN ('HDC', 'MANUAL')),
        CONSTRAINT FK_TrabajadorEstadoHistorial_Trabajador
            FOREIGN KEY (IdTrabajador) REFERENCES dbo.Trabajador(Id),
        CONSTRAINT FK_TrabajadorEstadoHistorial_Importacion
            FOREIGN KEY (IdImportacionHdc) REFERENCES dbo.ImportacionHdc(Id)
    );
END;

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.TrabajadorEstadoHistorial')
      AND name = N'IX_TrabajadorEstadoHistorial_Trabajador_Fecha'
)
    CREATE INDEX IX_TrabajadorEstadoHistorial_Trabajador_Fecha
        ON dbo.TrabajadorEstadoHistorial(IdTrabajador, FechaCambioUtc);

IF OBJECT_ID(N'dbo.SchemaVersion', N'U') IS NULL
    CREATE TABLE dbo.SchemaVersion (
        Version int NOT NULL PRIMARY KEY,
        FechaAplicacion datetime2(0) NOT NULL
            CONSTRAINT DF_SchemaVersion_FechaAplicacion DEFAULT (SYSUTCDATETIME()),
        Descripcion varchar(300) NOT NULL
    );

COMMIT TRANSACTION;
GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajador]
    @NoReloj VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
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
        COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea

    FROM dbo.Trabajador AS T

    LEFT JOIN dbo.Localidad AS Lo
        ON Lo.Id = T.IdLocalidad

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    INNER JOIN dbo.Planta AS P
        ON P.Id = T.IdPlanta

    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    WHERE T.NoReloj = @NoReloj;
END;

GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadores]
    @TextoBuscar VARCHAR(100) = ''
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
        COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea

    FROM dbo.Trabajador AS T

    LEFT JOIN dbo.Localidad AS Lo
        ON Lo.Id = T.IdLocalidad

    INNER JOIN dbo.Turno AS Tu
        ON Tu.Id = T.IdTurno

    INNER JOIN dbo.Planta AS P
        ON P.Id = T.IdPlanta

    LEFT JOIN dbo.Linea AS Li
        ON Li.Id = T.IdLinea

    WHERE
        T.NoReloj LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR T.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR ISNULL(Lo.Nombre, '') LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR Tu.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR P.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
        OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
            LIKE '%' + ISNULL(@TextoBuscar, '') + '%'

    ORDER BY T.Nombre;
END;

GO
CREATE OR ALTER PROCEDURE [dbo].[spConsultarTrabajadoresEstadoCertificacion]
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH TrabajadoresEstado AS
    (
        SELECT TOP (300)
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
            COALESCE(Li.Nombre, 'SIN ASIGNAR') AS NombreLinea,

            CASE
                WHEN COUNT(C.Id) = 0
                    THEN 'Sin certificar'

                WHEN SUM
                (
                    CASE
                        WHEN C.FechaVencimiento
                             < CAST(GETDATE() AS DATE)
                        THEN 1
                        ELSE 0
                    END
                ) > 0
                    THEN 'Vencida'

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
                    THEN 'Por vencer'

                ELSE 'Vigente'
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

        WHERE
        (
            T.NoReloj LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR T.Nombre LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR ISNULL(Lo.Nombre, '')
                LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR Tu.Nombre
                LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR P.Nombre
                LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR COALESCE(Li.Nombre, 'SIN ASIGNAR')
                LIKE '%' + ISNULL(@TextoBuscar, '') + '%'
            OR ISNULL(@TextoBuscar, '') = ''
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

    SELECT *
    FROM TrabajadoresEstado
    WHERE
        @MostrarPor = 'Todas'
        OR EstadoCertificacion = @MostrarPor
    ORDER BY Nombre;
END;

GO
CREATE OR ALTER PROCEDURE [dbo].[spBorrarTrabajador]
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM TrabajadorEstadoHistorial WHERE IdTrabajador=@Id)
	BEGIN
		DELETE FROM Trabajador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este trabajador' AS [Nombre];
		END
END
GO

IF COALESCE(OBJECT_DEFINITION(OBJECT_ID(N'dbo.spConsultarTrabajador')), N'') NOT LIKE '%T.Activo%'
    OR COALESCE(OBJECT_DEFINITION(OBJECT_ID(N'dbo.spConsultarTrabajadores')), N'') NOT LIKE '%T.Activo%'
    OR COALESCE(OBJECT_DEFINITION(OBJECT_ID(N'dbo.spConsultarTrabajadoresEstadoCertificacion')), N'') NOT LIKE '%T.Activo%'
    OR COALESCE(OBJECT_DEFINITION(OBJECT_ID(N'dbo.spBorrarTrabajador')), N'') NOT LIKE '%TrabajadorEstadoHistorial%'
    THROW 50012, 'Worker procedures were not upgraded; schema version 10 was not recorded.', 1;

IF NOT EXISTS (SELECT 1 FROM dbo.SchemaVersion WHERE Version = 10)
    INSERT INTO dbo.SchemaVersion(Version, FechaAplicacion, Descripcion)
    VALUES (10, SYSUTCDATETIME(), 'Estado activo de trabajador e historial de cambios');
GO
