USE [AseguranzaBD];
GO

CREATE OR ALTER PROCEDURE [dbo].[spConsultarAnulacionPorCertificacion]
    @IdCertificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
        Id,
        IdCertificacion,
        TipoAnulacion,
        FechaInicio,
        FechaFin,
        EsPermanente,
        Comentario,
        Activa,
        FechaRegistro,
        FechaModificacion
    FROM CertificacionAnulacion
    WHERE IdCertificacion = @IdCertificacion
      AND Activa = 1
      AND
      (
          EsPermanente = 1
          OR FechaFin IS NULL
          OR FechaFin >= CONVERT(DATE, GETDATE())
      )
    ORDER BY
        FechaRegistro DESC,
        Id DESC;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spGuardarCertificacionAnulacion]
    @IdCertificacion INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @TipoAnulacion =
        LTRIM(RTRIM(ISNULL(@TipoAnulacion, '')));

    SET @Comentario =
        LTRIM(RTRIM(ISNULL(@Comentario, '')));

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE Id = @IdCertificacion
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró la certificación seleccionada.'
                AS Nombre;

        RETURN;
    END;

    IF @TipoAnulacion = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar un tipo de anulación.'
                AS Nombre;

        RETURN;
    END;

    IF @FechaInicio IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de inicio es obligatoria.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe indicar si la anulación es permanente.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar una fecha fin para una anulación temporal.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin < @FechaInicio
    BEGIN
        SELECT
            0 AS Id,
            'La fecha fin no puede ser menor que la fecha inicio.'
                AS Nombre;

        RETURN;
    END;

    IF @Comentario = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe escribir un comentario de la anulación.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        /*
         * Conserva el historial y evita que queden
         * dos anulaciones activas para la certificación.
         */
        UPDATE CertificacionAnulacion
        SET
            Activa = 0,
            FechaModificacion = GETDATE()
        WHERE IdCertificacion = @IdCertificacion
          AND Activa = 1;

        INSERT INTO CertificacionAnulacion
        (
            IdCertificacion,
            TipoAnulacion,
            FechaInicio,
            FechaFin,
            EsPermanente,
            Comentario,
            Activa,
            FechaRegistro,
            FechaModificacion
        )
        VALUES
        (
            @IdCertificacion,
            @TipoAnulacion,
            @FechaInicio,
            CASE
                WHEN @EsPermanente = 1
                    THEN NULL
                ELSE @FechaFin
            END,
            @EsPermanente,
            @Comentario,
            1,
            GETDATE(),
            NULL
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación anulada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo guardar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spModificarCertificacionAnulacion]
    @Id INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @TipoAnulacion =
        LTRIM(RTRIM(ISNULL(@TipoAnulacion, '')));

    SET @Comentario =
        LTRIM(RTRIM(ISNULL(@Comentario, '')));

    IF NOT EXISTS
    (
        SELECT 1
        FROM CertificacionAnulacion
        WHERE Id = @Id
          AND Activa = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró una anulación activa para modificar.'
                AS Nombre;

        RETURN;
    END;

    IF @TipoAnulacion = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar un tipo de anulación.'
                AS Nombre;

        RETURN;
    END;

    IF @FechaInicio IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de inicio es obligatoria.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'Debe seleccionar una fecha fin para una anulación temporal.'
                AS Nombre;

        RETURN;
    END;

    IF @EsPermanente = 0
       AND @FechaFin < @FechaInicio
    BEGIN
        SELECT
            0 AS Id,
            'La fecha fin no puede ser menor que la fecha inicio.'
                AS Nombre;

        RETURN;
    END;

    IF @Comentario = ''
    BEGIN
        SELECT
            0 AS Id,
            'Debe escribir un comentario de la anulación.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE CertificacionAnulacion
        SET
            TipoAnulacion = @TipoAnulacion,
            FechaInicio = @FechaInicio,
            FechaFin =
                CASE
                    WHEN @EsPermanente = 1
                        THEN NULL
                    ELSE @FechaFin
                END,
            EsPermanente = @EsPermanente,
            Comentario = @Comentario,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activa = 1;

        SELECT
            1 AS Id,
            'Anulación modificada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo modificar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spEliminarCertificacionAnulacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM CertificacionAnulacion
        WHERE Id = @Id
          AND Activa = 1
    )
    BEGIN
        SELECT
            0 AS Id,
            'No se encontró una anulación activa para eliminar.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        UPDATE CertificacionAnulacion
        SET
            Activa = 0,
            FechaModificacion = GETDATE()
        WHERE Id = @Id
          AND Activa = 1;

        SELECT
            1 AS Id,
            'Anulación eliminada correctamente.'
                AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo eliminar la anulación. Detalle: '
                + ERROR_MESSAGE()
                AS Nombre;
    END CATCH;
END;
GO