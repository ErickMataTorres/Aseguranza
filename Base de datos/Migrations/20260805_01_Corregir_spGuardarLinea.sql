USE [AseguranzaBD];
GO

CREATE OR ALTER PROCEDURE [dbo].[spGuardarLinea]
    @Id INT,
    @Nombre VARCHAR(100),
    @IdPlanta INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    SET @Nombre = UPPER(LTRIM(RTRIM(@Nombre)));

    IF @Nombre = ''
    BEGIN
        SELECT
            0 AS Id,
            'El nombre de la línea es obligatorio.' AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM Planta
        WHERE Id = @IdPlanta
    )
    BEGIN
        SELECT
            0 AS Id,
            'La planta seleccionada no existe.' AS Nombre;

        RETURN;
    END;

    IF EXISTS (
        SELECT 1
        FROM Linea
        WHERE IdPlanta = @IdPlanta
          AND Nombre = @Nombre
          AND Id <> @Id
    )
    BEGIN
        SELECT
            0 AS Id,
            'Ya existe una línea con ese nombre en la planta seleccionada.'
                AS Nombre;

        RETURN;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM Linea
        WHERE Id = @Id
    )
    BEGIN
        INSERT INTO Linea (
            Nombre,
            IdPlanta
        )
        VALUES (
            @Nombre,
            @IdPlanta
        );

        SELECT
            1 AS Id,
            'Se ha registrado correctamente' AS Nombre;

        RETURN;
    END;

    UPDATE Linea
    SET
        Nombre = @Nombre,
        IdPlanta = @IdPlanta
    WHERE Id = @Id;

    SELECT
        2 AS Id,
        'Se ha modificado correctamente' AS Nombre;
END;
GO