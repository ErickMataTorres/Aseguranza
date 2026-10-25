USE [AseguranzaBD];
GO

CREATE OR ALTER PROCEDURE [dbo].[spGuardarCertificacion]
    @IdTrabajador INT,
    @IdProceso INT,
    @FechaCertificacion DATE,
    @IdCertificador INT,
    @Comentario VARCHAR(300) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @VigenciaMeses INT;
    DECLARE @FechaVencimiento DATE;
    DECLARE @IdCertificacionExistente INT;

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
            'El trabajador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    SELECT
        @VigenciaMeses = VigenciaMeses
    FROM Proceso
    WHERE Id = @IdProceso;

    IF @VigenciaMeses IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El proceso seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF @VigenciaMeses <= 0
    BEGIN
        SELECT
            0 AS Id,
            'La vigencia del proceso debe ser mayor que cero.' AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificador
        WHERE Id = @IdCertificador
    )
    BEGIN
        SELECT
            0 AS Id,
            'El certificador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF @FechaCertificacion IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'La fecha de certificación es obligatoria.' AS Nombre;

        RETURN;
    END;

    SELECT
        @IdCertificacionExistente = Id
    FROM Certificacion
    WHERE IdTrabajador = @IdTrabajador
      AND IdProceso = @IdProceso;

    IF @IdCertificacionExistente IS NOT NULL
       AND EXISTS
       (
            SELECT 1
            FROM CertificacionAnulacion
            WHERE IdCertificacion = @IdCertificacionExistente
              AND Activa = 1
              AND
              (
                  EsPermanente = 1
                  OR FechaFin IS NULL
                  OR FechaFin >= CAST(GETDATE() AS DATE)
              )
       )
    BEGIN
        SELECT
            0 AS Id,
            'Esta certificación se encuentra anulada. No se puede modificar ni renovar mientras tenga una anulación activa.'
                AS Nombre;

        RETURN;
    END;

    SET @FechaVencimiento =
        DATEADD(
            MONTH,
            @VigenciaMeses,
            @FechaCertificacion
        );

    BEGIN TRY
        BEGIN TRANSACTION;

        IF @IdCertificacionExistente IS NOT NULL
        BEGIN
            UPDATE Certificacion
            SET
                FechaCertificacion = @FechaCertificacion,
                FechaVencimiento = @FechaVencimiento,
                IdCertificador = @IdCertificador,
                Comentario = @Comentario
            WHERE Id = @IdCertificacionExistente;

            COMMIT TRANSACTION;

            SELECT
                2 AS Id,
                'Certificación renovada correctamente.' AS Nombre;

            RETURN;
        END;

        INSERT INTO Certificacion
        (
            IdTrabajador,
            IdProceso,
            FechaCertificacion,
            FechaVencimiento,
            IdCertificador,
            Comentario
        )
        VALUES
        (
            @IdTrabajador,
            @IdProceso,
            @FechaCertificacion,
            @FechaVencimiento,
            @IdCertificador,
            @Comentario
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación registrada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo guardar la certificación. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spBorrarCertificacion]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE Id = @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'No existe la certificación que intenta borrar.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM CertificacionAnulacion
        WHERE IdCertificacion = @Id;

        DELETE FROM Certificacion
        WHERE Id = @Id;

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Certificación eliminada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        SELECT
            0 AS Id,
            'No se pudo borrar la certificación. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO