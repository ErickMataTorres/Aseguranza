USE [AseguranzaBD];
GO

CREATE OR ALTER PROCEDURE [dbo].[spConsultarExpedienteTrabajador]
    @IdTrabajador INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @IdTrabajador <= 0
    BEGIN
        RETURN;
    END;

    SELECT
        Id,
        IdTrabajador,
        NombreOriginal,
        NombreArchivo,
        Extension,
        RutaArchivo,
        TipoArchivo,
        Comentario,
        FechaRegistro,
        FechaModificacion
    FROM ExpedienteTrabajador
    WHERE IdTrabajador = @IdTrabajador
      AND Activo = 1
    ORDER BY FechaRegistro DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spGuardarExpedienteTrabajador]
    @IdTrabajador INT,
    @NombreOriginal VARCHAR(255),
    @NombreArchivo VARCHAR(255),
    @Extension VARCHAR(20),
    @RutaArchivo VARCHAR(500),
    @TipoArchivo VARCHAR(50) = NULL,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @NombreOriginal =
        LTRIM(RTRIM(ISNULL(@NombreOriginal, '')));

    SET @NombreArchivo =
        LTRIM(RTRIM(ISNULL(@NombreArchivo, '')));

    SET @Extension =
        LTRIM(RTRIM(ISNULL(@Extension, '')));

    SET @RutaArchivo =
        LTRIM(RTRIM(ISNULL(@RutaArchivo, '')));

    SET @TipoArchivo =
        NULLIF(LTRIM(RTRIM(@TipoArchivo)), '');

    SET @Comentario =
        NULLIF(LTRIM(RTRIM(@Comentario)), '');

    IF NOT EXISTS
    (
        SELECT 1
        FROM Trabajador
        WHERE Id = @IdTrabajador
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el trabajador seleccionado.' AS Nombre;

        RETURN;
    END;

    IF @NombreOriginal = ''
       OR @NombreArchivo = ''
       OR @Extension = ''
       OR @RutaArchivo = ''
    BEGIN
        SELECT
            0 AS Id,
            'Los datos principales del archivo están incompletos.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        INSERT INTO ExpedienteTrabajador
        (
            IdTrabajador,
            NombreOriginal,
            NombreArchivo,
            Extension,
            RutaArchivo,
            TipoArchivo,
            Comentario,
            Activo,
            FechaRegistro,
            FechaModificacion
        )
        VALUES
        (
            @IdTrabajador,
            @NombreOriginal,
            @NombreArchivo,
            @Extension,
            @RutaArchivo,
            @TipoArchivo,
            @Comentario,
            1,
            GETDATE(),
            NULL
        );

        SELECT
            1 AS Id,
            'Archivo agregado correctamente al expediente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo registrar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spReemplazarExpedienteTrabajador]
    @Id INT,
    @NombreOriginal VARCHAR(255),
    @NombreArchivo VARCHAR(255),
    @Extension VARCHAR(20),
    @RutaArchivo VARCHAR(500),
    @TipoArchivo VARCHAR(50) = NULL,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @NombreOriginal =
        LTRIM(RTRIM(ISNULL(@NombreOriginal, '')));

    SET @NombreArchivo =
        LTRIM(RTRIM(ISNULL(@NombreArchivo, '')));

    SET @Extension =
        LTRIM(RTRIM(ISNULL(@Extension, '')));

    SET @RutaArchivo =
        LTRIM(RTRIM(ISNULL(@RutaArchivo, '')));

    SET @TipoArchivo =
        NULLIF(LTRIM(RTRIM(@TipoArchivo)), '');

    SET @Comentario =
        NULLIF(LTRIM(RTRIM(@Comentario)), '');

    IF NOT EXISTS
    (
        SELECT 1
        FROM ExpedienteTrabajador
        WHERE Id = @Id
          AND Activo = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el archivo activo que intenta reemplazar.'
                AS Nombre;

        RETURN;
    END;

    IF @NombreOriginal = ''
       OR @NombreArchivo = ''
       OR @Extension = ''
       OR @RutaArchivo = ''
    BEGIN
        SELECT
            0 AS Id,
            'Los datos principales del archivo están incompletos.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE ExpedienteTrabajador
        SET
            NombreOriginal = @NombreOriginal,
            NombreArchivo = @NombreArchivo,
            Extension = @Extension,
            RutaArchivo = @RutaArchivo,
            TipoArchivo = @TipoArchivo,
            Comentario = @Comentario,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activo = 1;

        SELECT
            1 AS Id,
            'Archivo reemplazado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo reemplazar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spEliminarExpedienteTrabajador]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM ExpedienteTrabajador
        WHERE Id = @Id
          AND Activo = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró el archivo activo que intenta eliminar.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        /*
         * Eliminación lógica para conservar el historial
         * del registro en la base de datos.
         */
        UPDATE ExpedienteTrabajador
        SET
            Activo = 0,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activo = 1;

        SELECT
            1 AS Id,
            'Archivo eliminado correctamente del expediente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo eliminar el archivo. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO