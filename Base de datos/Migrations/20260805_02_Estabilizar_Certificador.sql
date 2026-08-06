USE [AseguranzaBD];
GO

CREATE OR ALTER PROCEDURE [dbo].[spGuardarCertificador]
    @NoReloj VARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @IdTrabajador INT;

    SET @NoReloj = LTRIM(RTRIM(@NoReloj));

    IF @NoReloj = ''
    BEGIN
        SELECT
            0 AS Id,
            'El número de reloj es obligatorio.' AS Nombre;

        RETURN;
    END;

    SELECT
        @IdTrabajador = Id
    FROM Trabajador
    WHERE NoReloj = @NoReloj;

    IF @IdTrabajador IS NULL
    BEGIN
        SELECT
            0 AS Id,
            'El trabajador no existe.' AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF EXISTS
        (
            SELECT 1
            FROM Certificador WITH (UPDLOCK, HOLDLOCK)
            WHERE IdTrabajador = @IdTrabajador
        )
        BEGIN
            COMMIT TRANSACTION;

            SELECT
                2 AS Id,
                'El trabajador ya es certificador.' AS Nombre;

            RETURN;
        END;

        INSERT INTO Certificador
        (
            IdTrabajador
        )
        VALUES
        (
            @IdTrabajador
        );

        COMMIT TRANSACTION;

        SELECT
            1 AS Id,
            'Se ha guardado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT
            0 AS Id,
            'No se pudo guardar el certificador. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE [dbo].[spBorrarCertificador]
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificador
        WHERE Id = @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'El certificador seleccionado no existe.' AS Nombre;

        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE IdCertificador = @Id
    )
    BEGIN
        SELECT
            2 AS Id,
            'No se puede borrar porque hay certificaciones asociadas a este certificador.'
                AS Nombre;

        RETURN;
    END;

    BEGIN TRY
        DELETE FROM Certificador
        WHERE Id = @Id;

        SELECT
            1 AS Id,
            'Se ha borrado correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        SELECT
            0 AS Id,
            'No se pudo borrar el certificador. Detalle: '
                + ERROR_MESSAGE() AS Nombre;
    END CATCH;
END;
GO