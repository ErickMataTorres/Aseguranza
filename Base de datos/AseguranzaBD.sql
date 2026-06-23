IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AseguranzaBD')
BEGIN
    CREATE DATABASE AseguranzaBD;
END
GO
CREATE DATABASE AseguranzaBD
USE AseguranzaBD
-------------------------------------------
CREATE TABLE Localidad
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
CREATE OR ALTER PROCEDURE spConsultarLocalidades
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Localidad WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE OR ALTER PROCEDURE spGuardarLocalidad
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Localidad WHERE Id=@Id)
	BEGIN
		INSERT INTO Localidad (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Localidad SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE OR ALTER PROCEDURE spBorrarLocalidad
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdLocalidad=@Id)
	BEGIN
		DELETE FROM Localidad WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta localidad' AS [Nombre];
		END
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Turno
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
CREATE OR ALTER PROCEDURE spConsultarTurnos
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Turno WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE OR ALTER PROCEDURE spGuardarTurno
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Turno WHERE Id=@Id)
	BEGIN
		INSERT INTO Turno (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Turno SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE OR ALTER PROCEDURE spBorrarTurno
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdTurno=@Id)
	BEGIN
		DELETE FROM Turno WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este turno' AS [Nombre];
		END
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Planta
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL
)
CREATE OR ALTER PROCEDURE spConsultarPlantas
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Planta WHERE Nombre LIKE '%'+@TextoBuscar+'%';
END
CREATE OR ALTER PROCEDURE spGuardarPlanta
@Id INT,
@Nombre VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Planta WHERE Id=@Id)
	BEGIN
		INSERT INTO Planta (Nombre) VALUES (@Nombre);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Planta SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE OR ALTER PROCEDURE spBorrarPlanta
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Linea WHERE IdPlanta=@Id)
	BEGIN
		DELETE FROM Planta WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta planta' AS [Nombre];
		END
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Linea
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	Nombre VARCHAR(100) NOT NULL,
	IdPlanta INT NOT NULL FOREIGN KEY REFERENCES Planta(Id),
	CONSTRAINT UQ_Linea_Planta_Nombre
	UNIQUE (IdPlanta, Nombre)
)
CREATE OR ALTER PROCEDURE spConsultarLineas
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
        Linea.Id,
        Linea.Nombre,
        Linea.IdPlanta,
        Planta.Nombre AS [NombrePlanta]
    FROM Linea
    INNER JOIN Planta ON Linea.IdPlanta = Planta.Id
	WHERE Linea.Nombre LIKE '%' + @TextoBuscar + '%'
        OR Planta.Nombre LIKE '%' + @TextoBuscar + '%';
END
CREATE OR ALTER PROCEDURE spConsultarLineasPorPlanta
@IdPlanta INT
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, Nombre FROM Linea WHERE IdPlanta=@IdPlanta
END
CREATE OR ALTER PROCEDURE spGuardarLinea
@Id INT,
@Nombre VARCHAR(100),
@IdPlanta INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Linea WHERE Id=@Id)
	BEGIN
		INSERT INTO Linea (Nombre, IdPlanta) VALUES (@Nombre, @IdPlanta);
		SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Linea SET Nombre=@Nombre WHERE Id=@Id;
			SELECT '2' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END
CREATE OR ALTER PROCEDURE spBorrarLinea
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE IdLinea=@Id)
	BEGIN
		DELETE FROM Linea WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a esta linea' AS [Nombre];
		END
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Trabajador
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	NoReloj VARCHAR(10) UNIQUE NOT NULL,
	Nombre VARCHAR(200) NOT NULL,
	RutaFoto VARCHAR(300),
	IdLocalidad INT NOT NULL FOREIGN KEY REFERENCES Localidad(Id),
	IdTurno INT NOT NULL FOREIGN KEY REFERENCES Turno(Id),
	IdLinea INT NOT NULL FOREIGN KEY REFERENCES Linea(Id)
)
CREATE OR ALTER PROCEDURE spConsultarTrabajadores
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT TOP(100) Trabajador.Id, Trabajador.NoReloj, Trabajador.Nombre, Trabajador.RutaFoto, Trabajador.IdLocalidad, Localidad.Nombre AS [NombreLocalidad], Trabajador.IdTurno, 
	Turno.Nombre AS [NombreTurno], Linea.IdPlanta, Planta.Nombre AS [NombrePlanta], Trabajador.IdLinea, Linea.Nombre AS [NombreLinea] FROM Trabajador
	INNER JOIN Localidad ON Localidad.Id = Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea
	INNER JOIN Planta ON Planta.Id = Linea.IdPlanta WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Localidad.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%' OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%';
END
--ALTER PROCEDURE spConsultarTrabajador
--@NoReloj VARCHAR(10)
--AS
--BEGIN
--	SELECT Trabajador.Id, Trabajador.Nombre, Trabajador.RutaFoto, Localidad.Id AS [IdLocalidad],Localidad.Nombre as [NombreLocalidad], Turno.Id AS [IdTurno] ,Turno.Nombre AS [NombreTurno], 
--	Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta], Linea.Id AS [IdLinea], Linea.Nombre AS [NombreLinea] FROM Trabajador INNER JOIN Localidad
--	ON Localidad.Id=Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id=Trabajador.IdTurno 
--	INNER JOIN Linea ON Linea.Id=Trabajador.IdLinea INNER JOIN Planta ON Planta.Id=Linea.IdPlanta WHERE Trabajador.NoReloj=@NoReloj;
--END

CREATE OR ALTER PROCEDURE spGuardarTrabajador
@Id INT,
@NoReloj VARCHAR(10),
@Nombre VARCHAR(100),
@RutaFoto VARCHAR(200),
@IdLocalidad INT,
@IdTurno INT,
@IdLinea INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT Id FROM Trabajador WHERE Id=@Id)
	BEGIN
		IF NOT EXISTS (SELECT 1 FROM Trabajador WHERE NoReloj=@NoReloj)
		BEGIN
			INSERT INTO Trabajador (NoReloj, Nombre, RutaFoto, IdLocalidad, IdTurno, IdLinea) VALUES (@NoReloj, @Nombre, @RutaFoto, @IdLocalidad, @IdTurno, @IdLinea);
			SELECT '1' AS [Id], 'Se ha registrado correctamente' AS [Nombre];	
		END	ELSE
			BEGIN
				SELECT '2' AS [Id], 'Ya existe un trabajador registrado con ese numero de reloj' AS [Nombre];
			END
	END ELSE
		BEGIN
			UPDATE Trabajador SET Nombre=@Nombre, RutaFoto=@RutaFoto, IdLocalidad=@IdLocalidad, IdTurno=@IdTurno, IdLinea=@IdLinea WHERE Id=@Id AND NoReloj=@NoReloj;
			SELECT '3' AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END

CREATE OR ALTER PROCEDURE spBorrarTrabajador
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@Id) AND NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdTrabajador=@Id)
	BEGIN
		DELETE FROM Trabajador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este trabajador' AS [Nombre];
		END
END

CREATE OR ALTER PROCEDURE spConsultarTrabajador
@NoReloj VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT Id FROM Trabajador WHERE NoReloj=@NoReloj)
	BEGIN
		SELECT Trabajador.Nombre, Trabajador.RutaFoto, Localidad.Nombre, Turno.Nombre, Planta.Nombre, Linea.Nombre FROM Trabajador
		INNER JOIN Localidad ON Localidad.Id=Trabajador.IdLocalidad INNER JOIN Turno ON Turno.Id=Trabajador.IdTurno
		INNER JOIN Linea ON Linea.Id=Trabajador.IdLinea INNER JOIN Planta ON Planta.Id=LInea.IdPlanta WHERE Trabajador.NoReloj=@NoReloj;
	END ELSE
		BEGIN
			SELECT 1 AS [Id], 'No existe trabajador con ese número de reloj' AS [Nombre];
		END
END


CREATE OR ALTER PROCEDURE spConsultarTrabajadoresEstadoCertificacion
    @MostrarPor VARCHAR(20),
    @TextoBuscar VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH CTE_Estado AS
    (
        SELECT TOP(300)
            T.Id,
            T.NoReloj,
            T.Nombre,
            T.RutaFoto,
            T.IdLocalidad,
            L.Nombre AS NombreLocalidad,
            T.IdTurno,
            Tu.Nombre AS NombreTurno,
            Li.IdPlanta,
            P.Nombre AS NombrePlanta,
            T.IdLinea,
            Li.Nombre AS NombreLinea,

            CASE
                WHEN COUNT(C.Id) = 0 THEN 'Sin certificar'

                WHEN SUM(
                    CASE 
                        WHEN C.FechaVencimiento < CAST(GETDATE() AS DATE)
                        THEN 1 ELSE 0 
                    END
                ) > 0 THEN 'Vencida'

                WHEN SUM(
                    CASE 
                        WHEN C.FechaVencimiento BETWEEN CAST(GETDATE() AS DATE)
                             AND DATEADD(DAY, 30, CAST(GETDATE() AS DATE))
                        THEN 1 ELSE 0 
                    END
                ) > 0 THEN 'Por vencer'

                ELSE 'Vigente'
            END AS EstadoCertificacion

        FROM Trabajador T
        INNER JOIN Localidad L 
            ON L.Id = T.IdLocalidad
        INNER JOIN Turno Tu 
            ON Tu.Id = T.IdTurno
        INNER JOIN Linea Li 
            ON Li.Id = T.IdLinea
        INNER JOIN Planta P 
            ON P.Id = Li.IdPlanta

        LEFT JOIN Certificacion C 
            ON C.IdTrabajador = T.Id
           AND NOT EXISTS
           (
                SELECT 1
                FROM CertificacionAnulacion CA
                WHERE CA.IdCertificacion = C.Id
                  AND CA.Activa = 1
                  AND (
                        CA.EsPermanente = 1
                        OR CA.FechaFin IS NULL
                        OR CA.FechaFin >= CAST(GETDATE() AS DATE)
                      )
           )

        WHERE
        (
            T.NoReloj LIKE '%' + @TextoBuscar + '%'
            OR T.Nombre LIKE '%' + @TextoBuscar + '%'
            OR L.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Tu.Nombre LIKE '%' + @TextoBuscar + '%'
            OR P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR Li.Nombre LIKE '%' + @TextoBuscar + '%'
            OR @TextoBuscar = ''
        )

        GROUP BY
            T.Id,
            T.NoReloj,
            T.Nombre,
            T.RutaFoto,
            T.IdLocalidad,
            L.Nombre,
            T.IdTurno,
            Tu.Nombre,
            Li.IdPlanta,
            P.Nombre,
            T.IdLinea,
            Li.Nombre
    )

    SELECT *
    FROM CTE_Estado
    WHERE @MostrarPor = 'Todas'
       OR EstadoCertificacion = @MostrarPor;
END
GO

-------------------------------------------

-------------------------------------------
CREATE TABLE Certificador
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	IdTrabajador INT NOT NULL FOREIGN KEY REFERENCES Trabajador(Id)
)
CREATE OR ALTER PROCEDURE spConsultarCertificadores
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Certificador.Id, Certificador.IdTrabajador, Trabajador.NoReloj, Trabajador.Nombre AS [NombreTrabajador], Trabajador.RutaFoto,
	Trabajador.IdTurno, Turno.Nombre AS [NombreTurno], Planta.Id AS [IdPlanta], Planta.Nombre AS [NombrePlanta], Linea.Id AS [IdLinea], 
	Linea.Nombre AS [NombreLinea] FROM Certificador INNER JOIN Trabajador ON Certificador.IdTrabajador = Trabajador.Id
	INNER JOIN Turno ON Turno.Id = Trabajador.IdTurno INNER JOIN Linea ON Linea.Id = Trabajador.IdLinea INNER JOIN Planta ON Planta.Id = Linea.IdPlanta
	WHERE Trabajador.NoReloj LIKE '%'+@TextoBuscar+'%' OR Trabajador.Nombre LIKE '%'+@TextoBuscar+'%' OR Turno.Nombre LIKE '%'+@TextoBuscar+'%'
	OR Planta.Nombre LIKE '%'+@TextoBuscar+'%' OR Linea.Nombre LIKE '%'+@TextoBuscar+'%'
END

CREATE OR ALTER PROCEDURE spGuardarCertificador
@NoReloj VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @IdTrabajador INT
	SELECT @IdTrabajador = Id FROM Trabajador WHERE NoReloj=@NoReloj;

	IF NOT EXISTS (SELECT 1 FROM Certificador WHERE IdTrabajador=@IdTrabajador)
	BEGIN
		INSERT INTO Certificador (IdTrabajador) VALUES (@IdTrabajador);
		SELECT '1' AS [Id], 'Se ha guardado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'El trabajador ya es certificador' AS [Nombre];	
		END
END

CREATE OR ALTER PROCEDURE spBorrarCertificador
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdCertificador=@Id)
	BEGIN
		DELETE FROM Certificador WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este certificador' AS [Nombre];
		END
END
-------------------------------------------

-------------------------------------------
CREATE TABLE Proceso
(
	Id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	Nombre VARCHAR(100) NOT NULL,
	Descripcion VARCHAR(300),
	VigenciaMeses INT NOT NULL
)

CREATE OR ALTER PROCEDURE spConsultarProcesos
@TextoBuscar VARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM Proceso WHERE Nombre LIKE '%'+@TextoBuscar+'%'
END

CREATE OR ALTER PROCEDURE spGuardarProceso
@Id INT,
@Nombre VARCHAR(100),
@Descripcion VARCHAR(300),
@VigenciaMeses INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Proceso WHERE Id=@Id)
	BEGIN
		INSERT INTO Proceso (Nombre, Descripcion, VigenciaMeses) VALUES (@Nombre, @Descripcion, @VigenciaMeses);
		SELECT 1 AS [Id], 'Se ha guardado correctamente' AS [Nombre];
	END ELSE
		BEGIN
			UPDATE Proceso SET Nombre=@Nombre, Descripcion=@Descripcion, VigenciaMeses=@VigenciaMeses WHERE Id=@Id;
			SELECT 2 AS [Id], 'Se ha modificado correctamente' AS [Nombre];
		END
END

CREATE OR ALTER PROCEDURE spBorrarProceso
@Id INT
AS
BEGIN
	SET NOCOUNT ON;
	IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE IdProceso=@Id)
	BEGIN
		DELETE FROM Proceso WHERE Id=@Id
		SELECT '1' AS [Id], 'Se ha borrado correctamente' AS [Nombre];	
	END ELSE
		BEGIN
			SELECT '2' AS [Id], 'No se puede borrar por que hay registros asociados a este proceso' AS [Nombre];
		END
END

-------------------------------------------

-------------------------------------------
CREATE TABLE Certificacion
(
	Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	IdTrabajador INT NOT NULL FOREIGN KEY REFERENCES Trabajador(Id),
	IdProceso INT NOT NULL FOREIGN KEY REFERENCES Proceso(Id),
	FechaCertificacion DATE NOT NULL,
	FechaVencimiento DATE NOT NULL,
	IdCertificador INT NOT NULL FOREIGN KEY REFERENCES Certificador(Id),
	Comentario VARCHAR(300),
	    CONSTRAINT UQ_Certificacion_Trabajador_Proceso
        UNIQUE (IdTrabajador, IdProceso),

    --  Validación de fechas (opcional)
    CONSTRAINT CK_Fechas_Certificacion
        CHECK (FechaVencimiento > FechaCertificacion)
)


CREATE OR ALTER PROCEDURE spConsultarCertificacionesPorTrabajador
    @IdTrabajador INT,
    @TextoBuscar VARCHAR(100) = ''
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.Id,
        C.IdTrabajador,
        P.Id AS IdProceso,
        P.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), C.FechaVencimiento) AS DiasRestantes,
        C.Comentario,
        C.IdCertificador,
        TCert.Nombre AS NombreCertificador,

        CASE 
            WHEN CA.Id IS NOT NULL THEN 1
            ELSE 0
        END AS EstaAnulada,

        CA.Id AS IdAnulacion,
        CA.TipoAnulacion,
        CA.FechaInicio AS FechaInicioAnulacion,
        CA.FechaFin AS FechaFinAnulacion,
        CA.EsPermanente,
        CA.Comentario AS ComentarioAnulacion

    FROM Certificacion C
    INNER JOIN Proceso P 
        ON P.Id = C.IdProceso
    INNER JOIN Certificador Cert 
        ON Cert.Id = C.IdCertificador
    INNER JOIN Trabajador TCert 
        ON TCert.Id = Cert.IdTrabajador
    OUTER APPLY
    (
        SELECT TOP 1
            A.Id,
            A.TipoAnulacion,
            A.FechaInicio,
            A.FechaFin,
            A.EsPermanente,
            A.Comentario
        FROM CertificacionAnulacion A
        WHERE A.IdCertificacion = C.Id
          AND A.Activa = 1
          AND (
                A.EsPermanente = 1
                OR A.FechaFin IS NULL
                OR A.FechaFin >= CAST(GETDATE() AS DATE)
              )
        ORDER BY A.FechaRegistro DESC
    ) CA
    WHERE C.IdTrabajador = @IdTrabajador
      AND (
            @TextoBuscar = ''
            OR P.Nombre LIKE '%' + @TextoBuscar + '%'
            OR C.Comentario LIKE '%' + @TextoBuscar + '%'
            OR TCert.Nombre LIKE '%' + @TextoBuscar + '%'
            OR CA.Comentario LIKE '%' + @TextoBuscar + '%'
          )
    ORDER BY C.FechaVencimiento;
END
GO



CREATE OR ALTER PROCEDURE spConsultarVerificacionNoReloj
    @NoReloj VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        T.Id AS IdTrabajador,
        T.NoReloj,
        T.Nombre,
        T.RutaFoto,

        L.Nombre AS NombreLocalidad,
        Tu.Nombre AS NombreTurno,
        Pta.Nombre AS NombrePlanta,
        Li.Nombre AS NombreLinea,

        C.Id AS IdCertificacion,
        Pr.Id AS IdProceso,
        Pr.Nombre AS Proceso,
        C.FechaCertificacion,
        C.FechaVencimiento,
        DATEDIFF(DAY, CAST(GETDATE() AS DATE), C.FechaVencimiento) AS DiasRestantes,
        C.Comentario,

        C.IdCertificador,
        TCert.Nombre AS NombreCertificador

    FROM Trabajador T
    INNER JOIN Localidad L
        ON L.Id = T.IdLocalidad
    INNER JOIN Turno Tu
        ON Tu.Id = T.IdTurno
    INNER JOIN Linea Li
        ON Li.Id = T.IdLinea
    INNER JOIN Planta Pta
        ON Pta.Id = Li.IdPlanta

    LEFT JOIN Certificacion C
        ON C.IdTrabajador = T.Id
    LEFT JOIN Proceso Pr
        ON Pr.Id = C.IdProceso
    LEFT JOIN Certificador Cert
        ON Cert.Id = C.IdCertificador
    LEFT JOIN Trabajador TCert
        ON TCert.Id = Cert.IdTrabajador

    WHERE T.NoReloj = @NoReloj
      AND (
            C.Id IS NULL
            OR NOT EXISTS
            (
                SELECT 1
                FROM CertificacionAnulacion CA
                WHERE CA.IdCertificacion = C.Id
                  AND CA.Activa = 1
                  AND (
                        CA.EsPermanente = 1
                        OR CA.FechaFin IS NULL
                        OR CA.FechaFin >= CAST(GETDATE() AS DATE)
                      )
            )
          )

    ORDER BY Pr.Nombre;
END
GO



CREATE OR ALTER PROCEDURE spGuardarCertificacion
(
    @IdTrabajador INT,
    @IdProceso INT,
    @FechaCertificacion DATE,
    @IdCertificador INT,
    @Comentario VARCHAR(300)
)
AS
BEGIN
    SET NOCOUNT ON;

        DECLARE @IdCertificacionExistente INT;

    SELECT @IdCertificacionExistente = Id
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
              AND (
                    EsPermanente = 1
                    OR FechaFin IS NULL
                    OR FechaFin >= CAST(GETDATE() AS DATE)
                  )
       )
    BEGIN
        SELECT
            0 AS Id,
            'Esta certificación se encuentra anulada. No se puede agregar, modificar ni renovar mientras tenga una anulación activa.' AS Nombre;
        RETURN;
    END

    DECLARE @VigenciaMeses INT;
    DECLARE @FechaVencimiento DATE;

    -- Obtener vigencia desde Proceso
    SELECT @VigenciaMeses = VigenciaMeses
    FROM Proceso
    WHERE Id = @IdProceso;

    SET @FechaVencimiento =
        DATEADD(MONTH, @VigenciaMeses, @FechaCertificacion);

    IF EXISTS (
        SELECT 1
        FROM Certificacion
        WHERE IdTrabajador = @IdTrabajador
          AND IdProceso = @IdProceso
    )
    BEGIN
        -- RENOVAR
        UPDATE Certificacion
        SET
            FechaCertificacion = @FechaCertificacion,
            FechaVencimiento = @FechaVencimiento,
            IdCertificador = @IdCertificador,
            Comentario = @Comentario
        WHERE
            IdTrabajador = @IdTrabajador
            AND IdProceso = @IdProceso;

        SELECT 2 AS [Id], 'Certificación renovada correctamente' AS [Nombre];
    END
    ELSE
    BEGIN
        -- INSERTAR
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

        SELECT 1 AS [Id], 'Certificación registrada correctamente' AS [Nombre];
    END
END

CREATE OR ALTER PROCEDURE spBorrarCertificacion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE Id = @Id)
        BEGIN
            ROLLBACK TRANSACTION;

            SELECT 
                0 AS Id,
                'No existe la certificación que intenta borrar.' AS Nombre;

            RETURN;
        END

        -- Primero borrar historial de anulaciones relacionado
        DELETE FROM CertificacionAnulacion
        WHERE IdCertificacion = @Id;

        -- Después borrar la certificación
        DELETE FROM Certificacion
        WHERE Id = @Id;

        COMMIT TRANSACTION;

        SELECT 
            1 AS Id,
            'Certificación eliminada correctamente.' AS Nombre;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        SELECT 
            0 AS Id,
            'No se pudo eliminar la certificación. Detalle: ' + ERROR_MESSAGE() AS Nombre;
    END CATCH
END

-------------------------------------------

-------------------------------------------
CREATE TABLE ExpedienteTrabajador
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    IdTrabajador INT NOT NULL,

    NombreOriginal VARCHAR(255) NOT NULL,
    NombreArchivo VARCHAR(255) NOT NULL,
    Extension VARCHAR(20) NOT NULL,
    RutaArchivo VARCHAR(500) NOT NULL,
    TipoArchivo VARCHAR(50) NULL,
    Comentario VARCHAR(300) NULL,

    Activo BIT NOT NULL DEFAULT 1,

    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,

    CONSTRAINT FK_ExpedienteTrabajador_Trabajador
        FOREIGN KEY (IdTrabajador) REFERENCES Trabajador(Id)
);
GO

CREATE INDEX IX_ExpedienteTrabajador_IdTrabajador
ON ExpedienteTrabajador(IdTrabajador);
GO



CREATE OR ALTER PROCEDURE spConsultarExpedienteTrabajador
    @IdTrabajador INT
AS
BEGIN
    SET NOCOUNT ON;

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
END
GO


CREATE OR ALTER PROCEDURE spGuardarExpedienteTrabajador
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
        FechaRegistro
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
        GETDATE()
    );

    SELECT
        1 AS Id,
        'Archivo agregado correctamente al expediente.' AS Nombre;
END
GO


CREATE OR ALTER PROCEDURE spReemplazarExpedienteTrabajador
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
END
GO



CREATE OR ALTER PROCEDURE spEliminarExpedienteTrabajador
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ExpedienteTrabajador
    SET
        Activo = 0,
        FechaModificacion = GETDATE()
    WHERE Id = @Id;

    SELECT
        1 AS Id,
        'Archivo eliminado correctamente del expediente.' AS Nombre;
END
GO

-------------------------------------------

CREATE TABLE CertificacionAnulacion
(
    Id INT IDENTITY(1,1) PRIMARY KEY,

    IdCertificacion INT NOT NULL,

    TipoAnulacion VARCHAR(50) NOT NULL,
    FechaInicio DATE NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    FechaFin DATE NULL,
    EsPermanente BIT NOT NULL DEFAULT 0,

    Comentario VARCHAR(500) NOT NULL,

    Activa BIT NOT NULL DEFAULT 1,

    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FechaModificacion DATETIME NULL,

    CONSTRAINT FK_CertificacionAnulacion_Certificacion
        FOREIGN KEY (IdCertificacion) REFERENCES Certificacion(Id)
);
GO

CREATE INDEX IX_CertificacionAnulacion_IdCertificacion
ON CertificacionAnulacion(IdCertificacion);
GO




CREATE OR ALTER PROCEDURE spConsultarAnulacionPorCertificacion
    @IdCertificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
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
      AND (
            EsPermanente = 1
            OR FechaFin IS NULL
            OR FechaFin >= CAST(GETDATE() AS DATE)
          )
    ORDER BY FechaRegistro DESC;
END
GO



CREATE OR ALTER PROCEDURE spGuardarCertificacionAnulacion
    @IdCertificacion INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM Certificacion WHERE Id = @IdCertificacion)
    BEGIN
        SELECT 0 AS Id, 'No se encontró la certificación seleccionada.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin IS NULL
    BEGIN
        SELECT 0 AS Id, 'Debe seleccionar una fecha fin para una anulación temporal.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin < @FechaInicio
    BEGIN
        SELECT 0 AS Id, 'La fecha fin no puede ser menor que la fecha inicio.' AS Nombre;
        RETURN;
    END

    IF LTRIM(RTRIM(ISNULL(@Comentario, ''))) = ''
    BEGIN
        SELECT 0 AS Id, 'Debe escribir un comentario de la anulación.' AS Nombre;
        RETURN;
    END

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
        FechaRegistro
    )
    VALUES
    (
        @IdCertificacion,
        @TipoAnulacion,
        @FechaInicio,
        CASE WHEN @EsPermanente = 1 THEN NULL ELSE @FechaFin END,
        @EsPermanente,
        @Comentario,
        1,
        GETDATE()
    );

    SELECT 1 AS Id, 'Certificación anulada correctamente.' AS Nombre;
END
GO



CREATE OR ALTER PROCEDURE spModificarCertificacionAnulacion
    @Id INT,
    @TipoAnulacion VARCHAR(50),
    @FechaInicio DATE,
    @FechaFin DATE = NULL,
    @EsPermanente BIT,
    @Comentario VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CertificacionAnulacion WHERE Id = @Id AND Activa = 1)
    BEGIN
        SELECT 0 AS Id, 'No se encontró una anulación activa para modificar.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin IS NULL
    BEGIN
        SELECT 0 AS Id, 'Debe seleccionar una fecha fin para una anulación temporal.' AS Nombre;
        RETURN;
    END

    IF @EsPermanente = 0 AND @FechaFin < @FechaInicio
    BEGIN
        SELECT 0 AS Id, 'La fecha fin no puede ser menor que la fecha inicio.' AS Nombre;
        RETURN;
    END

    IF LTRIM(RTRIM(ISNULL(@Comentario, ''))) = ''
    BEGIN
        SELECT 0 AS Id, 'Debe escribir un comentario de la anulación.' AS Nombre;
        RETURN;
    END

    UPDATE CertificacionAnulacion
    SET
        TipoAnulacion = @TipoAnulacion,
        FechaInicio = @FechaInicio,
        FechaFin = CASE WHEN @EsPermanente = 1 THEN NULL ELSE @FechaFin END,
        EsPermanente = @EsPermanente,
        Comentario = @Comentario,
        FechaModificacion = GETDATE()
    WHERE Id = @Id
      AND Activa = 1;

    SELECT 1 AS Id, 'Anulación modificada correctamente.' AS Nombre;
END
GO


CREATE OR ALTER PROCEDURE spEliminarCertificacionAnulacion
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM CertificacionAnulacion WHERE Id = @Id AND Activa = 1)
    BEGIN
        SELECT 0 AS Id, 'No se encontró una anulación activa para eliminar.' AS Nombre;
        RETURN;
    END

    UPDATE CertificacionAnulacion
    SET
        Activa = 0,
        FechaModificacion = GETDATE()
    WHERE Id = @Id;

    SELECT 1 AS Id, 'Anulación eliminada correctamente.' AS Nombre;
END
GO


-------------------------------------------

CREATE INDEX IX_Trabajador_IdLinea
ON Trabajador (IdLinea);

CREATE INDEX IX_Trabajador_IdTurno
ON Trabajador (IdTurno);

CREATE INDEX IX_Trabajador_IdLocalidad
ON Trabajador (IdLocalidad);

CREATE INDEX IX_Linea_IdPlanta
ON Linea (IdPlanta);

CREATE INDEX IX_Certificador_IdTrabajador
ON Certificador (IdTrabajador);

CREATE INDEX IX_Certificacion_IdTrabajador
ON Certificacion (IdTrabajador);

CREATE INDEX IX_Certificacion_IdProceso
ON Certificacion (IdProceso);

CREATE INDEX IX_Certificacion_FechaVencimiento
ON Certificacion (FechaVencimiento);


INSERT INTO Trabajador
(NoReloj, Nombre, RutaFoto, IdLocalidad, IdTurno, IdLinea)
VALUES
(95001,'HERNANDEZ LOPEZ, JUAN CARLOS','C:\Aseguranza\95001_HERNANDEZ LOPEZ, JUAN CARLOS.jpg',1036,1005,1025),
(95002,'GARCIA RAMIREZ, ANA MARIA','C:\Aseguranza\95002_GARCIA RAMIREZ, ANA MARIA.jpg',1036,1005,1026),
(95003,'MARTINEZ SOTO, LUIS ANGEL','C:\Aseguranza\95003_MARTINEZ SOTO, LUIS ANGEL.jpg',1036,1005,1027),
(95004,'PEREZ CASTILLO, MARIA ELENA','C:\Aseguranza\95004_PEREZ CASTILLO, MARIA ELENA.jpg',1036,1005,1028),
(95005,'LOPEZ HERNANDEZ, JOSE ANTONIO','C:\Aseguranza\95005_LOPEZ HERNANDEZ, JOSE ANTONIO.jpg',1036,1005,1030),

(95006,'RAMOS FLORES, KARLA ITZEL','C:\Aseguranza\95006_RAMOS FLORES, KARLA ITZEL.jpg',1036,1005,1031),
(95007,'CRUZ GOMEZ, MIGUEL ANGEL','C:\Aseguranza\95007_CRUZ GOMEZ, MIGUEL ANGEL.jpg',1036,1005,1032),
(95008,'MENDOZA TORRES, DIANA PAOLA','C:\Aseguranza\95008_MENDOZA TORRES, DIANA PAOLA.jpg',1036,1005,1034),
(95009,'SALAZAR VEGA, ROBERTO','C:\Aseguranza\95009_SALAZAR VEGA, ROBERTO.jpg',1036,1005,1025),
(95010,'ORTIZ NAVARRO, CLAUDIA','C:\Aseguranza\95010_ORTIZ NAVARRO, CLAUDIA.jpg',1036,1005,1026),

-- -------- 20
(95011,'CASTRO LUNA, FRANCISCO','C:\Aseguranza\95011_CASTRO LUNA, FRANCISCO.jpg',1036,1005,1027),
(95012,'VELAZQUEZ RIOS, PAOLA','C:\Aseguranza\95012_VELAZQUEZ RIOS, PAOLA.jpg',1036,1005,1028),
(95013,'AGUILAR MORALES, ERNESTO','C:\Aseguranza\95013_AGUILAR MORALES, ERNESTO.jpg',1036,1005,1030),
(95014,'SANCHEZ PACHECO, LAURA','C:\Aseguranza\95014_SANCHEZ PACHECO, LAURA.jpg',1036,1005,1031),
(95015,'ROMERO BARRERA, HUGO','C:\Aseguranza\95015_ROMERO BARRERA, HUGO.jpg',1036,1005,1032),

(95016,'ALVAREZ GUERRERO, MARTHA','C:\Aseguranza\95016_ALVAREZ GUERRERO, MARTHA.jpg',1036,1005,1034),
(95017,'NAVARRO CAMPOS, OSCAR','C:\Aseguranza\95017_NAVARRO CAMPOS, OSCAR.jpg',1036,1005,1025),
(95018,'IBARRA RANGEL, PATRICIA','C:\Aseguranza\95018_IBARRA RANGEL, PATRICIA.jpg',1036,1005,1026),
(95019,'FUENTES MEZA, ALBERTO','C:\Aseguranza\95019_FUENTES MEZA, ALBERTO.jpg',1036,1005,1027),
(95020,'MEDINA ORTEGA, SILVIA','C:\Aseguranza\95020_MEDINA ORTEGA, SILVIA.jpg',1036,1005,1028),

-- -------- 40
(95021,'RIVERA VALDEZ, JESUS','C:\Aseguranza\95021_RIVERA VALDEZ, JESUS.jpg',1036,1005,1030),
(95022,'NUNEZ CARRILLO, ROCIO','C:\Aseguranza\95022_NUNEZ CARRILLO, ROCIO.jpg',1036,1005,1031),
(95023,'VARGAS SALINAS, DANIEL','C:\Aseguranza\95023_VARGAS SALINAS, DANIEL.jpg',1036,1005,1032),
(95024,'MORALES REYES, TERESA','C:\Aseguranza\95024_MORALES REYES, TERESA.jpg',1036,1005,1034),
(95025,'PADILLA CERVANTES, ENRIQUE','C:\Aseguranza\95025_PADILLA CERVANTES, ENRIQUE.jpg',1036,1005,1025),

-- -------- 60
(95026,'LEON CONTRERAS, FERNANDO','C:\Aseguranza\95026_LEON CONTRERAS, FERNANDO.jpg',1036,1005,1026),
(95027,'PACHECO ZAMORA, LILIANA','C:\Aseguranza\95027_PACHECO ZAMORA, LILIANA.jpg',1036,1005,1027),
(95028,'ESTRADA ROSAS, JULIO','C:\Aseguranza\95028_ESTRADA ROSAS, JULIO.jpg',1036,1005,1028),
(95029,'ZAVALA MONTOYA, VERONICA','C:\Aseguranza\95029_ZAVALA MONTOYA, VERONICA.jpg',1036,1005,1030),
(95030,'ARIAS BELTRAN, EDUARDO','C:\Aseguranza\95030_ARIAS BELTRAN, EDUARDO.jpg',1036,1005,1031),

-- -------- 80
(95031,'MEJIA QUINTERO, NORMA','C:\Aseguranza\95031_MEJIA QUINTERO, NORMA.jpg',1036,1005,1032),
(95032,'CERVANTES MORA, ISRAEL','C:\Aseguranza\95032_CERVANTES MORA, ISRAEL.jpg',1036,1005,1034),
(95033,'GUZMAN SOLIS, ADRIANA','C:\Aseguranza\95033_GUZMAN SOLIS, ADRIANA.jpg',1036,1005,1025),
(95034,'ESCOBEDO MENA, RICARDO','C:\Aseguranza\95034_ESCOBEDO MENA, RICARDO.jpg',1036,1005,1026),
(95035,'MIRANDA SERRANO, FABIOLA','C:\Aseguranza\95035_MIRANDA SERRANO, FABIOLA.jpg',1036,1005,1027),

-- -------- 100
(95036,'OLIVARES VAZQUEZ, SAUL','C:\Aseguranza\95036_OLIVARES VAZQUEZ, SAUL.jpg',1036,1005,1028),
(95037,'TAPIA RODRIGUEZ, NOEMI','C:\Aseguranza\95037_TAPIA RODRIGUEZ, NOEMI.jpg',1036,1005,1030),
(95038,'CORTEZ BECERRA, RAUL','C:\Aseguranza\95038_CORTEZ BECERRA, RAUL.jpg',1036,1005,1031),
(95039,'SOTO DELGADO, VANESSA','C:\Aseguranza\95039_SOTO DELGADO, VANESSA.jpg',1036,1005,1032),
(95040,'PALACIOS ROJAS, MARIO','C:\Aseguranza\95040_PALACIOS ROJAS, MARIO.jpg',1036,1005,1034);

SET NOCOUNT ON;

DECLARE @i INT = 1;
DECLARE @NoReloj INT = 110000; -- AJUSTA SI ES NECESARIO

WHILE @i <= 100000
BEGIN
    INSERT INTO Trabajador
    (
        NoReloj,
        Nombre,
        RutaFoto,
        IdLocalidad,
        IdTurno,
        IdLinea
    )
    VALUES
    (
        CAST(@NoReloj AS VARCHAR(10)),
        CONCAT('TRABAJADOR PRUEBA ', @i),
        CONCAT('C:\Aseguranza\', @NoReloj, '_EMPLEADO PRUEBA ', @i, '.jpg'),
        1036, -- IdLocalidad
        1005, -- IdTurno
        CASE (@i % 8)
            WHEN 0 THEN 1025
            WHEN 1 THEN 1026
            WHEN 2 THEN 1027
            WHEN 3 THEN 1028
            WHEN 4 THEN 1030
            WHEN 5 THEN 1031
            WHEN 6 THEN 1032
            WHEN 7 THEN 1034
        END
    );

    SET @i += 1;
    SET @NoReloj += 1;
END;




SET NOCOUNT ON;

DECLARE @Total INT = 100;
DECLARE @i INT = 1;

DECLARE @IdTrabajador INT;
DECLARE @IdProceso INT;
DECLARE @IdCertificador INT;
DECLARE @FechaCert DATE;
DECLARE @Vigencia INT;

-- Cursores simples (controlados)
DECLARE Trabajadores CURSOR FOR
SELECT Id FROM Trabajador;

DECLARE Procesos CURSOR FOR
SELECT Id, VigenciaMeses FROM Proceso;

DECLARE Certificadores CURSOR FOR
SELECT Id FROM Certificador;

OPEN Trabajadores;
OPEN Procesos;
OPEN Certificadores;

FETCH NEXT FROM Trabajadores INTO @IdTrabajador;
FETCH NEXT FROM Procesos INTO @IdProceso, @Vigencia;
FETCH NEXT FROM Certificadores INTO @IdCertificador;

WHILE @i <= @Total
BEGIN
    IF @IdTrabajador IS NULL
    BEGIN
        FETCH NEXT FROM Trabajadores INTO @IdTrabajador;
        CONTINUE;
    END

    IF @IdProceso IS NULL
    BEGIN
        CLOSE Procesos;
        OPEN Procesos;
        FETCH NEXT FROM Procesos INTO @IdProceso, @Vigencia;
    END

    IF @IdCertificador IS NULL
    BEGIN
        CLOSE Certificadores;
        OPEN Certificadores;
        FETCH NEXT FROM Certificadores INTO @IdCertificador;
    END

    IF NOT EXISTS
    (
        SELECT 1
        FROM Certificacion
        WHERE IdTrabajador = @IdTrabajador
          AND IdProceso = @IdProceso
    )
    BEGIN
        SET @FechaCert = DATEADD(DAY, -(@i % 3), GETDATE());

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
            @FechaCert,
            DATEADD(MONTH, @Vigencia, @FechaCert),
            @IdCertificador,
            CONCAT('Certificación de prueba ', @i)
        );

        SET @i += 1;
    END

    FETCH NEXT FROM Trabajadores INTO @IdTrabajador;
    FETCH NEXT FROM Procesos INTO @IdProceso, @Vigencia;
    FETCH NEXT FROM Certificadores INTO @IdCertificador;
END

CLOSE Trabajadores;
CLOSE Procesos;
CLOSE Certificadores;

DEALLOCATE Trabajadores;
DEALLOCATE Procesos;
DEALLOCATE Certificadores;
